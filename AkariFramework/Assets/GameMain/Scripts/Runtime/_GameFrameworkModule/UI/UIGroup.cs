using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Akari.UI
{
    public class UIGroup : IUIGroup
    {
        private readonly LinkedList<UIPanelInfo> m_UIPanelInfos;
        private LinkedListNode<UIPanelInfo> m_CachedNode;
        private bool m_Pause;
        private string m_GroupName;
        private int m_Depth;

        private GameObject m_RootGo;
        private Canvas m_Canvas;

        private IUIHelper m_UIHelper;

        public UIGroup(string groupName, int depth, Transform parent, IUIHelper uiResourceHelper)
        {
            m_GroupName = groupName;
            m_Depth = depth;
            m_UIHelper = uiResourceHelper;
            OnInit(parent);

            m_Pause = false;
            m_UIPanelInfos = new LinkedList<UIPanelInfo>();
            m_CachedNode = null;
        }

        /// <summary>
        /// 初始化 生成并缓存实体
        /// </summary>
        /// <param name="rootGo"></param>
        public void OnInit(Transform parentTrans) 
        {
            var depth = Depth;

            m_RootGo = new GameObject(m_GroupName);
            m_Canvas = m_RootGo.GetOrAddComponent<Canvas>();
            m_Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            m_Canvas.worldCamera = m_UIHelper.UICamera;
            m_RootGo.GetOrAddComponent<GraphicRaycaster>();

            Vector2 screenn = new Vector2(1920, 1080);
            CanvasScaler canvasScale = m_RootGo.GetOrAddComponent<CanvasScaler>();
            canvasScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScale.referenceResolution = screenn;
            canvasScale.matchWidthOrHeight = 1;

            m_Canvas.overrideSorting = true;
            m_Canvas.sortingOrder = depth;

            RectTransform transform = m_RootGo.GetComponent<RectTransform>();
            transform.SetParent(parentTrans);
            transform.localScale = Vector3.zero;
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
        }

        /// <summary>
        /// 界面组轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void OnUpdate(float elapseSeconds, float realElapseSecond)
        {
            LinkedListNode<UIPanelInfo> current = m_UIPanelInfos.First;
            while (current != null)
            {
                if (current.Value.Paused)
                {
                    break;
                }

                m_CachedNode = current.Next;
                current.Value.UIPanel.OnUpdate();
                current = m_CachedNode;
                m_CachedNode = null;
            }
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        public GameObject RootGO
        {
            get 
            { 
                return m_RootGo; 
            }
        }

        /// <summary>
        /// 获取界面组类型。
        /// </summary>
        public string GroupName
        {
            get
            {
                return m_GroupName;
            }
        }

        /// <summary>
        /// 获取或设置界面组深度。
        /// </summary>
        public int Depth
        {
            get
            {
                return m_Depth;
            }
            set
            {
                if (m_Depth == value)
                {
                    return;
                }

                m_Depth = value;
                m_Canvas.sortingOrder = m_Depth;
                //Refresh();
            }
        }

        /// <summary>
        /// 获取或设置界面组是否暂停。
        /// </summary>
        public bool IsPause
        {
            get
            {
                return m_Pause;
            }
            set
            {
                if (m_Pause == value)
                {
                    return;
                }

                m_Pause = value;
                //Refresh();
            }
        }

        /// <summary>
        /// 获取界面组中界面数量。
        /// </summary>
        public int UIPanelCount
        {
            get
            {
                return m_UIPanelInfos.Count;
            }
        }

        /// <summary>
        /// 获取当前界面。
        /// </summary>
        public UIPanel CurrentUIPanel
        {
            get
            {
                return m_UIPanelInfos.First != null ? m_UIPanelInfos.First.Value.UIPanel : null;
            }
        }

        /// <summary>
        /// 从界面组中获取界面。
        /// </summary>
        public UIPanel GetUIPanel(string uiPanelAssetName)
        {
            foreach (UIPanelInfo uiPanelInfo in m_UIPanelInfos)
            {
                if (uiPanelInfo.UIPanel.Name == uiPanelAssetName)
                {
                    return uiPanelInfo.UIPanel;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取界面信息
        /// </summary>
        /// <param name="uiPanel"></param>
        /// <returns></returns>
        private UIPanelInfo GetUIPanelInfo(UIPanel uiPanel)
        {

            foreach (UIPanelInfo uiPanelInfo in m_UIPanelInfos)
            {
                if (uiPanelInfo.UIPanel == uiPanel)
                {
                    return uiPanelInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="panelName"></param>
        public void OpenUIPanel(string panelName, object userData)
        {
            var uiPanel = GetUIPanel(panelName);
            if (uiPanel == null)
            {
                //没有 则创建
                var panel = m_UIHelper.LoadUIAsset(panelName);
                if(panel == null)
                {
                    return;
                }

                var type = m_UIHelper.GetUIPanelType(panelName);
                if (type == null)
                {
                    return;
                }

                uiPanel = (UIPanel)Activator.CreateInstance(type);
                uiPanel.OnInit(panelName, panel, m_RootGo.transform, userData);
                //缓存下来
                m_UIPanelInfos.AddFirst(UIPanelInfo.Create(uiPanel));
            }

            UIPanelInfo uiPanelInfo = GetUIPanelInfo(uiPanel);

            uiPanel.OnOpen(userData);
            if (uiPanelInfo.Paused)
            {
                uiPanel.OnResume();
                uiPanelInfo.Paused = false;
            }
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="panelName"></param>
        public void CloseUIPanel(string panelName)
        {
            var uiPanel = GetUIPanel(panelName);
            if (uiPanel == null)
            {
                //没有该界面
                return;
            }
            UIPanelInfo uiPanelInfo = GetUIPanelInfo(uiPanel);

            uiPanel.OnClose();
            if (!uiPanelInfo.Paused)
            {
                uiPanel.OnPause();
                uiPanelInfo.Paused = true;
            }
        }

        /// <summary>
        /// 回收界面
        /// </summary>
        /// <param name="panelName"></param>
        public void RecycleUIPanel(string panelName)
        {
            var uiPanel = GetUIPanel(panelName);
            if (uiPanel == null)
            {
                //没有该界面
                return;
            }

            UIPanelInfo uiPanelInfo = GetUIPanelInfo(uiPanel);
            if (m_CachedNode != null && m_CachedNode.Value.UIPanel == uiPanel)
            {
                m_CachedNode = m_CachedNode.Next;
            }

            uiPanelInfo.Clear();
            m_UIPanelInfos.Remove(uiPanelInfo);
        }

        /// <summary>
        /// 界面组刷新
        /// </summary>
        private void Refresh()
        {

        }
    }
}
