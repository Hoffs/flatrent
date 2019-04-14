using FlatRent.Entities;
using FlatRent.Repositories.Interfaces;
using Serilog;

namespace FlatRent.Repositories
{
    public class AttachmentRepository : BaseRepository<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(DataContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}