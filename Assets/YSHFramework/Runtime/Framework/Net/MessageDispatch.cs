
using Common.Message;

namespace YSH.Framework
{
    public class MessageDispatch<T> : Singleton<MessageDispatch<T>>
    {
        public void Dispatch(T sender, NetMessageResponse message)
        {
            if (message.UserRegister != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.UserRegister); }
        }

        public void Dispatch(T sender, NetMessageRequest message)
        {
            if (message.UserRegister != null) { MessageDistributer<T>.Instance.RaiseEvent(sender, message.UserRegister); }
        }
    }
}

