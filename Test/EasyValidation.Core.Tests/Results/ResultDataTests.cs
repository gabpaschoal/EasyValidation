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
        resultData.AddFieldError(key, message);

        var data = resultData.FieldErrors.Single(x => x.Key == key);
        data.Value.Single().Should().Be(message);
    }

    [Fact(DisplayName = "Should not add a new message when message already exists to this key")]
    public void Should_not_add_a_new_message_when_message_already_exists_to_this_key()
    {
        var resultData = new ResultData();

        var key = "_key";
        var message = "This is a valid message!";

        resultData.AddFieldError(key, message);
        resultData.AddFieldError(key, message);

        var data = resultData.FieldErrors.Single(x => x.Key == key);
        data.Value.Single().Should().Be(message);
    }

    [Fact(DisplayName = "Should throws an exception when key param is null")]
    public void Should_throws_an_exception_when_key_param_is_null()
    {
        var resultData = new ResultData();

        string? key = null;

        var message = "This is a valid message!";

#pragma warning disable CS8604 // Possible null reference argument.
        Action act = () => resultData.AddFieldError(key, message);
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
        Action act = () => resultData.AddFieldError(key, message);
#pragma warning restore CS8604 // Possible null reference argument.

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Should add a new message when key exists and the messages are different")]
    public void Should_add_a_new_message_when_key_exists_and_the_messages_are_different()
    {
        var resultData = new ResultData();

        var key = "_key";
        var message1 = "This is a valid message1!";
        var message2 = "This is a valid message2!";
        resultData.AddFieldError(key, message1);
        resultData.AddFieldError(key, message2);

        var data = resultData.FieldErrors.Single(x => x.Key == key);

        data.Value[0].Should().Be(message1);
        data.Value[1].Should().Be(message2);

        data.Value.Count.Should().Be(2);
    }

    [Fact(DisplayName = "Should not be valid when contains any error")]
    public void Should_not_be_valid_when_contains_any_error()
    {
        var resultData = new ResultData();

        var key = "_key";
        var message = "This is a valid message!";
        resultData.AddFieldError(key, message);

        resultData.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Should not add a message error when message is empty")]
    public void Should_not_add_a_message_error_when_message_is_empty()
    {
        var resultData = new ResultData();

        var message = "";
        resultData.AddMessageError(message);

        resultData.MessageErrors.Should().BeEmpty();
        resultData.FieldErrors.Should().BeEmpty();
        resultData.AssignFieldErrors.Should().BeEmpty();

        resultData.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Should not add a message error when message is null")]
    public void Should_not_add_a_message_error_when_message_is_null()
    {
        var resultData = new ResultData();

        string message = null;
        resultData.AddMessageError(message);

        resultData.MessageErrors.Should().BeEmpty();
        resultData.FieldErrors.Should().BeEmpty();
        resultData.AssignFieldErrors.Should().BeEmpty();

        resultData.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Should not add a message error when message is white space")]
    public void Should_not_add_a_message_error_when_message_is_white_space()
    {
        var resultData = new ResultData();

        var message = "                                ";
        resultData.AddMessageError(message);

        resultData.MessageErrors.Should().BeEmpty();
        resultData.FieldErrors.Should().BeEmpty();
        resultData.AssignFieldErrors.Should().BeEmpty();

        resultData.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Should not be valid when contains any message error")]
    public void Should_not_be_valid_when_contains_any_message_error()
    {
        var resultData = new ResultData();

        var message = "This is a valid message!";
        resultData.AddMessageError(message);

        resultData.MessageErrors.Single().Should().Be(message);
        resultData.FieldErrors.Should().BeEmpty();
        resultData.AssignFieldErrors.Should().BeEmpty();

        resultData.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Should not add another message if already exists")]
    public void Should_not_add_another_message_if_already_exists()
    {
        var resultData = new ResultData();

        var message = "This is a valid message!";
        resultData.AddMessageError(message);
        resultData.AddMessageError(message);

        resultData.MessageErrors.Single().Should().Be(message);
        resultData.FieldErrors.Should().BeEmpty();
        resultData.AssignFieldErrors.Should().BeEmpty();

        resultData.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Should be valid when dont contains any error")]
    public void Should_be_valid_when_dont_contains_any_error()
    {
        var resultData = new ResultData();
        resultData.MessageErrors.Should().BeEmpty();
        resultData.FieldErrors.Should().BeEmpty();
        resultData.AssignFieldErrors.Should().BeEmpty();
        resultData.IsValid.Should().BeTrue();
    }
}
