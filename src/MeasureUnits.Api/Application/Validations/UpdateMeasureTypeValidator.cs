using FluentValidation;
using Sofisoft.Erp.MeasureUnits.Api.Application.Commands;

namespace Sofisoft.Erp.MeasureUnits.Api.Application.Validations
{
    public class UpdateMeasureTypeValidator : AbstractValidator<UpdateMeasureTypeCommand>
    {
        public UpdateMeasureTypeValidator()
        {
            RuleFor(command => command.Id).Length(24).NotEmpty();
            RuleFor(command => command.Name).Length(2, 20).NotEmpty();
        }
    }
}