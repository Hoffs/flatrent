using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Entities;
using FlatRent.Models;

namespace FlatRent.Repositories.Interfaces
{
    public interface IAttachmentRepository : IAuthoredBaseRepository<Attachment>
    {
        Task<IEnumerable<FormError>> UpdateAsync(Attachment attachment);
    }
}