using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ReviewService.Application.Behaviors;
using ReviewService.Application.Mapping;

namespace ReviewService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }}
