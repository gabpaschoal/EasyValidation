using EasyValidation.Core.Results;
using System.Linq.Expressions;

namespace EasyValidation.Core;

public class Ruler<T, TProperty> : IRuler<T, TProperty>
{
    private readonly IResultData _resultData;
    private readonly string _propName;
    private readonly T _value;
    private readonly TProperty _valueProperty;
    private bool _foundError;
    private bool _addedMessage;

    public Ruler(
        IResultData resultData,
        T value,
        Expression<Func<T, TProperty>> expression
        )
    {
        _ = resultData ?? throw new ArgumentNullException(nameof(resultData));
        _ = value ?? throw new ArgumentNullException(nameof(value));
        _ = expression ?? throw new ArgumentNullException(nameof(expression));

        _resultData = resultData;
        _foundError = false;
        _value = value;
        _addedMessage = false;

        var memberExpression = (MemberExpression)expression.Body;
        _propName = memberExpression.Member.Name;

        var builtExpression = expression.Compile();
        _valueProperty = builtExpression.Invoke(_value);
    }

    public IRuler<T, TProperty> When(Func<TProperty, bool> validation)
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
            _resultData.AddError(_propName, message);
        }

        return this;
    }
}
