using System;
using System.IO;

namespace RemoteLinq.ExpressionBatch.SerDe
{
    public class ProtobufExpressionSerializer : IProtobufExpressionSerializer
    {
        public static readonly ProtobufExpressionSerializer Instance = new ProtobufExpressionSerializer();

        /// <inheritdoc />
        public void Write(
            Stream stream,
            object obj
            )
        {
            stream.WriteByte(ProtobufExpressionSerializerV1.Version);
            ProtobufExpressionSerializerV1.Instance.Write(stream, obj);
        }

        /// <inheritdoc />
        public T Read<T>(
            Stream stream
            )
        {
            var version = stream.ReadByte();

            if (version < 0)
            {
                throw new OperationCanceledException("network stream was closed by other party.");
            }

            if (version == ProtobufExpressionSerializerV1.Version)
            {
                return ProtobufExpressionSerializerV1.Instance.Read<T>(stream);
            }

            throw new InvalidOperationException($"Incorrect protocol version: {version}");
        }
    }
}