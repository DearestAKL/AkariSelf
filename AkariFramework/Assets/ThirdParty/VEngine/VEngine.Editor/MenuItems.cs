using System.IO;
using UnityEditor;
using UnityEngine;

namespace VEngine.Editor
{
    /// <summary>
    ///     编辑器菜单工具
    /// </summary>
    public static class MenuItems
    {
        // /// <summary>
        // ///     l
        // ///     打包分组
        // /// </summary>
        // [MenuItem("XASSET/Build/Groups")]
        // public static void BuildGroups()
        // {
        //     BuildScript.BuildGroups();
        // }

        /// <summary>
        ///     查看选中资源的 crc
        /// </summary>
        [MenuItem("XASSET/Compute CRC")]
        public static void ComputeCRC()
        {
            Logger.T(action: () =>
            {
                var target = Selection.activeObject;
                var path = AssetDatabase.GetAssetPath(target);
                var crc32 = Utility.ComputeCRC32(File.OpenRead(path));
                Logger.W("{0}={1}", path, crc32);
            }, name: "ComputeCRC");
        }


        /// <summary>
        ///     打包资源
        /// </summary>
        [MenuItem("XASSET/Build/Bundles")]
        public static void BuildBundles()
        {
            BuildScript.BuildBundles();
        }


        /// <summary>
        ///     打包播放器
        /// </summary>
        [MenuItem("XASSET/Build/Player")]
        public static void BuildPlayer()
        {
            BuildScript.BuildPlayer();
        }

        /// <summary>
        ///     复制路径
        /// </summary>
        [MenuItem("XASSET/Build/Copy To StreamingAssets")]
        public static void CopyToStreamingAssets()
        {
            BuildScript.CopyToStreamingAssets();
        }

        /// <summary>
        ///     清理所有数据
        /// </summary>
        [MenuItem("XASSET/Build/Clear")]
        public static void Clear()
        {
            BuildScript.Clear();
        }

        [MenuItem("XASSET/Build/Clear History")]
        public static void ClearHistory()
        {
            BuildScript.ClearHistory();
        }

        /// <summary>
        ///     查看打包的资源设置
        /// </summary>
        [MenuItem("XASSET/View/Settings")]
        public static void ViewSettings()
        {
            EditorUtility.PingWithSelected(Settings.GetDefaultSettings());
        }

        [MenuItem("XASSET/Clear Progress Bar")]
        public static void ClearProgressBar()
        {
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        ///     查看打包后的资源
        /// </summary>
        [MenuItem("XASSET/View/Build Path")]
        public static void ViewBuildPath()
        {
            UnityEditor.EditorUtility.OpenWithDefaultApp(EditorUtility.PlatformBuildPath);
        }

        /// <summary>
        ///     查看下载目录的资源
        /// </summary>
        [MenuItem("XASSET/View/Download Path")]
        public static void ViewDownloadPath()
        {
            UnityEditor.EditorUtility.OpenWithDefaultApp(Application.persistentDataPath);
        }

        /// <summary>
        ///     查看临时目录的资源
        /// </summary>
        [MenuItem("XASSET/View/Temporary")]
        public static void ViewTemporary()
        {
            UnityEditor.EditorUtility.OpenWithDefaultApp(Application.temporaryCachePath);
        }


        /// <summary>
        ///     复制路径
        /// </summary>
        [MenuItem("XASSET/Copy Path")]
        public static void CopyAssetPath()
        {
            EditorGUIUtility.systemCopyBuffer = AssetDatabase.GetAssetPath(Selection.activeObject);
        }
    }
}