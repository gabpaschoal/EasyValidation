using EasyValidation.Core.Results;
using FluentAssertions;
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
        var message = "This is a valid message!";
        resultData.AddError(key, message);

        var data = resultData.Errors.Single(x => x.Key == key);
        data.Value.Single().Should().Be(message);
    }


    [Fact(DisplayName = "Should not add a new message when message already exists to this key")]
    public void Should_not_add_a_new_message_when_message_already_exists_to_this_key()
    {
        var resultData = new ResultData();

        var key = "_key";
        var message = "This is a valid message!";

        resultData.AddError(key, message);
        resultData.AddError(key, message);

        var data = resultData.Errors.Single(x => x.Key == key);
        data.Value.Single().Should().Be(message);
    }
}
