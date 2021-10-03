using EasyValidation.Core.Tests.Stubs;
using FluentAssertions;
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

        return new(peopleStubCommand);
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

        var sut = new PeopleStubValidation(peopleStubCommand);

        sut.ResultData.Should().NotBeNull();
    }

    [Fact(DisplayName = "Should add an error in result data when use AddError")]
    public void Should_add_an_error_in_result_data_when_use_AddError()
    {
        var sut = MakeSut();
        var key = "_key";

        sut.AddError(key, "this is a message!");

        sut.ResultData.Errors.Single().Key.Should().Be(key);
    }

    [Fact(DisplayName = "Should HasErrors be true when resultData has errors")]
    public void Should_HasErrors_be_true_when_resultData_has_errors()
    {
        var sut = MakeSut();
        var key = "_key";

        sut.AddError(key, "this is a message!");

        sut.ResultData.Errors.Single().Key.Should().Be(key);
        sut.HasErrors.Should().BeTrue();
    }

    [Fact(DisplayName = "Should HasErrors be false when resultData has no errors")]
    public void Should_HasErrors_be_false_when_resultData_has_no_errors()
    {
        var sut = MakeSut();

        sut.ResultData.Errors.Should().BeEmpty();
        sut.HasErrors.Should().BeFalse();
    }

    [Fact(DisplayName = "Should return a ruler after use ForMember")]
    public void Should_return_a_ruler_after_use_ForMember()
    {
        var sut = MakeSut();

        var ruler = sut.ForMember(x => x.FirstName);

        ruler.Should().NotBeNull();

        sut.ResultData.Errors.Should().BeEmpty();
        sut.HasErrors.Should().BeFalse();
    }

    [Fact(DisplayName = "Should return a ruler after use WithMessage")]
    public void Should_return_a_ruler_after_use_WithMessage()
    {
        var sut = MakeSut();

        var ruler = sut.ForMember(x => x.FirstName).WithMessage("");

        ruler.Should().NotBeNull();

        sut.ResultData.Errors.Should().BeEmpty();
        sut.HasErrors.Should().BeFalse();
    }
}