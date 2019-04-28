using System;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Controllers.Abstractions;
using FlatRent.Controllers.Filters;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FlatRent.Controllers
{
    [Controller]
    [Route("api/agreement/{id}/[controller]")]
    public class FaultController : AuthoredBaseEntityController<Agreement>
    {
        private readonly IAgreementRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public FaultController(IAgreementRepository repository, IMapper mapper, ILogger logger) : base(repository)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> CreateFaultAsync([FromRoute] Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{faultId}")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> GetFaultAsync([FromRoute] Guid id, [FromRoute] Guid faultId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{faultId}/fixed")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> UpdateFaultAsync([FromRoute] Guid id, [FromRoute] Guid faultId, [FromBody] float price)
        {
            throw new NotImplementedException();
        }
    }
}