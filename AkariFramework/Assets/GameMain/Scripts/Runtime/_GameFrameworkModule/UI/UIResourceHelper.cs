using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Akari
{
    public interface IUIHelper
    {
        /// <summary>
        /// 获取UI摄像机
        /// </summary>
        Camera UICamera
        {
            get;
        }

        /// <summary>
        /// 加载UI资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <returns></returns>
        GameObject LoadUIAsset(string path);

        /// <summary>
        /// 初始化UI类型映射
        /// </summary>
        void InitUITypeMap();

        /// <summary>
        /// 获取UIPanel的类型(Tips：需要先注册)
        /// </summary>
        /// <param name="name">资源名</param>
        /// <returns></returns>
        Type GetUIPanelType(string name);
    }
}
