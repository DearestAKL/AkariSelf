using System;
using System.Collections.Generic;
using UnityEngine;

namespace Akari
{
    public class UIManager : GameFrameworkModule
    {
        private Dictionary<string, UIGroup> m_UIGroups = new Dictionary<string, UIGroup>();
        private IUIHelper m_UIResourceHelper;

        public override int Priority
        {
            get
            {
                return 2;
            }
        }

        public override void Shutdown()
        {
            m_UIGroups.Clear();
            m_UIResourceHelper = null;
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroups)
            {
                uiGroup.Value.OnUpdate(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uIResourceHelper"></param>
        public void SetUIResourceHelper(IUIHelper uIResourceHelper)
        {
            m_UIResourceHelper = uIResourceHelper;
        }

        /// <summary>
        /// 添加界面组
        /// </summary>
        /// <param name="uiGroupName"></param>
        /// <param name="uiGroupDepth"></param>
        /// <param name="parent"></param>
        public void AddUIGroup(string uiGroupName,int uiGroupDepth,Transform parent)
        {
            m_UIGroups.Add(uiGroupName, new UIGroup(uiGroupName, uiGroupDepth, parent, m_UIResourceHelper));
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
