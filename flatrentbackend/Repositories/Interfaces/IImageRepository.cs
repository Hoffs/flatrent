using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Entities;
using FlatRent.Models;

namespace FlatRent.Repositories.Interfaces
{
    public interface IImageRepository : IAuthoredBaseRepository<Image>
    {
        Task<IEnumerable<FormError>> AddAsync(Image image, Guid authorId);
        Task<IEnumerable<FormError>> UpdateAsync(Image image);
        Task<IEnumerable<FormError>> DeleteAsync(Guid id);
    }
}