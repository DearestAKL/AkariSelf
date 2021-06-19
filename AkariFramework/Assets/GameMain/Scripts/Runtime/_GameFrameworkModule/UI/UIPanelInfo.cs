namespace Akari
{
    public sealed class UIPanelInfo
    {
        private UIPanel m_UIPanel;
        private bool m_Paused;
        private bool m_Covered;

        public UIPanelInfo()
        {
            m_UIPanel = null;
            m_Paused = false;
            m_Covered = false;
        }

        public UIPanel UIPanel
        {
            get
            {
                return m_UIPanel;
            }
        }

        public bool Paused
        {
            get
            {
                return m_Paused;
            }
            set
            {
                m_Paused = value;
            }
        }

        public bool Covered
        {
            get
            {
                return m_Covered;
            }
            set
            {
                m_Covered = value;
            }
        }

        public static UIPanelInfo Create(UIPanel uiPanel)
        {

            UIPanelInfo uiFormInfo = new UIPanelInfo();
            uiFormInfo.m_UIPanel = uiPanel;
            uiFormInfo.m_Paused = true;
            uiFormInfo.m_Covered = true;
            return uiFormInfo;
        }

        public void Clear()
        {
            m_UIPanel = null;
            m_Paused = false;
            m_Covered = false;
        }
    }
}
