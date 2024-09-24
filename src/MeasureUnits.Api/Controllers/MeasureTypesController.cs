using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sofisoft.Abstractions.Responses;
using Sofisoft.Erp.MeasureUnits.Api.Application.Commands;
using Sofisoft.Erp.MeasureUnits.Api.Application.Queries;
using Sofisoft.Erp.MeasureUnits.Api.Models;

namespace Sofisoft.Erp.MeasureUnits.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MeasureTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MeasureTypesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        #region Gets

        /// <summary>
        /// Returns paginated list of measure types.
        /// </summary>
        /// <param name="name">Filter by name.</param>
        /// <param name="sort">Ordering criterion.</param>
        /// <param name="pageSize">Number of records returned.</param>
        /// <param name="start">Index of the initial record.</param>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<PaginatedResponse<MeasureType>>> GetList(string name, string sort, 
            [FromQuery]int pageSize = 50, [FromQuery]int start = 0, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetListMeasureTypesQuery(name, sort, pageSize, start), cancellationToken);

            return new PaginatedResponse<MeasureType>(start, pageSize, result.total, result.data);
        }

        /// <summary>
        /// Returns the type of measure by ID.
        /// </summary>
        /// <param name="measureTypeId">Id of the measure type.</param>
        [HttpGet]
        [Route("{measureTypeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<MeasureType>> Get(string measureTypeId, CancellationToken cancellationToken)
            => await _mediator.Send(new GetMeasureTypeByIdQuery(measureTypeId), cancellationToken);

        #endregion

        #region Posts

        /// <summary>
        /// Create a new type of measure.
        /// </summary>
        /// <param name="command">Object to be created.</param>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<MeasureType>> Post([FromBody]CreateMeasureTypeCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(Get), new { measureTypeId = result.Id}, result);
        }

        #endregion

        #region Puts

        /// <summary>
        /// Modify a measure type.
        /// </summary>
        /// <param name="command">Object to be modified.</param>
        [Route("")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult> Put([FromBody] UpdateMeasureTypeCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        #endregion


    }
}