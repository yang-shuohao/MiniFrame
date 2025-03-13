using System.Collections.Generic;
using UnityEngine.Events;

namespace YSH.Framework
{
    public interface IEventInfo
    {

    }

    public class EventInfo : IEventInfo
    {
        public UnityAction actions;

        public EventInfo(UnityAction action)
        {
            actions += action;
        }
    }

    public class EventInfo<T> : IEventInfo
    {
        public UnityAction<T> actions;

        public EventInfo(UnityAction<T> action)
        {
            actions += action;
        }
    }

    public class EventInfo<T1,T2> : IEventInfo
    {
        public UnityAction<T1, T2> actions;

        public EventInfo(UnityAction<T1, T2> action)
        {
            actions += action;
        }
    }

    public class EventInfo<T1, T2, T3> : IEventInfo
    {
        public UnityAction<T1, T2, T3> actions;

        public EventInfo(UnityAction<T1, T2, T3> action)
        {
            actions += action;
        }
    }

    public class EventMgr : Singleton<EventMgr>
    {
        private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

        public void AddEventListener(string name, UnityAction action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo).actions += action;
            }
            else
            {
                eventDic.Add(name, new EventInfo(action));
            }
        }

        public void AddEventListener<T>(string name, UnityAction<T> action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T>).actions += action;
            }
            else
            {
                eventDic.Add(name, new EventInfo<T>(action));
            }
        }

        public void AddEventListener<T1, T2>(string name, UnityAction<T1, T2> action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T1, T2>).actions += action;
            }
            else
            {
                eventDic.Add(name, new EventInfo<T1, T2>(action));
            }
        }

        public void AddEventListener<T1, T2, T3>(string name, UnityAction<T1, T2, T3> action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T1, T2, T3>).actions += action;
            }
            else
            {
                eventDic.Add(name, new EventInfo<T1, T2, T3>(action));
            }
        }

        public void RemoveEventListener(string name, UnityAction action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo).actions -= action;
            }
        }

        public void RemoveEventListener<T>(string name, UnityAction<T> action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T>).actions -= action;
            }
        }

        public void RemoveEventListener<T1,T2>(string name, UnityAction<T1, T2> action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T1, T2>).actions -= action;
            }
        }

        public void RemoveEventListener<T1, T2, T3>(string name, UnityAction<T1, T2, T3> action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T1, T2, T3>).actions -= action;
            }
        }

        public void EventDispatcher(string name)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo).actions?.Invoke();
            }
        }

        public void EventDispatcher<T>(string name, T param)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T>).actions?.Invoke(param);
            }
        }

        public void EventDispatcher<T1,T2>(string name, T1 param1, T2 param2)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T1, T2>).actions?.Invoke(param1, param2);
            }
        }

        public void EventDispatcher<T1, T2, T3>(string name, T1 param1, T2 param2, T3 param3)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T1, T2, T3>).actions?.Invoke(param1, param2, param3);
            }
        }

        public void Clear()
        {
            eventDic.Clear();
        }
    }
}

