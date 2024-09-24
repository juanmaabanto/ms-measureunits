using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sofisoft.Abstractions;
using Sofisoft.Abstractions.Managers;
using Sofisoft.Erp.MeasureUnits.Api.Infrastructure.Exceptions;
using Sofisoft.Erp.MeasureUnits.Api.Models;
using Sofisoft.MongoDb.Repositories;

namespace Sofisoft.Erp.MeasureUnits.Api.Application.Commands
{
    public record UpdateMeasureTypeCommand(string Id, string Name) : IRequest
    {
        public class UpdateMeasureTypeCommandHandler : IRequestHandler<UpdateMeasureTypeCommand>
        {
            private readonly IIdentityManager _identityManager;
            private readonly IRepository<MeasureType> _measureTypeRepository;

            public UpdateMeasureTypeCommandHandler(IIdentityManager identityManager, IRepository<MeasureType> measureTypeRepository)
            {
                _identityManager = identityManager ?? throw new ArgumentNullException(nameof(identityManager));
                _measureTypeRepository = measureTypeRepository ?? throw new ArgumentNullException(nameof(measureTypeRepository));
            }

            public async Task<Unit> Handle(UpdateMeasureTypeCommand command, CancellationToken cancellationToken)
            {
                var username = _identityManager.Username;

                var existent = await _measureTypeRepository.FindOneAsync(t => t.Id != command.Id
                    &&  t.Name == command.Name.Trim(), cancellationToken: cancellationToken);

                if(existent is not null)
                {
                    throw new DomainException(SofisoftConstants.ValidationError.Exist);
                }

                var measureType = await _measureTypeRepository.FindByIdAsync(command.Id, cancellationToken: cancellationToken);

                if(measureType is null)
                {
                    throw new MeasureTypeNotFoundException(command.Id);
                }

                measureType.Name = command.Name;
                measureType.ModifiedBy = username;

                await _measureTypeRepository.UpdateOneAsync(measureType, cancellationToken: cancellationToken);

                return Unit.Value;
            }
        }
    }
}