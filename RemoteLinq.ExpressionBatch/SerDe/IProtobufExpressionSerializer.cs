using System.IO;

namespace RemoteLinq.ExpressionBatch.SerDe
{
    public interface IProtobufExpressionSerializer
    {
        void Write(Stream stream, object obj);

        T Read<T>(Stream stream);
    }
}