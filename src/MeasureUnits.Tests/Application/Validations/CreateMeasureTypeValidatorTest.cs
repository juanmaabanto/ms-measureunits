using Sofisoft.Erp.MeasureUnits.Api.Application.Commands;
using Sofisoft.Erp.MeasureUnits.Api.Application.Validations;
using Xunit;

namespace Sofisoft.Erp.MeasureUnits.Tests.Application.Validations
{
    public class CreateMeasureTypeValidatorTest
    {
        private CreateMeasureTypeValidator Validator {get;}

        public CreateMeasureTypeValidatorTest()
        {
            Validator = new CreateMeasureTypeValidator();
        }

        [Fact]
        public void Not_allow_empty_name()
        {
            var command = new CreateMeasureTypeCommand(string.Empty);

            Assert.False(Validator.Validate(command).IsValid);
        }

        [Fact]
        public void Not_has_length_requerid()
        {
            var command = new CreateMeasureTypeCommand("m");

            Assert.False(Validator.Validate(command).IsValid);
        }
    }

}