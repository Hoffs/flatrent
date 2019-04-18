using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Models;
using FlatRent.Repositories.Abstractions;
using FlatRent.Repositories.Interfaces;
using Serilog;
using Image = FlatRent.Entities.Image;

namespace FlatRent.Repositories
{
    public class ImageRepository : AuthoredBaseRepository<Image>, IImageRepository
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;

        public ImageRepository(DataContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public new Task<IEnumerable<FormError>> AddAsync(Image image, Guid authorId)
        {
            return base.AddAsync(image);
        }

        public new Task<IEnumerable<FormError>> UpdateAsync(Image image)
        {
            return base.UpdateAsync(image);
        }

        public async Task<IEnumerable<FormError>> DeleteAsync(Guid id)
        {
            var image = await GetAsync(id);
            return await DeleteAsync(image);
        }
    }
}