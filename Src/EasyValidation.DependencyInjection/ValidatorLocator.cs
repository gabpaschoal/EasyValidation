using EasyValidation.Core;
using EasyValidation.Core.Results;
using EasyValidation.DependencyInjection.Exceptions;

namespace EasyValidation.DependencyInjection;

internal class ValidatorLocator : IValidatorLocator
{
    private readonly IServiceProvider _serviceProvider;

    public ValidatorLocator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IResultData ValidateCommand<CommandType>(CommandType command)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        var serviceGen = _serviceProvider.GetService(typeof(IValidation<CommandType>));

        if (serviceGen is null)
            throw new ValidatorNotFoundException(command.GetType());

        var service = (IValidation<CommandType>)serviceGen;

        service.SetValue(command);
        service.Validate();

        return service.ResultData;
    }
}
