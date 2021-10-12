using EasyValidation.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EasyValidation.DependencyInjection
{
    public static class EasyValidationDI
    {
        public static IServiceCollection AddEasyValidationValidators(this IServiceCollection service, params Assembly[] assemblies)
        {
            assemblies.SelectMany(x => x.GetTypes())
                .Where(t => TypeFilter(t))
                .ToList()
                .ForEach(currentType =>
                {
                    var validatorType = currentType.GetInterfaces()
                                        .SingleOrDefault(gi => gi.IsGenericType 
                                                            && gi.GetGenericTypeDefinition() == typeof(IValidation<>));

                    if (validatorType is null)
                        return;

                    service.AddSingleton(validatorType, currentType);
                });

            service.AddSingleton<IValidatorLocator, ValidatorLocator>();

            return service;
        }

        private static bool TypeFilter(Type t)
        {
            return t.GetInterfaces().Any(gi => gi.IsGenericType && gi.GetGenericTypeDefinition() == typeof(IValidation<>));
        }
    }
}
