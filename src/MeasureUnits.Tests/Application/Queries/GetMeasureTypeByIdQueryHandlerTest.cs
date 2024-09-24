using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Moq;
using Sofisoft.Erp.MeasureUnits.Api.Application.Queries;
using Sofisoft.Erp.MeasureUnits.Api.Infrastructure.Exceptions;
using Sofisoft.Erp.MeasureUnits.Api.Models;
using Sofisoft.MongoDb.Repositories;
using Xunit;
using static Sofisoft.Erp.MeasureUnits.Api.Application.Queries.GetMeasureTypeByIdQuery;

namespace Sofisoft.Erp.MeasureUnits.Tests.Application.Queries
{
    public class GetMeasureTypeByIdQueryHandlerTest
    {
        private readonly Mock<IRepository<MeasureType>> _measureTypeRepositoryMock;
        
        public GetMeasureTypeByIdQueryHandlerTest()
        {
            _measureTypeRepositoryMock = new Mock<IRepository<MeasureType>>();
        }

        [Fact]
        public async Task Handle_return_item()
        {
            //Arrange
            var query = new GetMeasureTypeByIdQuery("001");
            var expectedResult = new MeasureType {
                Name = "Fake"
            };
            
            _measureTypeRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<FindOptions>(), CancellationToken.None))
                .ReturnsAsync(expectedResult);

            //Act
            var queryHandler = new GetMeasureTypeByIdQueryHandler(_measureTypeRepositoryMock.Object);
            var result = await queryHandler.Handle(query, CancellationToken.None);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Handle_throw_measuretype_not_found_exception()
        {
            //Arrange
            var query = new GetMeasureTypeByIdQuery("001");

            _measureTypeRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<FindOptions>(), CancellationToken.None))
                .ReturnsAsync((MeasureType) null);

            //Act
            var queryHandler = new GetMeasureTypeByIdQueryHandler(_measureTypeRepositoryMock.Object);

            //Assert
            await Assert.ThrowsAsync<MeasureTypeNotFoundException>(() => queryHandler.Handle(query, CancellationToken.None));
        }

    }
}