using System;
using UnityEngine;

namespace Akari
{
    public interface IUIGroup
    {
        /// <summary>
        /// 获取界面组类型。
        /// </summary>
        string GroupName { get; }

        /// <summary>
        /// 获取或设置界面组深度。
        /// </summary>
        int Depth { get; set; }

        /// <summary>
        /// 获取或设置界面组是否暂停。
        /// </summary>
        bool IsPause { get; set; }

        /// <summary>
        /// 获取界面组中界面数量。
        /// </summary>
        int UIPanelCount { get; }

        /// <summary>
        /// 界面组初始化
        /// </summary>
        /// <param name="parent"></param>
        void OnInit(Transform parent);

        /// <summary>
        /// 界面组轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        void OnUpdate(float elapseSeconds, float realElapseSecond);

        #region 界面相关

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userData"></param>
        void OpenUIPanel(string name,object userData);

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="name"></param>
        void CloseUIPanel(string name);

        /// <summary>
        /// 回收界面
        /// </summary>
        /// <param name="name"></param>
        void RecycleUIPanel(string name);

        #endregion 界面相关
    }
}
