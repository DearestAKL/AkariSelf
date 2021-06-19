using System;
using System.Collections.Generic;
using UnityEngine;

namespace VEngine
{
    /// <summary>
    ///     Bundle 类，将 Unity 的 AssetBundle 的进行了封装，加载的时候，底层会自动根据 Bundle 的版本状态进行寻址。
    /// </summary>
    public class Bundle : Loadable
    {
        /// <summary>
        ///     按 key 缓存的所有加载类对象
        /// </summary>
        protected internal static readonly Dictionary<string, Bundle> Cache = new Dictionary<string, Bundle>();

        protected internal static readonly List<Bundle> Unused = new List<Bundle>();

        protected BundleInfo info;
        protected internal AssetBundle assetBundle { get; set; }

        protected override void OnUnused()
        {
            Unused.Add(this);
        }

        internal static Bundle LoadInternal(BundleInfo info, bool mustCompleteOnNextFrame)
        {
            if (info == null)
            {
                throw new NullReferenceException();
            }

            if (!Cache.TryGetValue(info.name, out var item))
            {
                var url = Versions.GetBundlePathOrURL(info);
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    item = new WebBundle
                    {
                        pathOrURL = url,
                        info = info
                    };
                }
                else
                {
                    if (!string.IsNullOrEmpty(Versions.DownloadURL) && url.StartsWith(Versions.DownloadURL))
                    {
                        item = new DownloadBundle
                        {
                            pathOrURL = url,
                            info = info
                        };
                    }
                    else
                    {
                        item = new LocalBundle
                        {
                            pathOrURL = url,
                            info = info
                        };
                    }
                }

                Cache.Add(info.name, item);
            }

            item.mustCompleteOnNextFrame = mustCompleteOnNextFrame;
            item.Load();
            if (mustCompleteOnNextFrame)
            {
                item.LoadImmediate();
            }

            return item;
        }

        internal static void UpdateBundles()
        {
            for (var index = 0; index < Unused.Count; index++)
            {
                var item = Unused[index];
                if (Updater.busy)
                {
                    return;
                }

                if (!item.isDone)
                {
                    continue;
                }

                Unused.RemoveAt(index);
                index--;
                if (!item.reference.unused)
                {
                    continue;
                }

                item.Unload();
                Cache.Remove(item.info.name);
            }
        }

        protected override void OnUnload()
        {
            if (assetBundle == null)
            {
                return;
            }

            assetBundle.Unload(true);
            assetBundle = null;
        }
    }
}