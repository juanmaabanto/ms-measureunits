using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Moq;
using Sofisoft.Erp.MeasureUnits.Api.Application.Queries;
using Sofisoft.Erp.MeasureUnits.Api.Models;
using Sofisoft.MongoDb.Repositories;
using Xunit;
using static Sofisoft.Erp.MeasureUnits.Api.Application.Queries.GetListMeasureTypesQuery;

namespace Sofisoft.Erp.MeasureUnits.Tests.Application.Queries
{
    public class GetListMeasureTypesQueryTest
    {
        private readonly Mock<IRepository<MeasureType>> _measureTypeRepositoryMock;
        
        public GetListMeasureTypesQueryTest()
        {
            _measureTypeRepositoryMock = new Mock<IRepository<MeasureType>>();
        }

        [Fact]
        public async Task Handle_return_tuple_count_and_list()
        {
            //Arrange
            var query = new GetListMeasureTypesQuery("P", string.Empty, 50, 0);
            var expectedItem = new MeasureType {
                Name = "Peso"
            };
            var expectedTotal = 1L;

            _measureTypeRepositoryMock.Setup(x => x.CountAsync(It.IsAny<Expression<Func<MeasureType, bool>>>()
                , It.IsAny<CountOptions>(), CancellationToken.None))
                .ReturnsAsync(expectedTotal);

            _measureTypeRepositoryMock.Setup(x => x.FilterPaginatedAsync(It.IsAny<Expression<Func<MeasureType, bool>>>(),
                It.IsAny<String>(), 50, 0, It.IsAny<FindOptions>(), CancellationToken.None))
                .ReturnsAsync(new List<MeasureType> { expectedItem });

            //Act
            var queryHandler = new GetListMeasureTypesQueryHandler(_measureTypeRepositoryMock.Object);
            var result = await queryHandler.Handle(query, CancellationToken.None);

            //Assert
            Assert.Equal(expectedTotal, result.total);

            foreach(var r in result.data)
            {
                Assert.Equal(expectedItem.Name, r.Name);
            }

        }
    }
}