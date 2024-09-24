using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sofisoft.Erp.MeasureUnits.Api.Models;
using Sofisoft.MongoDb.Repositories;

namespace Sofisoft.Erp.MeasureUnits.Api.Application.Queries
{
    public record GetListMeasureTypesQuery(string Name, string Sort, int PageSize, int Start)
        : IRequest<(long total, IEnumerable<MeasureType> data)>
    {
        public class GetListMeasureTypesQueryHandler
            : IRequestHandler<GetListMeasureTypesQuery, (long total, IEnumerable<MeasureType> data)>
        {
            private readonly IRepository<MeasureType> _measureTypeRepository;

            public GetListMeasureTypesQueryHandler(IRepository<MeasureType> measureTypeRepository)
            {
                _measureTypeRepository = measureTypeRepository ?? throw new ArgumentNullException(nameof(measureTypeRepository));
            }

            public async Task<(long total, IEnumerable<MeasureType> data)> Handle(GetListMeasureTypesQuery request, CancellationToken cancellationToken)
            {
                var regex = new Regex($".*{request.Name}.*", RegexOptions.IgnoreCase);
                var taskTotal = _measureTypeRepository.CountAsync(f => regex.IsMatch(f.Name)
                    , cancellationToken: cancellationToken);
                var taskList = _measureTypeRepository.FilterPaginatedAsync(f => regex.IsMatch(f.Name),
                    request.Sort, request.PageSize, request.Start, cancellationToken: cancellationToken);

                return (await taskTotal, await taskList);
            }
        }
    }
}