using UnityEngine;
using UnityEngine.UI;

namespace Akari
{
    public class UIPanel : IUIPanel
    {
        private GameObject m_RootGo;
        private Canvas m_CachedCanvas = null;
        private CanvasGroup m_CanvasGroup = null;
        private string m_Name = null;

        public GameObject RootGo
        {
            get
            {
                return m_RootGo;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public void OnInit(string name,GameObject go,object userData)
        {
            m_Name = name;
            m_RootGo = go;
            m_CachedCanvas = m_RootGo.GetOrAddComponent<Canvas>();
            m_CachedCanvas.overrideSorting = true;

            m_CanvasGroup = m_RootGo.GetOrAddComponent<CanvasGroup>();

            RectTransform transform = m_RootGo.GetComponent<RectTransform>();
            transform.localScale = Vector3.one;
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
            transform.anchoredPosition3D = Vector3.zero;

            m_RootGo.GetOrAddComponent<GraphicRaycaster>();
        }

        public void OnOpen(object userData)
        {
            m_CanvasGroup.alpha = 1f;
            m_CanvasGroup.blocksRaycasts = true;
        }

        public void OnClose()
        {
            m_CanvasGroup.alpha = 0f;
            m_CanvasGroup.blocksRaycasts = false;
        }

        public void OnPause()
        {

        }

        public void OnRecycle()
        {
            GameObject.Destroy(m_RootGo);
        }

        public void OnResume()
        {

        }

        public void OnUpdate()
        {

        }
    }
}
