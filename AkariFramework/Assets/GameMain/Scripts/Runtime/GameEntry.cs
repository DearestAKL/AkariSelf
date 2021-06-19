using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Akari
{
    public class GameEntry : MonoBehaviour
    {
        [SerializeField]
        private Camera m_UICamera;
        [SerializeField][Header("目标帧率")]
        private int targetFrameRate = 120;

        public void Awake()
        {
            UI.OpenUIPanel("UITest", UIGroupType.Base);
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        private void InitConfig()
        {
            UICamera = m_UICamera;
            Application.targetFrameRate = targetFrameRate;
        }

        /// <summary>
        /// 这里注册组件
        /// </summary>
        private void InitBuiltinComponents()
        {
            Event = GameFrameworkComponentEntry.GetComponent<EventComponent>();
            ObjectPool = GameFrameworkComponentEntry.GetComponent<ObjectPoolComponent>();
            UI = GameFrameworkComponentEntry.GetComponent<UIComponent>();
        }

        public static Camera UICamera
        {
            get;
            private set;
        }

        public static ResourceComponent Resource
        {
            get;
            private set;
        }

        public static EventComponent Event
        {
            get;
            private set;
        }

        public static ObjectPoolComponent ObjectPool
        {
            get;
            private set;
        }

        public static UIComponent UI
        {
            get;
            private set;
        }
    }
}