namespace EasyValidation.DependencyInjection.Exceptions;

public class ValidatorNotFoundException : Exception
{
    public ValidatorNotFoundException(Type commandType)
        : base($"No validator found for command {commandType.Name}")
    { }
}

