using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac;
using FluentValidation;
using MediatR;
using Sofisoft.Erp.MeasureUnits.Api.Application.Behaviours;
using Sofisoft.Erp.MeasureUnits.Api.Application.Commands;
using Sofisoft.Erp.MeasureUnits.Api.Application.Validations;
using Sofisoft.MongoDb.Behaviors;

namespace Sofisoft.Erp.MeasureUnits.Api.Infrastructure.AutofacModules
{
    [ExcludeFromCodeCoverage]
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(CreateMeasureTypeCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));
            
            builder.RegisterAssemblyTypes(typeof(CreateMeasureTypeValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}