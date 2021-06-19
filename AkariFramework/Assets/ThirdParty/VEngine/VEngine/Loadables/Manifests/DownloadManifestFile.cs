using System;
using System.IO;
using UnityEngine;

namespace VEngine
{
    public class DownloadManifestFile : ManifestFile
    {
        private Download download;

        public string versionName { get; set; }

        protected override void OnLoad()
        {
            base.OnLoad();
            pathOrURL = Versions.GetDownloadURL(name);
            versionName = Manifest.GetVersionFile(name);
            var path = Versions.GetDownloadDataPath(versionName);
            var url = Versions.GetDownloadURL(versionName);
            download = Download.DownloadAsync(url, path);
            status = LoadableStatus.CheckVersion;
        }

        protected override void OnUpdate()
        {
            switch (status)
            {
                case LoadableStatus.CheckVersion:
                    UpdateVersion();
                    break;

                case LoadableStatus.Downloading:
                    UpdateDownloading();
                    break;

                case LoadableStatus.Loading:
                    var path = Versions.GetDownloadDataPath(name);
                    target.Load(path);
                    Finish();
                    break;
            }
        }

        public override void Override()
        {
            var split = name.Split(new[]
            {
                '_'
            }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length > 1)
            {
                var newName = split[0];
                var path = Versions.GetDownloadDataPath(name).Replace(name, newName);
                var path2 = Versions.GetDownloadDataPath(versionName).Replace(name, newName);
                File.Copy(Versions.GetDownloadDataPath(name), path, true);
                File.Copy(Versions.GetDownloadDataPath(versionName), path2, true);
                target.name = newName;
            }
            if (Versions.IsChanged(target.name))
            {
                target.Load(Versions.GetDownloadDataPath(target.name));
                Versions.Override(target);
            }
        }

        private void UpdateDownloading()
        {
            if (download == null)
            {
                Finish("request == nul with " + status);
                return;
            }

            progress = 0.2f + download.progress;
            if (!download.isDone)
            {
                return;
            }

            if (!string.IsNullOrEmpty(download.error))
            {
                Finish(download.error);
                return;
            }

            download = null;
            status = LoadableStatus.Loading;
        }

        private void UpdateVersion()
        {
            if (download == null)
            {
                Finish("request == null with " + status);
                return;
            }

            progress = 0.2f * download.progress;
            if (!download.isDone)
            {
                return;
            }

            if (!string.IsNullOrEmpty(download.error))
            {
                Finish(download.error);
                return;
            }

            var savePath = Versions.GetDownloadDataPath(versionName);
            if (!File.Exists(savePath))
            {
                Finish("version not exist.");
                return;
            }

            versionFile = ManifestVersionFile.Load(savePath);
            if (Versions.Manifests.Exists(m => m.version == versionFile.version && name.Contains(m.name)))
            {
                Logger.I("Skip to download {0}, because nothing to update.", name);
                status = LoadableStatus.Loading;
                return;
            }

            Logger.I("Read {0} with version {1} crc {2}", name, versionFile.version, versionFile.crc);
            var path = Versions.GetDownloadDataPath(name);
            if (File.Exists(path))
            {
                using (var stream = File.OpenRead(path))
                {
                    if (Utility.ComputeCRC32(stream) == versionFile.crc)
                    {
                        Logger.I("Skip to download {0}, because nothing to update.", name);
                        status = LoadableStatus.Loading;
                        return;
                    }
                }
            }

            download = Download.DownloadAsync(pathOrURL, path);
            status = LoadableStatus.Downloading;
        }
    }
}