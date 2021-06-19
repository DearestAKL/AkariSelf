namespace VEngine
{
    /// <summary>
    ///     Group 的运行时信息
    /// </summary>
    public class GroupInfo : ISerializable
    {
        public int[] bundles = new int[0];
        public string name;

        public void Deserialize(string line)
        {
            var fields = line.Split(',');
            name = fields[0];
            bundles = fields[1].IntArrayValue("|");
        }

        public string Serialize()
        {
            return $"{name},{StringExtensions.Join("|", bundles)}";
        }
    }
}