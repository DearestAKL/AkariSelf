using System.Collections.Generic;

namespace Akari
{
    public class EventComponent : GameFrameworkComponent
    {
        public delegate void EventHandler(GameEvent gameEvent);

        private Dictionary<int, EventHandler> m_EventHandlerMap = new Dictionary<int, EventHandler>();


        protected override void Awake()
        {
            base.Awake();
        }


        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="eventHandler"></param>
        public void Subscribe(EventType eventType, EventHandler eventHandler)
        {
            if (m_EventHandlerMap == null)
            {
                m_EventHandlerMap = new Dictionary<int, EventHandler>();
            }
            int eventTypeID = (int)eventType;
            if (m_EventHandlerMap.ContainsKey(eventTypeID))
            {
                m_EventHandlerMap[eventTypeID] += eventHandler;
            }
            else
            {
                m_EventHandlerMap.Add(eventTypeID, eventHandler);
            }
        }

        /// <summary>
        /// 取消事件订阅
        /// 该EventType下所有的注册消息
        /// </summary>
        public void Unsubscribe(EventType eventType)
        {
            int eventTypeID = (int)eventType;

            if (m_EventHandlerMap == null)
            {
                return;
            }

            if (m_EventHandlerMap.ContainsKey(eventTypeID))
            {
                m_EventHandlerMap.Remove(eventTypeID);
            }
        }
        /// <summary>
        /// 取消事件订阅
        /// 反注册该EventType下指定的EventHandler
        /// </summary>
        public void Unsubscribe(EventType eventType, EventHandler eventHandler)
        {
            int eventTypeID = (int)eventType;

            if (m_EventHandlerMap == null)
            {
                return;
            }
            //删除eventHandler指定的消息响应
            if (m_EventHandlerMap.ContainsKey(eventTypeID))
            {
                m_EventHandlerMap[eventTypeID] -= eventHandler;
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        private void Fire(EventType eventType, GameEvent gameEvent)
        {
            int eventTypeID = (int)eventType;

            if (m_EventHandlerMap == null)
            {
                return;
            }

            if (m_EventHandlerMap.ContainsKey(eventTypeID))
            {
                m_EventHandlerMap[eventTypeID]?.Invoke(gameEvent);
            }
        }

        /// <summary>
        /// 触发事件 带参数
        /// </summary>
        public void Fire(EventType eventType, params object[] inParams)
        {
            GameEvent gameEvent = new GameEvent(eventType, inParams);
            Fire(eventType, gameEvent);
        }

        /// <summary>
        /// 触发事件 无参数
        /// </summary>
        public void Fire(EventType eventType)
        {
            GameEvent gameEvent = new GameEvent();
            Fire(eventType, gameEvent);

        }
    }
}
