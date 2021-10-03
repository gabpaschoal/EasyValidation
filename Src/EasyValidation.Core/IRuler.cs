namespace EasyValidation.Core;

public interface IRuler<T, TProperty>
{
    IRuler<T, TProperty> On(Func<TProperty, bool> validation);
    IRuler<T, TProperty> WithMessage(string message);
}
