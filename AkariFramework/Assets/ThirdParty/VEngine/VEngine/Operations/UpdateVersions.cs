using System.Collections.Generic;
using System.IO;

namespace VEngine
{
    /// <summary>
    ///     版本更新操作，更新操作默认只处理服务器的就行
    /// </summary>
    public sealed class UpdateVersions : Operation
    {
        public string[] manifests;
        private readonly List<ManifestFile> assets = new List<ManifestFile>();

        public override void Start()
        {
            base.Start();
            if (Versions.SkipUpdate)
            {
                Finish();
                return;
            }

            foreach (var manifest in manifests)
            {
                assets.Add(ManifestFile.LoadAsync(manifest));
            }
        }

        public void Override()
        {
            foreach (var asset in assets)
            {
                asset.Override();
            }
        }

        public void Dispose()
        {
            foreach (var asset in assets)
            {
                if (asset.status != LoadableStatus.Unloaded)
                {
                    asset.Release();
                }
            }
            assets.Clear();
        }

        protected override void Update()
        {
            switch (status)
            {
                case OperationStatus.Processing:
                    foreach (var asset in assets)
                    {
                        if (!asset.isDone)
                        {
                            return;
                        }
                    }

                    var errors = new List<string>();
                    foreach (var asset in assets)
                    {
                        // fixed issues:Version.UpdateAsync状态错误 #2
                        if (asset.status == LoadableStatus.Unloaded)
                        {
                            errors.Add($"Failed to load {Path.GetFileName(asset.pathOrURL)} with {asset.error}");
                        }
                    }
                    Finish(errors.Count == 0 ? null : string.Join("\n", errors.ToArray()));
                    break;
            }
        }
    }
}