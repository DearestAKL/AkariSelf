namespace VEngine
{
    /// <summary>
    ///     Bundle 的运行时信息
    /// </summary>
    public class BundleInfo : ISerializable
    {
        /// <summary>
        ///     包含的所有 assets（id）
        /// </summary>
        public int[] assets = new int[0];

        /// <summary>
        ///     crc 用作版本校验
        /// </summary>
        public uint crc;

        /// <summary>
        ///     依赖列表
        /// </summary>
        public int[] deps = new int[0];

        /// <summary>
        ///     bundle 的 id
        /// </summary>
        public int id;

        /// <summary>
        ///     bundle 名字
        /// </summary>
        public string name;

        /// <summary>
        ///     字节大小
        /// </summary>
        public ulong size;

        public void Deserialize(string line)
        {
            var fields = line.Split(',');
            id = fields[0].IntValue();
            name = fields[1];
            crc = fields[2].UIntValue();
            size = fields[3].ULongValue();
            assets = fields[4].IntArrayValue("|");
            deps = fields[5].IntArrayValue("|");
        }

        public string Serialize()
        {
            return $"{id},{name},{crc},{size},{StringExtensions.Join("|", assets)},{StringExtensions.Join("|", deps)}";
        }
    }
}