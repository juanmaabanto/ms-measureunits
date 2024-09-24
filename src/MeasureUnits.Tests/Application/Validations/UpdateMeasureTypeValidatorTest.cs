using Sofisoft.Erp.MeasureUnits.Api.Application.Commands;
using Sofisoft.Erp.MeasureUnits.Api.Application.Validations;
using Xunit;

namespace Sofisoft.Erp.MeasureUnits.Tests.Application.Validations
{
    public class UpdateMeasureTypeValidatorTest
    {
        private UpdateMeasureTypeValidator Validator {get;}

        public UpdateMeasureTypeValidatorTest()
        {
            Validator = new UpdateMeasureTypeValidator();
        }

        [Fact]
        public void Not_allow_empty_name_and_id()
        {
            var command = new UpdateMeasureTypeCommand(string.Empty, string.Empty);
            
            Assert.False(Validator.Validate(command).IsValid);
        }

        [Fact]
        public void Not_has_length_requerid()
        {
            var command = new UpdateMeasureTypeCommand("001", "m");

            Assert.False(Validator.Validate(command).IsValid);
        }
    }
}