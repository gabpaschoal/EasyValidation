using EasyValidation.Core.Results;
using EasyValidation.Core.Tests.Stubs;
using System;
using System.Linq.Expressions;
using Xunit;

namespace EasyValidation.Core.Tests;

public class RulerTests
{
    private static Ruler<PeopleStubCommand, string> MakeSut(
        IResultData? resultData = null,
        PeopleStubCommand? peopleStub = null,
        Expression<Func<PeopleStubCommand, string>>? expression = null)
    {
        resultData ??= new ResultData();

        var addressStub = new AddressStubCommand("St. 1", "Downtown", "NYC", 99);
        peopleStub ??= new PeopleStubCommand("Maria", "Santos", 27, addressStub, addressStub);

        Func<PeopleStubCommand, string> func = peopleStubCommand => peopleStubCommand.FirstName;
        expression ??= peopleStubCommand => func(peopleStubCommand);

        return new Ruler<PeopleStubCommand, string>(resultData, peopleStub, expression);
    }

    [Fact(DisplayName = "Should")]
    public void Should()
    {
        var sut = MakeSut();
        //=>
    }
}