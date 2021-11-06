namespace EasyValidation.Core.Extensions;

public static class GuidExtensions
{
    public static IRuler<T, Guid> IsNotEmpty<T>(this IRuler<T, Guid> ruler, string message = "Should not be empty")
        => ruler.When(x => x == Guid.Empty).WithMessage(message);
}
