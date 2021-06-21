using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Akari
{
    public class GameEntry : MonoBehaviour
    {
        [SerializeField][Header("目标帧率")]
        private int targetFrameRate = 120;

        public void OnAwake()
        {
            InitConfig();
            InitBuiltinComponents();

            GameEntry.UI.OpenUIPanel(UIType.UILogin, UIGroupType.Base);
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        private void InitConfig()
        {
            Application.targetFrameRate = targetFrameRate;
        }

        /// <summary>
        /// 这里注册组件
        /// </summary>
        private void InitBuiltinComponents()
        {
            Event = GameFrameworkComponentEntry.GetComponent<EventComponent>();
            Resource = GameFrameworkComponentEntry.GetComponent<ResourceComponent>();
            UI = GameFrameworkComponentEntry.GetComponent<UIComponent>();
            ObjectPool = GameFrameworkComponentEntry.GetComponent<ObjectPoolComponent>();
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

        public static UIComponent UI
        {
            get;
            private set;
        }

        public static ObjectPoolComponent ObjectPool
        {
            get;
            private set;
        }
    }
}