using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UnityEngine;

namespace VEngine
{
    /// <summary>
    ///     下载操作，可以通过 MaxDownloads 控制最大并发数量，以及 MaxBandwidth 控制最大带宽，目前底层采用的是 c# 的 HttpWebRequest 下载资源暂时没有测试
    /// </summary>
    public sealed class Download : CustomYieldInstruction
    {
        /// <summary>
        ///     最大并行下载的数量，建议不操过 10 个，多了可能会出现异常，而 Unity 官方博客有篇老文章之前说同时下载的 Bundle 数量需要控制在 3 - 5 个。
        /// </summary>
        public static uint MaxDownloads = 10;

        /// <summary>
        ///     最大带宽, 大于 0 时 表示开启限速，默认为 0 不限速。
        /// </summary>
        public static ulong MaxBandwidth = 0; // 1 MB

        /// <summary>
        ///     准备下载的队列，当 Progressing 中的数量小于 MaxDownloads 的时候会自动按照 FIFO 的策略，从这个队列启动新的下载。
        /// </summary>
        private static readonly List<Download> Prepared = new List<Download>();

        /// <summary>
        ///     下载中的队列，最大数量收到 MaxDownloads 控制。
        /// </summary>
        public static readonly List<Download> Progressing = new List<Download>();

        /// <summary>
        ///     缓冲区的内容，防止重复下载，当所有内容下载完成后，自动清理。
        /// </summary>
        private static readonly Dictionary<string, Download> Cache = new Dictionary<string, Download>();


        private static float lastSampleTime;

        private static ulong lastTotalDownloadedBytes;

        /// <summary>
        ///     读取缓冲区
        /// </summary>
        private readonly byte[] _readBuffer = new byte[4096];

        /// <summary>
        ///     当前带宽，每秒下载的字节数
        /// </summary>
        private ulong _bandWidth; 

        /// <summary>
        ///     下载线程
        /// </summary>
        private Thread _thread;

        private Download()
        {
            status = DownloadStatus.Wait;
            downloadedBytes = 0;
        }

        public float progress => downloadedBytes * 1f / info.size;


        /// <summary>
        ///     下载 <see cref="DownloadInfo" />
        /// </summary>
        public DownloadInfo info { get; private set; }

        /// <summary>
        ///     下载的状态
        /// </summary>
        public DownloadStatus status { get; private set; }

        /// <summary>
        ///     错误内容
        /// </summary>
        public string error { get; private set; }

        /// <summary>
        ///     下载完成的回调
        /// </summary>
        public Action<Download> completed { get; set; }

        /// <summary>
        ///     是否下载完成
        /// </summary>
        public bool isDone => status == DownloadStatus.Failed || status == DownloadStatus.Success;

        /// <summary>
        ///     已经下载的字节
        /// </summary>
        public ulong downloadedBytes { get; private set; }

        /// <summary>
        ///     是否让协程继续等待
        /// </summary>
        public override bool keepWaiting => !isDone;

        /// <summary>
        ///     是否有内容在下载中
        /// </summary>
        public static bool Working => Progressing.Count > 0;

        /// <summary>
        ///     所有下载内容的已经下载的字节大小
        /// </summary>
        public static ulong TotalDownloadedBytes
        {
            get
            {
                var size = 0ul;
                foreach (var item in Cache)
                {
                    size += item.Value.downloadedBytes;
                }

                return size;
            }
        }

        /// <summary>
        ///     所有下载内容的总字节大小
        /// </summary>
        public static ulong TotalSize
        {
            get
            {
                var value = 0UL;
                foreach (var item in Cache)
                {
                    value += item.Value.info.size;
                }

                return value;
            }
        }

        /// <summary>
        ///     当前下载的总带宽
        /// </summary>
        public static ulong TotalBandwidth { get; private set; }

        /// <summary>
        ///     异步下载一个文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="savePath"></param>
        /// <param name="completed"></param>
        /// <param name="size"></param>
        /// <param name="crc"></param>
        /// <returns></returns>
        public static Download DownloadAsync(string url, string savePath, Action<Download> completed = null,
            ulong size = 0, uint crc = 0)
        {
            return DownloadAsync(new DownloadInfo
            {
                url = url,
                savePath = savePath,
                crc = crc,
                size = size
            }, completed);
        }

        /// <summary>
        ///     下载指定信息的内容
        /// </summary>
        /// <param name="info"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        public static Download DownloadAsync(DownloadInfo info, Action<Download> completed = null)
        {
            if (!Cache.TryGetValue(info.url, out var download))
            {
                download = new Download
                {
                    info = info
                };
                Prepared.Add(download);
                Cache.Add(info.url, download);
            }
            else
            {
                Logger.W("Download url {0} already exist.", info.url);
            }

            if (completed != null)
            {
                download.completed += completed;
            }

            return download;
        }


