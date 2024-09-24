using System;
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
    public record CreateMeasureTypeCommand(string Name) : IRequest<MeasureType>
    {
        public class CreateMeasureTypeCommandHandler : IRequestHandler<CreateMeasureTypeCommand, MeasureType>
        {
            private readonly IIdentityManager _identityManager;
            private readonly IRepository<MeasureType> _measureTypeRepository;

            public CreateMeasureTypeCommandHandler(IIdentityManager identityManager, IRepository<MeasureType> measureTypeRepository)
            {
                _identityManager = identityManager ?? throw new ArgumentNullException(nameof(identityManager));
                _measureTypeRepository = measureTypeRepository ?? throw new ArgumentNullException(nameof(measureTypeRepository));
            }

            public async  Task<MeasureType> Handle(CreateMeasureTypeCommand command, CancellationToken cancellationToken)
            {
                var username = _identityManager.Username;
                
                var existent = await _measureTypeRepository.FindOneAsync(t => t.Name == command.Name.Trim()
                    , cancellationToken: cancellationToken);
                
                if(existent is not null)
                {
                    throw new DomainException(SofisoftConstants.ValidationError.Exist);
                }

                var measureType = new MeasureType {
                    Name = command.Name,
                    CreatedBy = username
                };

                await _measureTypeRepository.InsertOneAsync(measureType, cancellationToken: cancellationToken);
                
                return measureType;
            }
        }
    }
}