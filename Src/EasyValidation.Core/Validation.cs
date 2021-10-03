using EasyValidation.Core.Results;
using System.Linq.Expressions;

namespace EasyValidation.Core;

public abstract class Validation<T> : IValidation<T>
        where T : class
{
    private readonly ICollection<ErrorNotification> notifications;

    private T? _value;

    public IResultData ResultData => _resultData;

    private readonly IResultData _resultData;

    public bool HasNotifications => notifications.Any();

    public Validation()
    {
        _resultData = new ResultData();
        notifications = new List<ErrorNotification>();
    }

    public void Setup(T value)
    {
        _value = value;

        Validate();
    }

    public abstract void Validate();

    public void AddNotification(string property, string message)
    {
        _resultData.AddError(key: property, message);
    }

    public void AddNotifications(IEnumerable<ErrorNotification> notifications)
    {
        foreach (var notification in notifications)
        {
            if (!this.notifications.Any(x => x.Equals(notification)))
                this.notifications.Add(notification);
        }
    }

    public void ClearNotifications()
    {
        notifications.Clear();
    }

    public IRuler<T, TProperty> ForMember<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        var ruler = new Ruler<T, TProperty>(_resultData, _value, expression);

        return ruler;
    }

    public void AssignMember<TPartialCommandValidator, TProperty>(Expression<Func<T, TProperty>> expression, bool isRequired = true)
        where TPartialCommandValidator : IValidation<TProperty>, new()
    {
        //var memberExpression = (MemberExpression)expression.Body;
        //var propName = memberExpression.Member.Name;

        //var concatedPropName = string.IsNullOrWhiteSpace(_firstPartName)
        //                                ? propName
        //                                : $"{_firstPartName}.{propName}";

        //var builtExpression = expression.Compile();
        //var valueProperty = builtExpression.Invoke(_value);

        //if (valueProperty is null)
        //{
        //    if (isRequired)
        //        AddNotification(concatedPropName, "");

        //    return;
        //}

        //var partialCommandValidator = new TPartialCommandValidator();
        //partialCommandValidator.Setup(valueProperty, concatedPropName);

        //if (partialCommandValidator.HasNotifications)
        //    AddNotifications(partialCommandValidator.Notifications);
    }
}

