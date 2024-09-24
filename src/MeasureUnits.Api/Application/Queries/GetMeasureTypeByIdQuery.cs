using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sofisoft.Erp.MeasureUnits.Api.Infrastructure.Exceptions;
using Sofisoft.Erp.MeasureUnits.Api.Models;
using Sofisoft.MongoDb.Repositories;

namespace Sofisoft.Erp.MeasureUnits.Api.Application.Queries
{
    public record GetMeasureTypeByIdQuery(string Id) : IRequest<MeasureType>
    {
        public class GetMeasureTypeByIdQueryHandler : IRequestHandler<GetMeasureTypeByIdQuery, MeasureType>
        {
            private readonly IRepository<MeasureType> _measureTypeRepository;

            public GetMeasureTypeByIdQueryHandler(IRepository<MeasureType> measureTypeRepository)
            {
                _measureTypeRepository = measureTypeRepository ?? throw new ArgumentNullException(nameof(measureTypeRepository));
            }

            public async Task<MeasureType> Handle(GetMeasureTypeByIdQuery request, CancellationToken cancellationToken)
            {
                return await _measureTypeRepository.FindByIdAsync(request.Id, cancellationToken: cancellationToken) ??
                    throw new MeasureTypeNotFoundException(request.Id);
            }
        }
    }
}