namespace EasyValidation.Core.Exceptions;

public class ValidationValueException : Exception
{
    public ValidationValueException() : base("The given command to validate should be valid and not null.")
    { }
}

