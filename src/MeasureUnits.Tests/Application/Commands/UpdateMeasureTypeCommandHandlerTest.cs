using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using Moq;
using Sofisoft.Abstractions.Managers;
using Sofisoft.Erp.MeasureUnits.Api.Application.Commands;
using Sofisoft.Erp.MeasureUnits.Api.Infrastructure.Exceptions;
using Sofisoft.Erp.MeasureUnits.Api.Models;
using Sofisoft.MongoDb.Repositories;
using Xunit;
using static Sofisoft.Erp.MeasureUnits.Api.Application.Commands.UpdateMeasureTypeCommand;

namespace Sofisoft.Erp.MeasureUnits.Tests.Application.Commands
{
    public class UpdateMeasureTypeCommandHandlerTest
    {
        private readonly Mock<IIdentityManager> _identityMock;
        private readonly Mock<IRepository<MeasureType>>  _measureTypeRepositoryMock;

        [Fact]
        public void Builder_receivenull_identity_manager_throws()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new UpdateMeasureTypeCommandHandler((IIdentityManager) null, _measureTypeRepositoryMock.Object));
        }

        [Fact]
        public void Builder_receivenull_repository_throws()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new UpdateMeasureTypeCommandHandler(_identityMock.Object, (IRepository<MeasureType>) null));
        }

        public UpdateMeasureTypeCommandHandlerTest()
        {
            _identityMock = new Mock<IIdentityManager>();
            _measureTypeRepositoryMock = new Mock<IRepository<MeasureType>>();
        }

        [Fact]
        public async Task Handle_update_item_and_return_unit()
        {
            //Arrange
            var command = new UpdateMeasureTypeCommand("001", "Peso Modificado");
            var document = new MeasureType {
                Name = "Peso"
            };
            var username = "test";

            _identityMock.SetupGet(x => x.Username).Returns(username);

            _measureTypeRepositoryMock.Setup(x => x.FindByIdAsync(command.Id, It.IsAny<FindOptions>(), CancellationToken.None))
                .ReturnsAsync(document);

            //Act
            var commandHandler = new UpdateMeasureTypeCommandHandler(_identityMock.Object, _measureTypeRepositoryMock.Object);
            var result = await commandHandler.Handle(command, CancellationToken.None);

            //Assert
            _measureTypeRepositoryMock.Verify(x => x.UpdateOneAsync(document, It.IsAny<UpdateOptions>(), CancellationToken.None)
                , Times.Once);
            Assert.Equal(command.Name, document.Name);
            Assert.Equal(username, document.ModifiedBy);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_throw_measuretype_not_found_exception()
        {
            //Arrange
            var command = new UpdateMeasureTypeCommand("001", "Peso Modificado");

            _measureTypeRepositoryMock.Setup(x => x.FindByIdAsync(command.Id, It.IsAny<FindOptions>(), CancellationToken.None))
                .ReturnsAsync((MeasureType) null);

            //Act
            var commandHandler = new UpdateMeasureTypeCommandHandler(_identityMock.Object, _measureTypeRepositoryMock.Object);

            //Assert
            await Assert.ThrowsAsync<MeasureTypeNotFoundException>(() => commandHandler.Handle(command, CancellationToken.None));
        }
        
        [Fact]
        public async Task Handle_if_exist_name_throw_domain_exception()
        {
            //Arrange
            var command = new UpdateMeasureTypeCommand("001", "Peso");
            var document = new MeasureType {
                Name = "Peso"
            };

            _measureTypeRepositoryMock.Setup(x => x.FindOneAsync(
                f => f.Id != command.Id && f.Name == command.Name.Trim(), It.IsAny<FindOptions>(), CancellationToken.None))
                .ReturnsAsync(document);

            //Act
            var commandHandler = new UpdateMeasureTypeCommandHandler(_identityMock.Object, _measureTypeRepositoryMock.Object);

            //Assert
            await Assert.ThrowsAsync<DomainException>(() => commandHandler.Handle(command, CancellationToken.None));
        }

    }
}