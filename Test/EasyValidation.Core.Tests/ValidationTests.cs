using EasyValidation.Core.Tests.Stubs;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace EasyValidation.Core.Tests;

public class ValidationTests
{
    private static AddressStubCommand MakeAddress(
        string street = "2nd Av.",
        string neighborhood = "Downtown",
        string city = "NYC",
        int houseNumber = 99)
    {
        return new(street, neighborhood, city, houseNumber);
    }

    private static PeopleStubValidation MakeSut(
        string firstName = "Joseph",
        string lastName = "Smith",
        int age = 90,
        AddressStubCommand? address1 = null,
        AddressStubCommand? address2 = null)
    {
        address1 ??= MakeAddress();
        address2 ??= MakeAddress();

        var peopleStubCommand = new PeopleStubCommand(firstName, lastName, age, address1, address2);
        var validation = new PeopleStubValidation();
        validation.SetValue(peopleStubCommand);

        return validation;
    }

    [Fact(DisplayName = "Should ResultData be valid after create")]
    public void Should_ResultData_be_valid_after_create()
    {
        var peopleStubCommand = new PeopleStubCommand(
            FirstName: "Joseph",
            LastName: "Joestar",
            Age: 0,
            Address1: MakeAddress(),
            Address2: MakeAddress());

        var sut = new PeopleStubValidation();
        sut.SetValue(peopleStubCommand);

        sut.ResultData.Should().NotBeNull();
    }

    [Fact(DisplayName = "Should throws an exception if parameter is null")]
    public void Should_throws_an_exception_if_parameter_is_null()
    {
        var sut = new PeopleStubValidation();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Action act = () => sut.SetValue(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Should add an error in result data when use AddError")]
    public void Should_add_an_error_in_result_data_when_use_AddError()
    {
        var sut = MakeSut();
        var key = "_key";

        sut.AddError(key, "this is a message!");

        sut.ResultData.FieldErrors.Single().Key.Should().Be(key);
    }

    [Fact(DisplayName = "Should HasErrors be true when resultData has errors")]
    public void Should_HasErrors_be_true_when_resultData_has_errors()
    {
        var sut = MakeSut();
        var key = "_key";

        sut.AddError(key, "this is a message!");

        sut.ResultData.FieldErrors.Single().Key.Should().Be(key);
        sut.HasErrors.Should().BeTrue();
    }

    [Fact(DisplayName = "Should HasErrors be false when resultData has no errors")]
    public void Should_HasErrors_be_false_when_resultData_has_no_errors()
    {
        var sut = MakeSut();

        sut.ResultData.FieldErrors.Should().BeEmpty();
        sut.HasErrors.Should().BeFalse();
    }

    [Fact(DisplayName = "Should return a ruler after use ForMember")]
    public void Should_return_a_ruler_after_use_ForMember()
    {
        var sut = MakeSut();

        var ruler = sut.ForMember(x => x.FirstName);

        ruler.Should().NotBeNull();

        sut.ResultData.FieldErrors.Should().BeEmpty();
        sut.HasErrors.Should().BeFalse();
    }

    [Fact(DisplayName = "Should return a ruler after use WithMessage")]
    public void Should_return_a_ruler_after_use_WithMessage()
    {
        var sut = MakeSut();

        var ruler = sut.ForMember(x => x.FirstName).WithMessage("");

        ruler.Should().NotBeNull();

        sut.ResultData.FieldErrors.Should().BeEmpty();
        sut.HasErrors.Should().BeFalse();
    }

    [Fact(DisplayName = "Should return the propertyValue when use GetCommandProperty")]
    public void Should_return_the_propertyValue_when_use_GetValueForValue()
    {
        var sut = MakeSut();

        var ruler = sut.ForMember(x => x.FirstName).WithMessage("");

        ruler.Should().NotBeNull();

        sut.ResultData.FieldErrors.Should().BeEmpty();
        sut.HasErrors.Should().BeFalse();

        var value = sut.GetCommand();

        var firstName = sut.GetCommandProperty(x => x.FirstName);
        var lastName = sut.GetCommandProperty(x => x.LastName);
        var age = sut.GetCommandProperty(x => x.Age);
        var address1 = sut.GetCommandProperty(x => x.Address1);
        var address2 = sut.GetCommandProperty(x => x.Address2);

        var value1 = (firstName, lastName, age, address1, address2);
        var value2 = (value.FirstName, value.LastName, value.Age, value.Address1, value.Address2);

        value1.Should().Be(value2);
    }

    [Fact(DisplayName = "Should not call or add message if already has errors to the current ruler")]
    public void Should_not_call_or_add_message_if_already_has_errors_to_the_current_ruler()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var sut = MakeSut(firstName: null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        sut.Validate();
        sut.ResultData.FieldErrors.Single().Key.Should().Be("FirstName");
        sut.ResultData.FieldErrors.Single().Value.Should().HaveCount(1);
        sut.HasErrors.Should().BeTrue();
    }

    [Fact(DisplayName = "Should call the second validator")]
    public void Should_call_the_second_validator()
    {
        var sut = MakeSut(firstName: "minor");
        sut.Validate();
        sut.ResultData.FieldErrors.Single().Key.Should().Be("FirstName");
        sut.ResultData.FieldErrors.Single().Value.Single().Should().Be("Should have more than 10 digits");
        sut.HasErrors.Should().BeTrue();
    }

    [Fact(DisplayName = "Should call the third validator")]
    public void Should_call_the_third_validator()
    {
        var sut = MakeSut(firstName: "012345678901234567890123456789");
        sut.Validate();
        sut.ResultData.FieldErrors.Single().Key.Should().Be("FirstName");
        sut.ResultData.FieldErrors.Single().Value.Single().Should().Be("Should have minus than 20 digits");
        sut.HasErrors.Should().BeTrue();
    }

    [Fact(DisplayName = "Should add a message error")]
    public void Should_add_a_message_error()
    {
        var sut = MakeSut();
        sut.Validate();
        var message = "This is a valid message!";
        sut.AddMessageError(message);

        sut.ResultData.MessageErrors.Single().Should().Be(message);

        sut.ResultData.IsValid.Should().BeFalse();
        sut.HasErrors.Should().BeTrue();
    }
}