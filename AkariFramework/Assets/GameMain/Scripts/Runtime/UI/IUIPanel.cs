using UnityEngine;

namespace Akari
{
    public interface IUIPanel
    {
        /// <summary>
        /// 初始化界面。
        /// </summary>
        void OnInit(string name, GameObject go, object userData);

        /// <summary>
        /// 界面回收。
        /// </summary>
        void OnRecycle();

        /// <summary>
        /// 界面打开。
        /// </summary>
        void OnOpen(object userData);

        /// <summary>
        /// 界面关闭。
        /// </summary>
        void OnClose();

        /// <summary>
        /// 界面暂停。
        /// </summary>
        void OnPause();

        /// <summary>
        /// 界面暂停恢复。
        /// </summary>
        void OnResume();

        /// <summary>
        /// 界面轮询。
        /// </summary>
        void OnUpdate();
    }
}
