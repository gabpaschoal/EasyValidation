using EasyValidation.Core.Results;
using System.Linq.Expressions;

namespace EasyValidation.Core;

public interface IValidation<T> : IDisposable
{
    IResultData ResultData { get; }
    bool HasErrors { get; }

    void AddMessageError(string message);
    void AddError(string property, string message);
    TProperty? GetCommandProperty<TProperty>(Expression<Func<T, TProperty>> expression);
    void SetValue(T value);
    void Validate();
}

