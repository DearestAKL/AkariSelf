using System.Collections.Generic;
using UnityEngine;

namespace VEngine
{
    public class PlayerSettings : ScriptableObject
    {
        /// <summary>
        ///     资源下载地址，指向平台目录的父目录
        /// </summary>
        [Tooltip("资源下载地址，指向平台目录的父目录")] public string downloadURL = "http://127.0.0.1/Bundles/";

        public List<string> assets = new List<string>();

        /// <summary>
        ///     初始化的清单配置，配置包外的清单，底层会自动按需更新下载
        /// </summary>
        [Tooltip("初始化的清单配置，配置包外的清单，底层会自动按需更新下载")]
        public List<string> manifests = new List<string>();
    }
}