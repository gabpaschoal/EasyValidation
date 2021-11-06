using EasyValidation.Core.Results;
using EasyValidation.DependencyInjection.Exceptions;

namespace EasyValidation.DependencyInjection;

public interface IValidatorLocator
{
    /// <summary>
    /// This Method find and validate the command
    /// </summary>
    /// <returns>
    /// IResultData with the message from the Validator
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Return an exception when the command is null
    /// </exception>
    /// <exception cref="ValidatorNotFoundException">
    /// Return an exception when don't find the validator for this command
    /// </exception>
    IResultData ValidateCommand<CommandType>(CommandType command);
}
