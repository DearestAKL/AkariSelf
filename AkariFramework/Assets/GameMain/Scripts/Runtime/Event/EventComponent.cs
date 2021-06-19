using System;
using System.Collections.Generic;

using EventHandler = Akari.EventManager.EventHandler;

namespace Akari
{
    public class EventComponent : GameFrameworkComponent
    {
        private EventManager m_EventManager;

        protected override void Awake()
        {
            base.Awake();

            m_EventManager = GameFrameworkEntry.GetModule<EventManager>();
        }


        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="eventHandler"></param>
        public void Subscribe(EventType eventType, Akari.EventManager.EventHandler eventHandler)
        {
            m_EventManager.Subscribe(eventType, eventHandler);
        }

        /// <summary>
        /// 取消事件订阅
        /// 该EventType下所有的注册消息
        /// </summary>
        public void Unsubscribe(EventType eventType)
        {
            m_EventManager.Unsubscribe(eventType);
        }
        /// <summary>
        /// 取消事件订阅
        /// 反注册该EventType下指定的EventHandler
        /// </summary>
        public void Unsubscribe(EventType eventType, EventHandler eventHandler)
        {
            m_EventManager.Unsubscribe(eventType, eventHandler);
        }


        /// <summary>
        /// 触发事件 带参数
        /// </summary>
        public void Fire(EventType eventType, params object[] inParams)
        {
            GameEvent gameEvent = new GameEvent(eventType, inParams);
            m_EventManager.Fire(eventType, gameEvent);
        }

        /// <summary>
        /// 触发事件 无参数
        /// </summary>
        public void Fire(EventType eventType)
        {
            GameEvent gameEvent = new GameEvent();
            m_EventManager.Fire(eventType, gameEvent);

        }
    }
}
