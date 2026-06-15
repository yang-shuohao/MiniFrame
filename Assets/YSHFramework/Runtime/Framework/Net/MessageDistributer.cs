using Common.Message;
using System;
using System.Collections.Generic;
using System.Threading;

namespace YSH.Framework
{
    /// <summary>
    /// 消息分发器（object 版）
    /// </summary>
    public class MessageDistributer : MessageDistributer<object> { }

    /// <summary>
    /// 泛型消息分发器
    /// </summary>
    /// <typeparam name="T">消息发送者类型</typeparam>
    public class MessageDistributer<T> : Singleton<MessageDistributer<T>>
    {
        /// <summary>
        /// 消息包结构
        /// </summary>
        private class MessageArgs
        {
            public T sender;
            public NetMessage message;
        }

        private readonly Queue<MessageArgs> messageQueue = new Queue<MessageArgs>();
        private readonly AutoResetEvent threadEvent = new AutoResetEvent(false);
        private readonly object queueLock = new object();

        private readonly Dictionary<string, Delegate> messageHandlers = new Dictionary<string, Delegate>();

        private volatile bool running = false;
        private int threadCount = 0;
        private int activeThreadCount = 0;

        /// <summary>
        /// 当前活跃的工作线程数量（线程安全）
        /// </summary>
        public int ActiveThreadCount => Interlocked.CompareExchange(ref activeThreadCount, 0, 0);

        /// <summary>
        /// 是否在处理事件时抛出异常
        /// </summary>
        public bool ThrowException { get; set; } = false;

        /// <summary>
        /// 订阅消息处理器
        /// </summary>
        public void Subscribe<Tm>(MessageHandler<Tm> messageHandler)
        {
            string type = typeof(Tm).Name;
            if (!messageHandlers.ContainsKey(type))
            {
                messageHandlers[type] = null;
            }
            messageHandlers[type] = (MessageHandler<Tm>)messageHandlers[type] + messageHandler;
        }

        /// <summary>
        /// 取消订阅消息处理器
        /// </summary>
        public void Unsubscribe<Tm>(MessageHandler<Tm> messageHandler)
        {
            string type = typeof(Tm).Name;
            if (messageHandlers.ContainsKey(type))
            {
                messageHandlers[type] = (MessageHandler<Tm>)messageHandlers[type] - messageHandler;
            }
        }

        /// <summary>
        /// 手动触发消息分发（直接调用）
        /// </summary>
        public void RaiseEvent<Tm>(T sender, Tm msg)
        {
            string key = msg.GetType().Name;
            if (messageHandlers.TryGetValue(key, out var handler) && handler is MessageHandler<Tm> casted)
            {
                try
                {
                    casted(sender, msg);
                }
                catch (Exception ex)
                {
                    LogMgr.Instance.LogError($"Message handler exception: {ex}");
                    if (ThrowException) throw;
                }
            }
            else
            {
                LogMgr.Instance.LogWarning($"No handler subscribed for message type: {key}");
            }
        }

        /// <summary>
        /// 接收网络消息（入队）
        /// </summary>
        public void ReceiveMessage(T sender, NetMessage message)
        {
            lock (queueLock)
            {
                messageQueue.Enqueue(new MessageArgs { sender = sender, message = message });
            }
            threadEvent.Set();
        }

        /// <summary>
        /// 清空消息队列
        /// </summary>
        public void Clear()
        {
            lock (queueLock)
            {
                messageQueue.Clear();
            }
        }

        /// <summary>
        /// 单线程模式：同步分发队列中所有消息
        /// </summary>
        public void Distribute()
        {
            while (true)
            {
                MessageArgs package = null;
                lock (queueLock)
                {
                    if (messageQueue.Count > 0)
                        package = messageQueue.Dequeue();
                    else
                        break;
                }

                DispatchMessage(package);
            }
        }

        /// <summary>
        /// 启动多线程分发器
        /// </summary>
        public void Start(int threadNum)
        {
            threadCount = Math.Clamp(threadNum, 1, 1000);
            running = true;

            for (int i = 0; i < threadCount; i++)
            {
                ThreadPool.QueueUserWorkItem(MessageDistribute);
            }

            LogMgr.Instance.Log($"MessageDistributer started with {threadCount} threads.");
        }

        /// <summary>
        /// 停止多线程分发器
        /// </summary>
        public void Stop()
        {
            running = false;
            Clear();

            for (int i = 0; i < threadCount; i++)
            {
                threadEvent.Set(); // 唤醒所有等待线程
            }

            LogMgr.Instance.Log("MessageDistributer stopping...");
        }

        /// <summary>
        /// 消息处理线程入口
        /// </summary>
        private void MessageDistribute(object state)
        {
            LogMgr.Instance.Log("MessageDistribute thread started.");
            Interlocked.Increment(ref activeThreadCount);

            try
            {
                while (running)
                {
                    MessageArgs package = null;

                    lock (queueLock)
                    {
                        if (messageQueue.Count > 0)
                            package = messageQueue.Dequeue();
                    }

                    if (package != null)
                    {
                        DispatchMessage(package);
                    }
                    else
                    {
                        threadEvent.WaitOne(); // 等待有新消息
                    }
                }
            }
            catch (Exception ex)
            {
                LogMgr.Instance.LogError($"MessageDistribute thread exception: {ex}");
            }
            finally
            {
                Interlocked.Decrement(ref activeThreadCount);
                LogMgr.Instance.Log("MessageDistribute thread ended.");
            }
        }

        /// <summary>
        /// 调用分发器进行消息处理
        /// </summary>
        private void DispatchMessage(MessageArgs package)
        {
            if (package.message.Request != null)
                MessageDispatch<T>.Instance.Dispatch(package.sender, package.message.Request);

            if (package.message.Response != null)
                MessageDispatch<T>.Instance.Dispatch(package.sender, package.message.Response);
        }

        /// <summary>
        /// 消息处理代理类型
        /// </summary>
        public delegate void MessageHandler<Tm>(T sender, Tm message);
    }
}
