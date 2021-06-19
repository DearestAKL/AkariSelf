using UnityEngine;

namespace VEngine
{
    /// <summary>
    ///     从服务器下载的 Bundle，下载完成后会自动加载，并更新下载地址到 bundleToURLs 的缓存。
    /// </summary>
    internal class DownloadBundle : Bundle
    {
        private Download download;
        private AssetBundleCreateRequest request;

        public override void LoadImmediate()
        {
            if (isDone)
            {
                return;
            }
            while (!download.isDone)
            {
                Download.UpdateDownloads();
            }
            assetBundle = request == null ? AssetBundle.LoadFromFile(download.info.savePath) : request.assetBundle;
            if (assetBundle == null)
            {
                Finish("assetBundle == null");
                return;
            }
            request = null;
        }

        protected override void OnLoad()
        {
            download = Download.DownloadAsync(pathOrURL, Versions.GetDownloadDataPath(info.name), null, info.size, info.crc);
            download.completed += OnDownloaded;
        }

        private void OnDownloaded(Download obj)
        {
            if (assetBundle != null)
            {
                return;
            }
            request = AssetBundle.LoadFromFileAsync(obj.info.savePath);
            Versions.SetBundlePathOrURl(info.name, obj.info.savePath);
        }

        protected override void OnUpdate()
        {
            if (status != LoadableStatus.Loading)
            {
                return;
            }

            if (download != null && !download.isDone)
            {
                progress = download.downloadedBytes * 1f / download.info.size * 0.5f;
                if (!string.IsNullOrEmpty(download.error))
                {
                    Finish(download.error);
                    return;
                }
            }

            if (request == null)
            {
                return;
            }

            progress = 0.5f + request.progress;
            if (!request.isDone)
            {
                return;
            }

            assetBundle = request.assetBundle;
            Finish(assetBundle == null ? "assetBundle == null" : null);
            request = null;
        }
    }
}