using System.Dynamic;
using System.Text.Json;

namespace EasyValidation.Core.Results
{
    public static class JsonExtensions
    {
        public static string ToJson(this IResultData resultData)
        {
            var expandoObject = ToExpandoObject(resultData);

            CommonAplies(resultData, ref expandoObject);

            return JsonSerializer.Serialize(expandoObject);
        }

        public static string ToJson<TData>(this IResultData<TData> resultData) where TData : new()
        {
            var expandoObject = ToExpandoObject(resultData);

            CommonAplies(resultData, ref expandoObject);

            if (resultData.Data is not null)
                expandoObject.TryAdd(nameof(resultData.Data), resultData.Data);

            return JsonSerializer.Serialize(expandoObject);
        }

        private static ExpandoObject ToExpandoObject(IResultData resultData)
        {
            var expandoErrorsObject = new ExpandoObject();

            ToExpandoObjectLooper(resultData, ref expandoErrorsObject);

            var expandoResultObject = new ExpandoObject();
            expandoResultObject.TryAdd("Errors", expandoErrorsObject);

            return expandoResultObject;
        }

        private static ExpandoObject ToExpandoObjectLooper(IResultData resultData, ref ExpandoObject expandoObject)
        {
            if (resultData.IsValid)
                return expandoObject;

            foreach (var fieldError in resultData.FieldErrors)
                expandoObject.TryAdd(fieldError.Key, fieldError.Value.First());

            if (!resultData.AssignFieldErrors.Any())
                return expandoObject;

            foreach (var assignsFieldError in resultData.AssignFieldErrors)
            {
                var assignsFieldErrorExpandoObject = new ExpandoObject();

                ToExpandoObjectLooper(assignsFieldError.Value, ref assignsFieldErrorExpandoObject);

                if (!assignsFieldErrorExpandoObject.Any())
                    continue;

                expandoObject.TryAdd(assignsFieldError.Key, assignsFieldErrorExpandoObject);
            }

            return expandoObject;
        }

        private static void CommonAplies(IResultData resultData, ref ExpandoObject expandoObject)
        {
            expandoObject.TryAdd(nameof(resultData.IsValid), resultData.IsValid);
        }
    }
}
