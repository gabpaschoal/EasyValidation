namespace EasyValidation.Core.Extensions;

public static class StringExtensions
{
    public static IRuler<T, string> HasMinLenght<T>(this IRuler<T, string> ruler, int minLenght, string message = "Should have more than {0} digits")
            => ruler.When(x => x.Length < minLenght).WithMessage(string.Format(message, minLenght));

    public static IRuler<T, string> HasMaxLenght<T>(this IRuler<T, string> ruler, int maxLenght, string message = "Should have minus than {0} digits")
            => ruler.When(x => x.Length > maxLenght).WithMessage(string.Format(message, maxLenght));

    public static IRuler<T, string> ShouldBeBetweenLenght<T>(this IRuler<T, string> ruler, int minLenght, int maxLenght,
        string minLenghtMessage = "Should have more than {0} digits",
        string maxLenghtMessage = "Should have minus than {0} digits")
            => ruler.HasMinLenght(minLenght, minLenghtMessage).HasMaxLenght(maxLenght, maxLenghtMessage);

    public static IRuler<T, string> IsNotNullOrEmpty<T>(this IRuler<T, string> ruler, string message = "Should not be null or empty")
            => ruler.When(x => string.IsNullOrEmpty(x)).WithMessage(message);
    public static IRuler<T, string> IsNotNullOrWhiteSpace<T>(this IRuler<T, string> ruler, string message = "Should not be null or white space")
            => ruler.When(x => string.IsNullOrWhiteSpace(x)).WithMessage(message);
}
