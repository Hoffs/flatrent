using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Models.Requests;
using FlatRent.Repositories.Abstractions;
using FlatRent.Repositories.Interfaces;
using Serilog;

namespace FlatRent.Repositories
{
    public class IncidentRepository : AuthoredBaseRepository<Incident>, IIncidentRepository
    {
        public IncidentRepository(DataContext context, IMapper mapper, ILogger logger) : base(context, mapper, logger)
        {
        }

        public async Task<(IEnumerable<FormError>, Incident)> CreateAsync(Guid agreementId, IncidentForm form, Guid userId)
        {
            var agreement = await Context.Agreements.FindAsync(agreementId);
            var incident = Mapper.Map<Incident>(form);
            incident.AuthorId = userId;
            incident.AgreementId = agreementId;
            incident.Attachments.SetProperty(a => a.Incident, incident);
            incident.Attachments.SetProperty(a => a.AuthorId, userId);
            incident.Conversation = new Conversation
            {
                AuthorId = userId,
                RecipientId = agreement.Flat.AuthorId,
                Subject = incident.Name,
            };

            return (await base.AddAsync(incident), incident);
        }

        public new Task<IEnumerable<FormError>> UpdateAsync(Incident entity)
        {
            return base.UpdateAsync(entity);
        }

        public new Task<IEnumerable<FormError>> DeleteAsync(Incident entity)
        {
            return base.DeleteAsync(entity);
        }
    }
}