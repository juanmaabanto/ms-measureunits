using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sofisoft.Abstractions.Responses;
using Sofisoft.Erp.MeasureUnits.Api.Application.Commands;
using Sofisoft.Erp.MeasureUnits.Api.Application.Queries;
using Sofisoft.Erp.MeasureUnits.Api.Controllers;
using Sofisoft.Erp.MeasureUnits.Api.Models;
using Xunit;

namespace Sofisoft.Erp.MeasureUnits.Tests.Controllers
{
    public class MeasureTypesWebApiTest
    {
        private readonly Mock<IMediator> _mediatorMock;

        public MeasureTypesWebApiTest()
        {
            _mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public void Constructor_receivenull_mediator_throws()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new MeasureTypesController((IMediator) null));
        }

        [Fact]
        public async Task GetList_return_result_with_paginated()
        {
            //Arrange
            var query = new GetListMeasureTypesQuery(string.Empty, string.Empty, 50, 0);
            var expectedResult = (2L, GetFakeListMeasureTypes());

            _mediatorMock.Setup(x => x.Send(query, CancellationToken.None))
                .ReturnsAsync(expectedResult);

            var measureTypesController = new MeasureTypesController(_mediatorMock.Object);

            //Act
            var result = await measureTypesController.GetList(string.Empty, string.Empty, 50, 0);

            //Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResponse<MeasureType>>>(result);
            var model = Assert.IsAssignableFrom<PaginatedResponse<MeasureType>>(actionResult.Value);
            Assert.Equal(expectedResult.Item1, model.Total);
        }

        [Fact]
        public async Task Get_measure_type_success()
        {
            //Arrange
            var query = new GetMeasureTypeByIdQuery("001");
            var expectedResult = new MeasureType {
                Name = "fake"
            };

            _mediatorMock.Setup(x => x.Send(query, CancellationToken.None))
                .ReturnsAsync(expectedResult);
            
            var measureTypesController = new MeasureTypesController(_mediatorMock.Object);

            //Act
            var result = await measureTypesController.Get("001", CancellationToken.None);

            //Assert
            var actionResult = Assert.IsType<ActionResult<MeasureType>>(result);
            var model = Assert.IsAssignableFrom<MeasureType>(actionResult.Value);
            Assert.Equal(expectedResult.Name, model.Name);
        }

        [Fact]
        public async Task Post_return_new_measure_type()
        {
            //Arrange
            var command = new CreateMeasureTypeCommand("Fake");
            var expectedResult = new MeasureType {
                Name = "Fake"
            };

            _mediatorMock.Setup(x => x.Send(command, CancellationToken.None))
                .ReturnsAsync(expectedResult);

            var measureTypesController = new MeasureTypesController(_mediatorMock.Object);

            //Act
            var result = await measureTypesController.Post(command, CancellationToken.None);

            //Assert
            var actionResult = Assert.IsType<ActionResult<MeasureType>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<MeasureType>(createdAtActionResult.Value);

            Assert.Equal(expectedResult.Name, returnValue.Name);
        }

        [Fact]
        public async Task Put_return_no_content()
        {
            //Arrange
            var command = new UpdateMeasureTypeCommand("001", "Fake");

            _mediatorMock.Setup(x => x.Send(command, CancellationToken.None))
                .ReturnsAsync(Unit.Value);

            var measureTypesController = new MeasureTypesController(_mediatorMock.Object);

            //Act
            var result = await measureTypesController.Put(command, CancellationToken.None);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        private IEnumerable<MeasureType> GetFakeListMeasureTypes()
        {
            var measureTypes = new List<MeasureType>();

            measureTypes.Add(new MeasureType {
                Name = "Test One"
            });

            measureTypes.Add(new MeasureType {
                Name = "Test Two"
            });

            return measureTypes;
        }

    }
}