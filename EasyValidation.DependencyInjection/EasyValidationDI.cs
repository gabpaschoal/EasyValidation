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
                .Select(t => Activator.CreateInstance(t))
                .ToList()
                .ForEach(x =>
                {
                    if (x is null)
                        return;

                    service.AddTransient(x.GetType());
                });

            return service;
        }

        private static bool TypeFilter(Type t)
        {
            return t.GetInterfaces().Any(gi => gi.IsGenericType && gi.GetGenericTypeDefinition() == typeof(IValidation<>));
        }
    }
}
