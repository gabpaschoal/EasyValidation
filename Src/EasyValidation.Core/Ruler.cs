using System.Linq.Expressions;

namespace EasyValidation.Core;

public class Ruler<T, TProperty> : IRuler<T, TProperty>
{
    private readonly IValidation<T> _validation;
    private readonly string _firstPartName;
    private readonly string _propName;
    private readonly T _value;
    private readonly TProperty _valueProperty;
    private bool _foundError;
    private bool _addedMessage;

    private string FullPropName => string.IsNullOrWhiteSpace(_firstPartName) ? _propName : $"{_firstPartName}.{_propName}";

    public Ruler(
        IValidation<T> validation,
        T value,
        Expression<Func<T, TProperty>> expression,
        string firstPartName = ""
        )
    {
        _validation = validation;
        _foundError = false;
        _value = value;
        _addedMessage = false;
        _firstPartName = firstPartName;

        var memberExpression = (MemberExpression)expression.Body;
        _propName = memberExpression.Member.Name;

        var builtExpression = expression.Compile();
        _valueProperty = builtExpression.Invoke(_value);
    }

    public IRuler<T, TProperty> On(Func<TProperty, bool> validation)
    {
        if (_foundError)
            return this;

        if (validation.Invoke(_valueProperty))
            _foundError = true;

        return this;
    }

    public IRuler<T, TProperty> WithMessage(string message)
    {
        if (_foundError && !_addedMessage)
        {
            _addedMessage = true;
            _validation.AddNotification(FullPropName, message);
        }

        return this;
    }
}
