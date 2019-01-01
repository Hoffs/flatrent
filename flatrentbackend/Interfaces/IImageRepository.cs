using System;
using System.IO;
using System.Threading.Tasks;

namespace FlatRent.Interfaces
{
    public interface IImageRepository
    {
        Task<byte[]> GetImageAsync(Guid id);
    }
}