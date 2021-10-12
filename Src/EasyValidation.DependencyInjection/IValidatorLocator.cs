using EasyValidation.Core.Results;

namespace EasyValidation.DependencyInjection
{
    public interface IValidatorLocator
    {
        IResultData ValidateCommand<CommandType>(CommandType command);
    }
}
