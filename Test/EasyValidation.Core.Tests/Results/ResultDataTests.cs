using EasyValidation.Core.Results;
using FluentAssertions;
using System;
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

    [Fact(DisplayName = "Should throws an exception when key param is null")]
    public void Should_throws_an_exception_when_key_param_is_null()
    {
        var resultData = new ResultData();

        string? key = null;

        var message = "This is a valid message!";

#pragma warning disable CS8604 // Possible null reference argument.
        Action act = () => resultData.AddError(key, message);
#pragma warning restore CS8604 // Possible null reference argument.

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Should throws an exception when message param is null")]
    public void Should_throws_an_exception_when_message_param_is_null()
    {
        var resultData = new ResultData();

        var key = "_key";

        string? message = null;

#pragma warning disable CS8604 // Possible null reference argument.
        Action act = () => resultData.AddError(key, message);
#pragma warning restore CS8604 // Possible null reference argument.

        act.Should().Throw<ArgumentNullException>();
    }
}
