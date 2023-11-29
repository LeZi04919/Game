
using System.Collections.Generic;

namespace RoguelikeGame.Interfaces
{
    public interface ISerializable<T>
    {
        abstract static string Serialize(T? obj);
        abstract static T? Deserialize(string serializeStr);
        abstract static string SerializeArray(IEnumerable<T?> objs);
        abstract static T?[] DeserializeArray(string serializeStr);
    }
    public interface ISerializable
    {
        string Serialize();
        void Deserialize(string serializeStr);
    }
}