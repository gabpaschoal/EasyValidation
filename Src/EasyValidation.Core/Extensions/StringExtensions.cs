namespace EasyValidation.Core.Extensions;

public static class StringExtensions
{
    public static IRuler<T, string> HasMinLenght<T>(this IRuler<T, string> ruler, int minLenght, string message = "Should have more than {0} digits")
            => ruler.When(x => x.Length < minLenght).WithMessage(string.Format(message, minLenght));
}
