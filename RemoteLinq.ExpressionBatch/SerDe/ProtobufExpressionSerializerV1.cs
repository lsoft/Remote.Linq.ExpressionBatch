using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Aqua.TypeSystem;

namespace RemoteLinq.ExpressionBatch.SerDe
{
    internal class ProtobufExpressionSerializerV1 : IProtobufExpressionSerializer
    {
        public const int Version = 1;

        private readonly ProtoBuf.Meta.RuntimeTypeModel _configuration = Remote.Linq.ProtoBufTypeModel.ConfigureRemoteLinq();

        public static readonly ProtobufExpressionSerializerV1 Instance = new ProtobufExpressionSerializerV1();

        public void Write(Stream stream, object obj)
        {
            WriteType(stream, obj);

            WriteObject(stream, obj);
        }

        public T Read<T>(Stream stream)
        {
            var type = ReadType(stream);

            T obj = ReadObject<T>(stream, type);
            return obj;
        }


        #region private

        private void WriteType(Stream stream, object obj)
        {
            var typeInfo = new TypeInfo(
                obj.GetType(),
                false,
                false
                );

            WriteObject(stream, typeInfo);
        }

        private void WriteObject(Stream stream, object obj)
        {
            byte[] data;
            using (var dataStream = new MemoryStream())
            {
                _configuration.Serialize(dataStream, obj);
                dataStream.Position = 0;
                data = dataStream.ToArray();
            }

            long size = data.LongLength;
            byte[] sizeData = BitConverter.GetBytes(size);

            var datatype = obj is Exception ? (byte)1 : (byte)0;

            stream.WriteByte(datatype);
            stream.Write(sizeData, 0, sizeData.Length);
            stream.Write(data, 0, data.Length);
        }

        private Type ReadType(
            Stream stream
            )
        {
            var typeInfo = ReadObject<TypeInfo>(stream);
            var type = typeInfo.ToType();

            return type;
        }

        private T ReadObject<T>(Stream stream, Type? type = null)
        {
            int dataType = stream.ReadByte();
            if (dataType < 0)
            {
                throw new OperationCanceledException("network stream was closed by other party.");
            }

            bool isException = dataType == 1;
            byte[] bytes = new byte[65536];

            stream.Read(bytes, 0, sizeof(long));
            long size = BitConverter.ToInt64(bytes, 0);

            object obj;
            if (size > 0)
            {
                using (var dataStream = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        int length = size - count < bytes.Length
                            ? (int)(size - count)
                            : bytes.Length;

                        int i = stream.Read(bytes, 0, length);

                        if (i == 0)
                        {
                            throw new OperationCanceledException("network stream was closed by other party.");
                        }
                        
                        count += i;

                        dataStream.Write(bytes, 0, i);
                    }
                    while (count < size);

                    dataStream.Position = 0;
                    obj = _configuration.Deserialize(dataStream, null, type ?? typeof(T));
                }
            }
            else
            {
                obj = Activator.CreateInstance(type!)!;
            }

            if (isException)
            {
                var exception = (Exception)obj;
                throw exception;
            }

            return (T)obj;
        }

        #endregion
    }
}
