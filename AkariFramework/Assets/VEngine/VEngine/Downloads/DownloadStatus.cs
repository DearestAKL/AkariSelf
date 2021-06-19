namespace VEngine
{
    /// <summary>
    ///     下载状态
    /// </summary>
    public enum DownloadStatus
    {
        /// <summary>
        ///     等待启动
        /// </summary>
        Wait,

        /// <summary>
        ///     下载中，排队或正在下载
        /// </summary>
        Progressing,

        /// <summary>
        ///     下载成功
        /// </summary>
        Success,

        /// <summary>
        ///     下载失败
        /// </summary>
        Failed
    }
}