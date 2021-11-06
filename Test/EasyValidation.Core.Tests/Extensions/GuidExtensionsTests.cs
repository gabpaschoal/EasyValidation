using EasyValidation.Core.Extensions;
using EasyValidation.Core.Results;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace EasyValidation.Core.Tests.Extensions;

public class GuidExtensionsTests
{
    private record GuidCommandStub(
       Guid IsBeEmpty);

    private class GuidValidatorStub : Validation<GuidCommandStub>
    {
        public override void Validate()
        {
            ForMember(x => x.IsBeEmpty).IsNotEmpty();
        }
    }

    private static GuidCommandStub MakeValidModel()
    {
        return new(
            IsBeEmpty: Guid.NewGuid()
        );
    }

    private static IResultData MakeAndValidateSut(GuidCommandStub commandSut)
    {
        var stub = new GuidValidatorStub();
        stub.SetValue(commandSut);
        stub.Validate();

        return stub.ResultData;
    }

    [Fact(DisplayName = "Should add an error when IsNotBeEmpty is Empty")]
    public void Should_add_an_error_when_IsNotBeEmpty_is_Empty()
    {
        var model = MakeValidModel() with { IsBeEmpty = Guid.Empty };
        var resultData = MakeAndValidateSut(model);

        resultData.FieldErrors.Single().Key.Should().BeEquivalentTo(nameof(model.IsBeEmpty));
    }
}
