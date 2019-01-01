using System;
using System.Threading.Tasks;
using FlatRent.Interfaces;
using Serilog;

namespace FlatRent.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;

        public ImageRepository(DataContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<byte[]> GetImageAsync(Guid id)
        {
            var photo = await _context.Photos.FindAsync(id).ConfigureAwait(false);
            return photo?.PhotoBytes;
        }
    }
}