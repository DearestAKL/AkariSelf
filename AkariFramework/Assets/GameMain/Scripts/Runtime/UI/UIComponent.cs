using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Akari
{
    public class UIComponent: GameFrameworkComponent
    {
        [SerializeField]
        private Camera m_UICamera;

        private Transform m_RootTransCache;

        private UIManager m_UIManager;

        private IUIHelper m_UIResourceHelper;


        protected override void Awake()
        {
            base.Awake();

            m_UIManager = GameFrameworkEntry.GetModule<UIManager>();

            m_UIResourceHelper = new UIHelper(m_UICamera);

            m_UIManager.SetUIResourceHelper(m_UIResourceHelper);

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
}
