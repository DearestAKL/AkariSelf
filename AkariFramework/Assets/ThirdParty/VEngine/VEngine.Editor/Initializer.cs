using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VEngine.Editor
{
    /// <summary>
    ///     初始化类，提供了编辑器的初始化操作
    /// </summary>
    public static class Initializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            var settings = Settings.GetDefaultSettings();
            Versions.DownloadDataPath = Path.Combine(Application.persistentDataPath, Utility.buildPath);
            Versions.PlatformName = EditorUtility.GetPlatformName();
            Versions.SkipUpdate = false;
            var config = settings.GetPlayerSettings();
            config.manifests = settings.manifests.ConvertAll(m => m.name);
            EditorUtility.SaveAsset(config);
            switch (settings.scriptPlayMode)
            {
                case ScriptPlayMode.Simulation:
                    Versions.FuncCreateAsset = EditorAsset.Create;
                    Versions.FuncCreateScene = EditorScene.Create;
                    Versions.FuncCreateManifest = EditorManifestFile.Create;
                    Versions.SkipUpdate = true;
                    DisableAutoUpdate();
                    break;
                case ScriptPlayMode.Preload:
                    Versions.PlayerDataPath =
                        Path.Combine(Environment.CurrentDirectory, EditorUtility.PlatformBuildPath);
                    Versions.SkipUpdate = true;
                    DisableAutoUpdate();
                    break;
                case ScriptPlayMode.Incremental:
                    if (! Directory.Exists(Path.Combine(Application.streamingAssetsPath, Versions.PlatformName)))
                    {
                        config.assets.Clear();  
                    } 
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void DisableAutoUpdate()
        {
            var startup = Object.FindObjectOfType<Startup>();
            if (startup != null)
            {
                startup.autoUpdate = false;
            }
        }
    }
}