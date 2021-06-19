using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akari
{
    public class EventManager : GameFrameworkModule
    {
        public delegate void EventHandler(GameEvent gameEvent);

        private Dictionary<int, EventHandler> m_EventHandlerMap = new Dictionary<int, EventHandler>();

        public override int Priority
        {
            get
            {
                return 0;
            }
        }
        public override void Shutdown()
        {
            m_EventHandlerMap.Clear();
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {

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
        /// </summary>
        /// <param name="eventType">反注册的EventType</param>
        /// <param name="eventHandler">EventType下指定的EventHandler</param>
        public void Unsubscribe(EventType eventType, EventHandler eventHandler = null)
        {
            int eventTypeID = (int)eventType;

            if (m_EventHandlerMap == null)
            {
                return;
            }
            //删除eventHandler指定的消息响应
            if (m_EventHandlerMap.ContainsKey(eventTypeID))
            {
                if (eventHandler != null)
                {
                    // 移除指定EventHandler
                    m_EventHandlerMap[eventTypeID] -= eventHandler;
                }
                else
                {
                    // 移除所有EventHandler
                    m_EventHandlerMap.Remove(eventTypeID);
                }
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        public void Fire(EventType eventType, GameEvent gameEvent)
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
    }
}
