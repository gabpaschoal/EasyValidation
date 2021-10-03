namespace EasyValidation.Core.Extensions;

public static class CommonExtensions
{
    public static IRuler<T, R> IsRequired<T, R>(this IRuler<T, R> ruler, string message = "Is required")
            => ruler.On(x => x is null).WithMessage(message);
}
