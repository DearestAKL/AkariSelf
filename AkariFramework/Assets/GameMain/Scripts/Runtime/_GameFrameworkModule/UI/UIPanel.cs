using UnityEngine;
using UnityEngine.UI;

namespace Akari.UI
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

        public virtual void OnInit(string name,GameObject go,Transform parent,object userData)
        {
            m_Name = name;
            m_RootGo = go;
            m_RootGo.transform.SetParent(parent);
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

        public virtual void OnOpen(object userData)
        {
            m_CanvasGroup.alpha = 1f;
            m_CanvasGroup.blocksRaycasts = true;
        }

        public virtual void OnClose()
        {
            m_CanvasGroup.alpha = 0f;
            m_CanvasGroup.blocksRaycasts = false;
        }

        public virtual void OnPause()
        {

        }

        public virtual void OnRecycle()
        {
            GameObject.Destroy(m_RootGo);
        }

        public virtual void OnResume()
        {

        }

        public virtual void OnUpdate()
        {

        }
    }
}
