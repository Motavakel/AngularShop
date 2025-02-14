using Application.Common.BehavioursPipes;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace Application;

public static class ConfigureService
{
    public static void AddApplicationServices(this IServiceCollection services)
    {

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


/*        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachedQueryBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));*/

    }
}