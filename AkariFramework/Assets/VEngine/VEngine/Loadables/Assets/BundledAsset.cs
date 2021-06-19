using System;
using UnityEngine;

namespace VEngine
{
    /// <summary>
    ///     在 Bundle 中的资源，持有 Dependencies 可以自动管理依赖
    /// </summary>
    public class BundledAsset : Asset
    {
        private Dependencies dependencies;
        private AssetBundleRequest request;

        internal static BundledAsset Create(string path, Type type)
        {
            return new BundledAsset
            {
                pathOrURL = path,
                type = type
            };
        }

        protected override void OnLoad()
        {
            dependencies = new Dependencies
            {
                pathOrURL = pathOrURL
            };
            dependencies.Load();
            status = LoadableStatus.DependentLoading;
        }

        protected override void OnUnload()
        {
            if (dependencies != null)
            {
                dependencies.Unload();
                dependencies = null;
            }

            request = null;
            asset = null;
        }

        public override void LoadImmediate()
        {
            if (isDone)
            {
                return;
            }

            if (dependencies == null)
            {
                Finish("dependencies == null");
                return;
            }

            if (!dependencies.isDone)
            {
                dependencies.LoadImmediate();
            }

            if (dependencies.assetBundle == null)
            {
                Finish("dependencies.assetBundle == null");
                return;
            }

            asset = dependencies.assetBundle.LoadAsset(pathOrURL, type);
            if (asset == null)
            {
                Finish("target == null");
                return;
            }

            Finish();
        }

        protected override void OnUpdate()
        {
            switch (status)
            {
                case LoadableStatus.Loading:
                    UpdateLoading();
                    break;

                case LoadableStatus.DependentLoading:
                    UpdateDependencies();
                    break;
            }
        }

        private void UpdateLoading()
        {
            if (request == null)
            {
                Finish("request == null");
                return;
            }

            progress = 0.5f + request.progress * 0.5f;
            if (!request.isDone)
            {
                return;
            }

            asset = request.asset;
            if (asset == null)
            {
                Finish("asset == null");
                return;
            }

            Finish();
        }

        private void UpdateDependencies()
        {
            if (dependencies == null)
            {
                Finish("dependencies == null");
                return;
            }

            progress = 0.5f * dependencies.progress;
            if (!dependencies.isDone)
            {
                return;
            }

            var assetBundle = dependencies.assetBundle;
            if (assetBundle == null)
            {
                Finish("assetBundle == null");
                return;
            }

            request = assetBundle.LoadAssetAsync(pathOrURL, type);
            status = LoadableStatus.Loading;
        }
    }
}