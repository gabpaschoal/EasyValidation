using EasyValidation.Core.Exceptions;
using EasyValidation.Core.Results;
using System.Linq.Expressions;

namespace EasyValidation.Core;

public abstract class Validation<T> : IValidation<T>
        where T : class
{
    private T? _value;

    public IResultData ResultData => _resultData;

    private readonly IResultData _resultData;

    public bool HasErrors => !_resultData.IsValid;

    public Validation()
    {
        _resultData = new ResultData();
    }

    public void SetValue(T value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public abstract void Validate();

    public void AddError(string property, string message)
    {
        _resultData.AddFieldError(key: property, message);
    }

    public IRuler<T, TProperty> ForMember<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        if (_value is null)
            throw new ValidationValueException();

        var ruler = new Ruler<T, TProperty>(_resultData, _value, expression);

        return ruler;
    }

    public TProperty? GetCommandProperty<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        if (_value is null)
            throw new ValidationValueException();

        var builtExpression = expression.Compile();
        var value = builtExpression.Invoke(_value);
        return value;
    }

    public T GetCommand() => _value ?? throw new ValidationValueException();

    public void AssignMember<TPartialCommandValidator, TProperty>(Expression<Func<T, TProperty>> expression, bool isRequired = true)
        where TPartialCommandValidator : IValidation<TProperty>, new()
    {
        if (_value is null)
            throw new ValidationValueException();

        var memberExpression = (MemberExpression)expression.Body;
        var propName = memberExpression.Member.Name;

        var builtExpression = expression.Compile();
        var valueProperty = builtExpression.Invoke(_value);

        if (valueProperty is null)
        {
            if (isRequired)
                AddError(propName, "Is Required");

            return;
        }

        var partialCommandValidator = new TPartialCommandValidator();
        partialCommandValidator.SetValue(valueProperty);
        partialCommandValidator.Validate();

        if (partialCommandValidator.HasErrors)
            ResultData.AssignMember(propName, partialCommandValidator.ResultData);
    }

    public void Dispose()
    {
        _resultData.Dispose();
        GC.SuppressFinalize(this);
    }
}