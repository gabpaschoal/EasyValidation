using EasyValidation.Core.Results;
using System.Linq;
using Xunit;

namespace EasyValidation.Core.Tests.Results;

public class ResultDataTests
{
    [Fact(DisplayName = "Should add a new value when dont exist in the dictionary")]
    public void Should_add_a_new_value_when_dont_exist_in_the_dictionary()
    {
        var resultData = new ResultData();

        var key = "_key";

        resultData.AddError(key, "This is a valid message!");

        var data = resultData.Errors.Single(x => x.Key == key);
    }
}
