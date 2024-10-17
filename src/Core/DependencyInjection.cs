using System.Reflection;
using Banhcafe.Microservices.ServiceChargingSystem.Core.Common.Behaviours;
using Banhcafe.Microservices.ServiceChargingSystem.Core.UserPopups.Models;
using Banhcafe.Microservices.ServiceChargingSystem.Core.UserPopups.Validators;
using Banhcafe.Microservices.ServiceChargingSystem.Core.UserSubscriptions.Models;
using Banhcafe.Microservices.ServiceChargingSystem.Core.UserSubscriptions.Validators;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Banhcafe.Microservices.ServiceChargingSystem.Core;
public static class DependencyInjection
{
    public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
    {
        _ = services.AddMediatR(cfg =>
        {
            _ = cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            _ = cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            _ = cfg.AddBehavior(
                typeof(IPipelineBehavior<,>),
                typeof(UnhandledExceptionBehaviour<,>)
            );
        });

        _ = services.AddAutoMapper(c => c.AddProfile(new UserPopups.Mapper.AutoMapperProfile()));
        _ = services.AddAutoMapper(c => c.AddProfile(new UserSubscriptions.Mapper.AutoMapperProfile()));
        _ = services.AddAutoMapper(c => c.AddProfile(new UserServices.Mapper.AutoMapperProfile()));

        // Create validators
        services.AddScoped<IValidator<CreateUserSubscriptions>, CreateUserSubscriptionsValidator>();
        services.AddScoped<IValidator<HideUserPopup>, HideUserPopupsValidator> ();

        return services;
    }
}
