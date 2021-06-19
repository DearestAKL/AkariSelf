namespace VEngine
{
    /// <summary>
    ///     下载内容的信息
    /// </summary>
    public class DownloadInfo
    {
        /// <summary>
        ///     下载内容的 crc，默认为 0，下载后不校验 crc，crc != 0 时，下载完成后会开启校验
        /// </summary>
        public uint crc;

        /// <summary>
        ///     下载内容的 保存路径
        /// </summary>
        public string savePath;

        /// <summary>
        ///     下载内容的 大小，默认为 0，下载后不校验长度， size > 0 时，下载完成后会开启校验
        /// </summary>
        public ulong size;

        /// <summary>
        ///     下载内容的 地址
        /// </summary>
        public string url;
    }
}