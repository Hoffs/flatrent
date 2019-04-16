using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Entities;
using FlatRent.Models;
using FlatRent.Repositories.Interfaces;
using Serilog;

namespace FlatRent.Repositories
{
    public class AttachmentRepository : BaseRepository<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(DataContext context, ILogger logger) : base(context, logger)
        {
        }

        public Task<IEnumerable<FormError>> UpdateAsync(Attachment attachment)
        {
            throw new System.NotImplementedException();
        }
    }
}