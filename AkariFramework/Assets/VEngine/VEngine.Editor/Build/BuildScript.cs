using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace VEngine.Editor
{
    /// <summary>
    ///     BuildScript 类，实现了具体的打包逻辑
    /// </summary>
    public static class BuildScript
    {
        public static Action<Manifest> preprocessBuildBundles { get; set; }
        public static Action<Manifest> postprocessBuildBundles { get; set; }

        /// <summary>
        ///     构建资源
        /// </summary>
        public static void BuildBundles()
        {
            var settings = Settings.GetDefaultSettings();
            var manifests = new List<string>();
            foreach (var manifest in settings.manifests)
            {
                manifests.Add(AssetDatabase.GetAssetPath(manifest));
            }
            foreach (var manifest in manifests)
            {
                BuildBundles(manifest);
            }
        }

        public static void BuildBundles(string manifestPath)
        {
            var asset = EditorUtility.GetOrCreateAsset<Manifest>(manifestPath);
            BuildBundles(asset);
        }

        public static void BuildBundles(Manifest manifest)
        {
            if (manifest == null)
            {
                return;
            }
            Logger.T(() =>
            {
                if (preprocessBuildBundles != null)
                {
                    preprocessBuildBundles.Invoke(manifest);
                }

                var builds = manifest.BuildGroups(out var bundleBuilds, out var rawBundleBuilds, out _);
                UnityEditor.EditorUtility.SetDirty(manifest);
                if (builds.Length > 0)
                {
                    var assetPath = AssetDatabase.GetAssetPath(manifest);
                    var platform = EditorUserBuildSettings.activeBuildTarget;
                    var outputFolder = EditorUtility.PlatformBuildPath;
                    var bundleOptions = manifest.buildAssetBundleOptions |
                                        BuildAssetBundleOptions.AppendHashToAssetBundleName;
                    var assetBundleManifest =
                        BuildPipeline.BuildAssetBundles(outputFolder, builds, bundleOptions, platform);
                    // 重新获取之前的版本文件，因为打包后，之前的内存数据会被 Unity 清空
                    manifest = EditorUtility.GetOrCreateAsset<Manifest>(assetPath);
                    if (assetBundleManifest == null)
                    {
                        Logger.E("Failed to build {0} with bundles, because assetBundleManifest == null.",
                            manifest.name);
                        return;
                    }

                    manifest.CreateVersions(assetBundleManifest, bundleBuilds);
                }
                else
                {
                    if (rawBundleBuilds.Count > 0 || manifest.GetBuild().GetBundles().Count != bundleBuilds.Count)
                    {
                        manifest.CreateVersions(null, bundleBuilds);
                    }
                    else
                    {
                        Logger.I("Nothing to build for {0}.", manifest.name);
                    }
                }

                if (postprocessBuildBundles != null)
                {
                    postprocessBuildBundles.Invoke(manifest);
                }
            }, $"Build Bundles for {manifest.name}");
        }

        /// <summary>
        ///     构建自动分组
        /// </summary>
        public static void BuildGroups()
        {
            var settings = Settings.GetDefaultSettings();
            foreach (var manifest in settings.manifests)
            {
                Logger.T(() => manifest.BuildGroups(out _, out _, out _), $"Build Groups for {manifest.name}");
            }
        }

        private static string GetTimeForNow()
        {
            return DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }

        /// <summary>
        ///     获取打包播放器的目标名字
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string GetBuildTargetName(BuildTarget target)
        {
            var productName = "xc" + "-v" + UnityEditor.PlayerSettings.bundleVersion + ".";
            var targetName = $"/{productName}-{GetTimeForNow()}";
            switch (target)
            {
                case BuildTarget.Android:
                    return targetName + ".apk";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return targetName + ".exe";
                case BuildTarget.StandaloneOSX:
                    return targetName + ".app";
                default:
                    return targetName;
            }
        }

        /// <summary>
        ///     打包播放器
        /// </summary>
        public static void BuildPlayer()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Build");
            if (path.Length == 0)
            {
                return;
            }

            var levels = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    levels.Add(scene.path);
                }
            }

            if (levels.Count == 0)
            {
                Logger.I("Nothing to build.");
                return;
            }

            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var buildTargetName = GetBuildTargetName(buildTarget);
            if (buildTargetName == null)
            {
                return;
            }

            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = levels.ToArray(),
                locationPathName = path + buildTargetName,
                target = buildTarget,
                options = EditorUserBuildSettings.development
                    ? BuildOptions.Development
                    : BuildOptions.None
            };
            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }

        public static void Clear()
        {
            Settings.GetDefaultSettings().Clear();
        }

        public static void CopyToStreamingAssets()
        {
            Settings.GetDefaultSettings().CopyToStreamingAssets();
        }

        public static void ClearHistory()
        {
            var settings = Settings.GetDefaultSettings();
            var usedFiles = new List<string>
            {
                EditorUtility.GetPlatformName(),
                EditorUtility.GetPlatformName() + ".manifest"
            };
            foreach (var manifest in settings.manifests)
            {
                var build = manifest.GetBuild();
                usedFiles.Add(manifest.name + ".json");
                usedFiles.Add(manifest.name.ToLower());
                usedFiles.Add(VEngine.Manifest.GetVersionFile(manifest.name.ToLower()));
                foreach (var bundle in build.bundles)
                {
                    usedFiles.Add(bundle.nameWithAppendHash);
                    usedFiles.Add(bundle.name + ".manifest");
                }
            }

            var files = Directory.GetFiles(EditorUtility.PlatformBuildPath);
            foreach (var file in files)
            {
                var name = Path.GetFileName(file);
                if (usedFiles.Contains(name))
                {
                    continue;
                }
                File.Delete(file);
                Logger.I("Delete {0}", file);
            }
        }
    }
}