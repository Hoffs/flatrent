using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
using FlatRent.Dtos;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Interfaces;
using FlatRent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Serilog;

namespace FlatRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlatController : Controller
    {
        private readonly IFlatRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public FlatController(IFlatRepository repository, IMapper mapper, ILogger logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize(Policy = "Supply")]
        [HttpPost]
        public async Task<IActionResult> CreateFlat(FlatForm form)
        {
            try
            {
                var errors = await _repository.AddFlatAsync(form).ConfigureAwait(false);
                if (errors != null) return new BadRequestObjectResult(errors.GetFormattedResponse());
                return StatusCode(201);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Exception thrown while creating flat {FlatForm}", form);
                return StatusCode(500);
            }
        }

        [Authorize(Policy = "Supply")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlat([FromRoute] Guid id)
        {
            try
            {
                var errors = await _repository.DeleteFlatAsync(id).ConfigureAwait(false);
                if (errors != null) return new BadRequestObjectResult(errors.GetFormattedResponse());
                return StatusCode(201);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Exception thrown while deleting flat {Id}", id);
                return StatusCode(500);
            }
        }

        [Authorize(Policy = "Supply")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlat([FromRoute] Guid id, FlatForm form)
        {
            throw new NotImplementedException();
        }

        [Authorize(Policy = "Client")]
        [ExactQueryParam("count", "offset")]
        [HttpGet]
        public async Task<IActionResult> GetFlats([Range(1, 100, ErrorMessage = Errors.Range)] int count = 20, [Range(0, int.MaxValue, ErrorMessage = Errors.Range)] int offset = 0)
        {
            var flats = await _repository.GetFlatsAsync(false, count, offset).ConfigureAwait(false);
            var mappedFlats = _mapper.Map<List<FlatListItem>>(flats);
            Response.Headers.Add("X-Total-Count", (await _repository.GetFlatCountAsync().ConfigureAwait(false)).ToString());
            return new OkObjectResult(mappedFlats);
        }

        [Authorize(Policy = "Employee")]
        [ExactQueryParam("rented", "count", "offset")]
        [HttpGet]
        public async Task<IActionResult> GetFlats(bool rented = false, [Range(1, 100)] int count = 20, [Range(0, int.MaxValue)] int offset = 0)
        {
            var flats = await _repository.GetFlatsAsync(rented, count, offset).ConfigureAwait(false);
            var mappedFlats = _mapper.Map<List<FlatListItem>>(flats);
            Response.Headers.Add("X-Total-Count", (await _repository.GetFlatCountAsync(rented).ConfigureAwait(false)).ToString());
            return new OkObjectResult(mappedFlats);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlat([FromRoute] Guid id)
        {
            var flat = await _repository.GetFlatAsync(id).ConfigureAwait(false);
            if (flat == null) return NotFound(id);
            return new OkObjectResult(flat);
        }
    }
}