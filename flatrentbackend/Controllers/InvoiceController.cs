using System;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Controllers.Abstractions;
using FlatRent.Controllers.Filters;
using FlatRent.Entities;
using FlatRent.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FlatRent.Controllers
{
    [Controller]
    [Route("api/agreement/{id}/[controller]")]
    public class InvoiceController : AuthoredBaseEntityController<Agreement>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public InvoiceController(IAgreementRepository repository, IMapper mapper, ILogger logger) : base(repository)
        {
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{faultId}")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> GetFaultAsync([FromRoute] Guid id, [FromRoute] Guid faultId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{faultId}")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> UpdateFaultAsync([FromRoute] Guid id, [FromRoute] Guid faultId)
        {
            throw new NotImplementedException();
        }
    }
}