using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Entities;
using FlatRent.Models;
using FlatRent.Repositories.Abstractions;
using FlatRent.Repositories.Interfaces;
using Serilog;

namespace FlatRent.Repositories
{
    public class AttachmentRepository : AuthoredBaseRepository<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(DataContext context, IMapper mapper, ILogger logger) : base(context, mapper, logger)
        {
        }

        public Task<IEnumerable<FormError>> UpdateAsync(Attachment attachment)
        {
            return base.UpdateAsync(attachment);
        }
    }
}