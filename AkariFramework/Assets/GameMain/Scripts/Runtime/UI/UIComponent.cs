using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Akari
{
    public class UIComponent: GameFrameworkComponent
    {
        private Transform m_RootTransCache;

        private UIManager m_UIManager;


        protected override void Awake()
        {
            base.Awake();

            m_UIManager = GameFrameworkEntry.GetModule<UIManager>();

            m_RootTransCache = new GameObject("UIRoot").transform;
            InitCanvas();
        }


        #region 初始化

        /// <summary>
        /// 初始化生成Canvas
        /// </summary>
        private void InitCanvas()
        {
            AddUIGroup(UIGroupType.Base, 0, m_RootTransCache);
            AddUIGroup(UIGroupType.Mid, 100, m_RootTransCache);
            AddUIGroup(UIGroupType.Top, 200, m_RootTransCache);
            AddUIGroup(UIGroupType.System, 300, m_RootTransCache);
        }

        #endregion

        #region 界面组

        /// <summary>
        /// 添加界面组
        /// </summary>
        /// <param name="uiGroupName"></param>
        /// <param name="uiGroupDepth"></param>
        /// <param name="parent"></param>
        public void AddUIGroup(string uiGroupName, int uiGroupDepth, Transform parent)
        {
            m_UIManager.AddUIGroup(uiGroupName, uiGroupDepth, parent);
        }

        #endregion

        #region 界面

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="panelName"></param>
        /// <param name="uiGroupName"></param>
        /// <returns></returns>
        public UIPanel GetUIPanel(string panelName, string uiGroupName)
        {
            return m_UIManager.GetUIPanel(panelName, uiGroupName);
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="panelName"></param>
        /// <param name="uiGroupName"></param>
        /// <param name="userData"></param>
        public void OpenUIPanel(string panelName, string uiGroupName, object userData = null)
        {
            m_UIManager.OpenUIPanel(panelName, uiGroupName, userData);
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="panelName"></param>
        /// <param name="uiGroupName"></param>
        public void CloseUIPanel(string panelName, string uiGroupName)
        {
            m_UIManager.CloseUIPanel(panelName, uiGroupName);
        }

        /// <summary>
        /// 回收界面
        /// </summary>
        /// <param name="panelName"></param>
        /// <param name="canvasType"></param>
        public void RecycleUIPanel(string panelName, string uiGroupName)
        {
            m_UIManager.RecycleUIPanel(panelName, uiGroupName);
        }
        #endregion
    }

    public class UIManager : GameFrameworkModule
    {
        private Dictionary<string, UIGroup> m_UIGroups = new Dictionary<string, UIGroup>();

        public override void Shutdown()
        {
            throw new NotImplementedException();
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                uiGroup.Value.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 添加界面组
        /// </summary>
        /// <param name="uiGroupName"></param>
        /// <param name="uiGroupDepth"></param>
        /// <param name="parent"></param>
        public void AddUIGroup(string uiGroupName,int uiGroupDepth,Transform parent)
        {
            m_UIGroups.Add(uiGroupName, new UIGroup(uiGroupName, uiGroupDepth, parent));
        }

        /// <summary>
        /// 获取界面组
        /// </summary>
        /// <param name="uiGroupName"></param>
        /// <returns></returns>
        public UIGroup GetUIGroup(string uiGroupName)
        {
            return m_UIGroups[uiGroupName];
        }

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="panelName"></param>
        /// <param name="uiGroupName"></param>
        /// <returns></returns>
        public UIPanel GetUIPanel(string panelName, string uiGroupName)
        {
            var group = GetUIGroup(uiGroupName);
            var uiPanel = group.GetUIPanel(panelName);
            return uiPanel;
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="panelName"></param>
        /// <param name="uiGroupName"></param>
        /// <param name="userData"></param>
        public void OpenUIPanel(string panelName, string uiGroupName, object userData = null)
        {
            var group = GetUIGroup(uiGroupName);
            group.OpenUIPanel(panelName, userData);
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="panelName"></param>
        /// <param name="uiGroupName"></param>
        public void CloseUIPanel(string panelName, string uiGroupName)
        {
            var group = GetUIGroup(uiGroupName);
            group.CloseUIPanel(panelName);
        }

        /// <summary>
        /// 回收界面
        /// </summary>
        /// <param name="panelName"></param>
        /// <param name="canvasType"></param>
        public void RecycleUIPanel(string panelName, string uiGroupName)
        {
            var group = GetUIGroup(uiGroupName);
            group.RecycleUIPanel(panelName);
        }
    }
}
