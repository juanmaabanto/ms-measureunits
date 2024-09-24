using FluentValidation;
using Sofisoft.Erp.MeasureUnits.Api.Application.Commands;

namespace Sofisoft.Erp.MeasureUnits.Api.Application.Validations
{
    public class CreateMeasureTypeValidator : AbstractValidator<CreateMeasureTypeCommand>
    {
        public CreateMeasureTypeValidator()
        {
            RuleFor(command => command.Name).Length(2, 20).NotEmpty();
        }
    }
}