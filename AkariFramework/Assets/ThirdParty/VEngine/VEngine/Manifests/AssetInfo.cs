namespace VEngine
{
    /// <summary>
    ///     Asset 的运行时信息
    /// </summary>
    public class AssetInfo : ISerializable
    {
        /// <summary>
        ///     资源名字 对应的 bundle（id）
        /// </summary>
        public int bundle;

        /// <summary>
        ///     资源所有依赖的 bundle 集合（id）
        /// </summary>
        public int[] bundles = new int[0];

        public int id;

        public void Deserialize(string line)
        {
            var fields = line.Split(',');
            id = fields[0].IntValue();
            bundle = fields[1].IntValue();
            bundles = fields[2].IntArrayValue("|");
        }

        public string Serialize()
        {
            return $"{id},{bundle},{StringExtensions.Join("|", bundles)}";
        }
    }
}