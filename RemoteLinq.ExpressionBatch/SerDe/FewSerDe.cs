using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RemoteLinq.ExpressionBatch.SerDe
{
    public static class FewSerDe
    {
        public static List<T> Deserialize<T>(
            byte[] reply
            )
        {
            var result = new List<T>();

            var count = reply[0];

            var positions = new List<long>();
            for (var index = 0; index < count; index++)
            {
                positions.Add(
                    BitConverter.ToInt64(
                        reply,
                        1 + index * sizeof(long)
                        )
                    );
            }

            var prefixLength = 1 + count * sizeof(long);

            for (var index = 0; index < count - 1; index++)
            {
                var currentPosition = positions[index];
                var nextPosition = positions[index + 1];

                var size = nextPosition - currentPosition;
                using (var stream = new MemoryStream(reply, (int)(prefixLength + currentPosition), (int)size))
                {
                    result.Add(
                        ProtobufExpressionSerializer.Instance.Read<T>(stream)
                        );
                }
            }

            var lastPosition = positions.Last();
            using (var stream = new MemoryStream(reply, (int)(prefixLength + lastPosition), (int)(reply.Length - prefixLength - lastPosition)))
            {
                result.Add(
                    ProtobufExpressionSerializer.Instance.Read<T>(stream)
                    );
            }

            return result;
        }

        public static byte[] Serialize<T>(
            T[] objects
            )
        {
            if (objects == null)
            {
                throw new ArgumentNullException(nameof(objects));
            }

            if (objects.Length == 0)
            {
                throw new ArgumentException(nameof(objects));
            }

            var positions = new List<long>();
            byte[] predata;
            using (var stream = new MemoryStream())
            {
                foreach (var obj in objects)
                {
                    if (obj is null)
                    {
                        throw new InvalidOperationException("Cennot serialize null-values");
                    }

                    positions.Add(stream.Position);
                    ProtobufExpressionSerializer.Instance.Write(stream, obj);
                }

                predata = stream.ToArray();
            }

            var result = new byte[1 + objects.Length * sizeof(long) + predata.Length];
            result[0] = (byte)objects.Length;

            var cposition = 1;
            foreach (long position in positions)
            {
                BitConverter.GetBytes(position).CopyTo(result, cposition);
                cposition += sizeof(long);
            }

            predata.CopyTo(result, cposition);
            return result;
        }
    }
}
