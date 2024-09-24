using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Moq;
using Sofisoft.Abstractions.Managers;
using Sofisoft.Erp.MeasureUnits.Api.Application.Commands;
using Sofisoft.Erp.MeasureUnits.Api.Infrastructure.Exceptions;
using Sofisoft.Erp.MeasureUnits.Api.Models;
using Sofisoft.MongoDb.Repositories;
using Xunit;
using static Sofisoft.Erp.MeasureUnits.Api.Application.Commands.CreateMeasureTypeCommand;

namespace Sofisoft.Erp.MeasureUnits.Tests.Application.Commands
{
    public class CreateMeasureTypeCommandHandlerTest
    {
        private readonly Mock<IIdentityManager> _identityMock;
        private readonly Mock<IRepository<MeasureType>> _measureTypeRepositoryMock;

        public CreateMeasureTypeCommandHandlerTest()
        {
            _measureTypeRepositoryMock = new Mock<IRepository<MeasureType>>();
            _identityMock = new Mock<IIdentityManager>();
        }

        [Fact]
        public void Builder_receivenull_identity_manager_throws()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new CreateMeasureTypeCommandHandler((IIdentityManager) null, _measureTypeRepositoryMock.Object));
        }

        [Fact]
        public void Builder_receivenull_repository_throws()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new CreateMeasureTypeCommandHandler(_identityMock.Object, (IRepository<MeasureType>) null));
        }

        [Fact]
        public async Task Handle_return_item()
        {
            //Arrange
            var username = "test";
            var item = new MeasureType {
                Name = "Peso"
            };
            var command = new CreateMeasureTypeCommand("Peso");

            _identityMock.SetupGet(x => x.Username).Returns(username);

            _measureTypeRepositoryMock.Setup(x => x.InsertOneAsync(It.IsAny<MeasureType>(), It.IsAny<InsertOneOptions>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            //Act
            var commandHandler = new CreateMeasureTypeCommandHandler(_identityMock.Object, _measureTypeRepositoryMock.Object);
            var result = await commandHandler.Handle(command, CancellationToken.None);

            //Assert
            _measureTypeRepositoryMock.Verify(x => x.InsertOneAsync(It.IsAny<MeasureType>(), It.IsAny<InsertOneOptions>(), CancellationToken.None), Times.Once);
        
            Assert.Equal(item.Name, result.Name);
            Assert.Equal(username, result.CreatedBy);
        }

        [Fact]
        public async Task Handle_exists_name_return_domain_exception()
        {
            //Arrange
            var command = new CreateMeasureTypeCommand("Peso");

            _measureTypeRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<MeasureType, bool>>>(), It.IsAny<FindOptions>(), CancellationToken.None))
                .ReturnsAsync(new MeasureType());

            //Act
            var commandHandler = new CreateMeasureTypeCommandHandler(_identityMock.Object, _measureTypeRepositoryMock.Object);

            //Assert
            await Assert.ThrowsAsync<DomainException>(() => commandHandler.Handle(command, CancellationToken.None));
        }
    }
}