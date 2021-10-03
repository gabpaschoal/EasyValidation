using EasyValidation.Core.Results;
using EasyValidation.Core.Tests.Stubs;
using FluentAssertions;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace EasyValidation.Core.Tests;

public class RulerTests
{
    private static AddressStubCommand MakeAddress(
        string? Street = null,
        string? Neighborhood = null,
        string? City = null,
        int? HouseNumber = null
        )
    {
        return new AddressStubCommand(
            Street ?? "St. 1",
            Neighborhood ?? "Downtown",
            City ?? "NYC",
            HouseNumber ?? 99);
    }

    private static PeopleStubCommand MakePeopleStub(
        string? FirstName = null,
        string? LastName = null,
        int? Age = null,
        AddressStubCommand? Address1 = null,
        AddressStubCommand? Address2 = null
        )
    {
        return new PeopleStubCommand(
            FirstName ?? "Maria",
            LastName ?? "Santos",
            Age ?? 27,
            Address1 ?? MakeAddress(),
            Address2 ?? MakeAddress());
    }

    private static Ruler<PeopleStubCommand, string> MakeSut(
        IResultData? resultData = null,
        PeopleStubCommand? peopleStub = null,
        Expression<Func<PeopleStubCommand, string>>? expression = null
        )
    {
        resultData ??= new ResultData();

        var addressStub = new AddressStubCommand("St. 1", "Downtown", "NYC", 99);
        peopleStub ??= new PeopleStubCommand("Maria", "Santos", 27, addressStub, addressStub);

        Func<PeopleStubCommand, string> func = peopleStubCommand => peopleStubCommand.FirstName;
        expression ??= peopleStubCommand => func(peopleStubCommand);

        return new Ruler<PeopleStubCommand, string>(resultData, peopleStub, expression);
    }


    [Fact(DisplayName = "Should throws an exception when resultDataParam is null")]
    public void Should_throws_an_exception_when_resultDataParam_is_null()
    {
        var resultData = new ResultData();

        resultData = null;

        var addressStub = new AddressStubCommand("St. 1", "Downtown", "NYC", 99);
        var peopleStub = new PeopleStubCommand("Maria", "Santos", 27, addressStub, addressStub);

        Func<PeopleStubCommand, string> func = peopleStubCommand => peopleStubCommand.FirstName;
        Expression<Func<PeopleStubCommand, string>> expression = peopleStubCommand => func(peopleStubCommand);

#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => new Ruler<PeopleStubCommand, string>(resultData, peopleStub, expression);
#pragma warning restore CS8604 // Possible null reference argument.

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Should throws an exception when peopleStubParam is null")]
    public void Should_throws_an_exception_when_peopleStubParam_is_null()
    {
        var resultData = new ResultData();

        resultData = null;

        var addressStub = new AddressStubCommand("St. 1", "Downtown", "NYC", 99);
        var peopleStub = new PeopleStubCommand("Maria", "Santos", 27, addressStub, addressStub);
        peopleStub = null;

        Func<PeopleStubCommand, string> func = peopleStubCommand => peopleStubCommand.FirstName;
        Expression<Func<PeopleStubCommand, string>> expression = peopleStubCommand => func(peopleStubCommand);

#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => new Ruler<PeopleStubCommand, string>(resultData, peopleStub, expression);
#pragma warning restore CS8604 // Possible null reference argument.

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Should only add error when WithMessage is called")]
    public void Should_only_add_error_when_validation_is_true_WithMessage_is_called()
    {
        var firstName = "Jeremy";
        var message = "This is a message to validate";

        var resultData = new ResultData();

        var peopleStub = MakePeopleStub(FirstName: firstName);

        var sut = MakeSut(resultData, peopleStub, x => x.FirstName);

        var ruler = sut.When(x => x.Length > 1);

        resultData.Errors.Should().BeEmpty();

        ruler.WithMessage(message: message);

        resultData.Errors.Single().Value.Single().Should().Be(message);
    }

    [Fact(DisplayName = "Should only one add error when validation has more than one error and WithMessage is called")]
    public void Should_only_one_add_error_when_validation_has_more_than_one_error_and_WithMessage_is_called()
    {
        var firstName = "Jeremy";
        var message = "This is a message to validate";

        var resultData = new ResultData();

        var peopleStub = MakePeopleStub(FirstName: firstName);

        var sut = MakeSut(resultData, peopleStub, x => x.FirstName);

        var ruler = sut
                        .When(x => x.Length > 1)
                        .When(x => x.Length > 2)
                        .When(x => x.Length > 2);

        resultData.Errors.Should().BeEmpty();

        ruler.WithMessage(message: message);

        resultData.Errors.Single().Value.Single().Should().Be(message);
    }
}