        /// <summary>
        ///     更新所有下载操作
        /// </summary>
        public static void UpdateDownloads()
        {
            if (Prepared.Count > 0)
            {
                for (var index = 0; index < Mathf.Min(Prepared.Count, MaxDownloads - Progressing.Count); index++)
                {
                    var download = Prepared[index];
                    Prepared.RemoveAt(index);
                    index--;
                    Progressing.Add(download);
                    download.Start();
                }
            }

            if (Progressing.Count > 0)
            {
                for (var index = 0; index < Progressing.Count; index++)
                {
                    var download = Progressing[index];
                    if (download.isDone)
                    {
                        if (download.status == DownloadStatus.Failed)
                        {
                            Logger.E("Unable to download {0} with error {1}", download.info.url, download.error);
                        }
                        else
                        {
                            Logger.I("Success to download {0}", download.info.url);
                        }
                        download.Complete();
                        Progressing.RemoveAt(index);
                        index--;
                    }
                }

                if (Time.realtimeSinceStartup - lastSampleTime >= 1)
                {
                    TotalBandwidth = TotalDownloadedBytes - lastTotalDownloadedBytes;
                    lastTotalDownloadedBytes = TotalDownloadedBytes;
                    lastSampleTime = Time.realtimeSinceStartup;
                }
            }
            else
            {
                if (Cache.Count <= 0)
                {
                    return;
                }

                Cache.Clear();
                lastTotalDownloadedBytes = 0;
                lastSampleTime = Time.realtimeSinceStartup;
            }
        }

        /// <summary>
        ///     重试下载
        /// </summary>
        public void Retry()
        {
            status = DownloadStatus.Wait;
            Start();
        }

        private void Complete()
        {
            if (completed != null)
            {
                completed.Invoke(this);
                completed = null;
            }
        }

        /// <summary>
        ///     取消下载
        /// </summary>
        public void Cancel()
        {
            error = "User Cancel.";
            status = DownloadStatus.Failed;
        }

        private void Run()
        {
            try
            {
                Downloading();
            }
            catch (Exception e)
            {
                error = e.Message;
                status = DownloadStatus.Failed;
            }

            CheckStatus();
            if (_thread == null)
            {
                return;
            }

            _thread.Abort();
            _thread = null;
        }

        private void CheckStatus()
        {
            if (status != DownloadStatus.Progressing)
            {
                return;
            }

            // 下载完成，进行校验
            if (downloadedBytes != info.size)
            {
                error = $"长度 {downloadedBytes} 不符合期望 {info.size}";
                status = DownloadStatus.Failed;
                return;
            }

            if (info.crc != 0)
            {
                using (var stream = File.OpenRead(info.savePath))
                {
                    var crc = Utility.ComputeCRC32(stream);
                    if (info.crc != crc)
                    {
                        error = $"crc {crc} 不符合期望 {info.crc}";
                        status = DownloadStatus.Failed;
                        return;
                    }
                }
            }

            status = DownloadStatus.Success;
        }

        private void Downloading()
        {
            WebRequest request;
            if (info.url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                var httpWebRequest = (HttpWebRequest) WebRequest.Create(info.url);
                httpWebRequest.ProtocolVersion = HttpVersion.Version10;
                request = httpWebRequest;
            }
            else if (info.url.StartsWith("ftp", StringComparison.OrdinalIgnoreCase))
            {
                var ftpWebRequest = (FtpWebRequest) WebRequest.Create(info.url);
                //TODO:
                request = ftpWebRequest;
            }
            else
            {
                request = WebRequest.Create(info.url);
            }

            using (var response = request.GetResponse())
            {
                using (var reader = response.GetResponseStream())
                {
                    if (File.Exists(info.savePath))
                    {
                        File.Delete(info.savePath);
                    }

                    using (var writer = File.OpenWrite(info.savePath))
                    {
                        if (info.size == 0)
                        {
                            info.size = (ulong) response.ContentLength;
                        }

                        var startTime = DateTime.Now;
                        while (status == DownloadStatus.Progressing)
                        {
                            if (reader != null)
                            {
                                var len = reader.Read(_readBuffer, 0, _readBuffer.Length);
                                if (len > 0)
                                {
                                    writer.Write(_readBuffer, 0, len);
                                    downloadedBytes += (ulong) len;
                                    _bandWidth += (ulong) len;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                error = "reader == null";
                                status = DownloadStatus.Failed;
                                break;
                            }

                            // 限速处理
                            var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
                            while (MaxBandwidth > 0 &&
                                   status == DownloadStatus.Progressing &&
                                   _bandWidth >= MaxBandwidth / (ulong) Progressing.Count &&
                                   elapsed < 1000)
                            {
                                var wait = Mathf.Clamp((int) (1000 - elapsed), 1, 33);
                                Thread.Sleep(wait);
                                elapsed = (DateTime.Now - startTime).TotalMilliseconds;
                            }

                            if (!(elapsed >= 1000))
                            {
                                continue;
                            }

                            startTime = DateTime.Now;
                            _bandWidth = 0L;
                        }
                    }
                }
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }

        private void Start()
        {
            if (status != DownloadStatus.Wait)
            {
                return;
            }

            status = DownloadStatus.Progressing;
            _thread = new Thread(Run)
            {
                IsBackground = true
            };
            _thread.Start();
        }
    }
}