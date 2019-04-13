using System;
using System.IO;
using System.Threading.Tasks;

namespace FlatRent.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] GetByteArray(this Stream stream)
        {
            if (!stream.CanRead) return new byte[0];
            var bytes = new byte[stream.Length];
            stream.Read(bytes);
            return bytes;
;        }

        public static async Task<byte[]> GetByteArrayAsync(this Stream stream)
        {
            if (!stream.CanRead) return new byte[0];
            var bytes = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            await stream.ReadAsync(bytes);
            return bytes;
;        }
    }
}