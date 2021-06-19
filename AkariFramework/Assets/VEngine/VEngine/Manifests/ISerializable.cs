namespace VEngine
{
    public interface ISerializable
    {
        void Deserialize(string line);
        string Serialize();
    }
}