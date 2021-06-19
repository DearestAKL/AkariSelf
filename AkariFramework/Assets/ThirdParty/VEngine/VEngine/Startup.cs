using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace VEngine
{
    /// <summary>
    ///     短链接加载模式
    /// </summary>
    public enum LoadMode
    {
        LoadByName,
        LoadByNameWithoutExtension,
        LoadByCustom
    }

    /// <summary>
    ///     Startup 类，主要用来对 Runtime 进行初始化。
    /// </summary>
    [RequireComponent(typeof(Updater))]
    [DisallowMultipleComponent]
    public class Startup : MonoBehaviour
    {
        [Tooltip("是否启动后更新服务器版本信息")] public bool autoUpdate;

        [Tooltip("是否开启日志")] public bool loggable;

        /// <summary>
        ///     通过关键字进行路径匹配，为路径生成短链接，可以按需使用
        /// </summary>
        [Tooltip("通过关键字进行路径匹配，为路径生成短链接，可以按需使用")]
        public string[] keys =
        {
            "Scenes", "Prefabs"
        };

        /// <summary>
        ///     加载模式，目前支持两种：LoadByName 使用扩展名，LoadByNameWithoutExtension 不使用扩展名
        /// </summary>
        [Tooltip("加载模式")] public LoadMode loadMode = LoadMode.LoadByNameWithoutExtension;

        /// <summary>
        ///     自定义加载器
        /// </summary>
        public UnityEvent onFinished;

        // Start is called before the first frame update
        private IEnumerator Start()
        {
            switch (loadMode)
            {
                case LoadMode.LoadByName:
                    Versions.customLoadPath = LoadByName;
                    break;
                case LoadMode.LoadByNameWithoutExtension:
                    Versions.customLoadPath = LoadByNameWithoutExtension;
                    break;
                case LoadMode.LoadByCustom:
                    Versions.customLoadPath = null;
                    break;
                default:
                    Versions.customLoadPath = null;
                    break;
            }

            DontDestroyOnLoad(gameObject);
            var operation = Versions.InitializeAsync();
            yield return operation;
            if (operation.status == OperationStatus.Failed)
            {
                Logger.E("Failed to initialize Runtime with error: {0}", operation.error);
            }
            else
            {
                Logger.I("Success to initialize Runtime with:");
            }

            Logger.I("API Version: {0}", Versions.APIVersion);
            Logger.I("Manifests Version: {0}", Versions.ManifestsVersion);
            Logger.I("PlayerDataPath: {0}", Versions.PlayerDataPath);
            Logger.I("DownloadDataPath: {0}", Versions.DownloadDataPath);
            Logger.I("DownloadURL: {0}", Versions.DownloadURL);

            if (autoUpdate)
            {
                // 这里用固定的名字加载最新的清单文件，生产环境这个可以去后台取。 
                var update = Versions.UpdateAsync(operation.manifests);
                yield return update;
                if (update.status == OperationStatus.Failed)
                {
                    Logger.E("Failed to initialize Runtime with error: {0}", update.error);
                }
                else
                {
                    Logger.I("Success to update versions with:");
                }
                update.Override();
                update.Dispose();
            }

            if (onFinished != null)
            {
                onFinished.Invoke();
            }
        }

        private void Update()
        {
            Logger.Loggable = loggable;
        }

        private string LoadByNameWithoutExtension(string assetPath)
        {
            if (keys == null || keys.Length == 0)
            {
                return null;
            }

            if (!Array.Exists(keys, assetPath.Contains))
            {
                return null;
            }

            var assetName = Path.GetFileNameWithoutExtension(assetPath);
            return assetName;
        }

        private string LoadByName(string assetPath)
        {
            if (keys == null || keys.Length == 0)
            {
                return null;
            }

            if (!Array.Exists(keys, assetPath.Contains))
            {
                return null;
            }

            var assetName = Path.GetFileName(assetPath);
            return assetName;
        }
    }
}