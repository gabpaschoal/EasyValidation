using EasyValidation.Core.Extensions;
using EasyValidation.Core.Results;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace EasyValidation.Core.Tests.Extensions;

public class StringExtensionsTests
{
    private record StringCommandStub(string MinLenght, string MaxLenght);

    private class StringValidatorStub : Validation<StringCommandStub>
    {
        public override void Validate()
        {
            ForMember(x => x.MinLenght).HasMinLenght(10);
            ForMember(x => x.MaxLenght).HasMaxLenght(10);
        }
    }

    private static StringCommandStub MakeValidModel()
    {
        return new(
            MinLenght: "ValidMinLenght",
            MaxLenght: "ValidMax"
        );
    }

    private static IResultData MakeAndValidateSut(StringCommandStub commandSut)
    {
        var stub = new StringValidatorStub();
        stub.SetValue(commandSut);
        stub.Validate();

        return stub.ResultData;
    }

    [Fact(DisplayName = "Should add an error when MinLenght is minor than required")]
    public void Should_add_an_error_when_MinLenght_is_minor_than_required()
    {
        var model = MakeValidModel() with { MinLenght = "Invalid" };
        var resultData = MakeAndValidateSut(model);

        resultData.FieldErrors.Single().Key.Should().BeEquivalentTo(nameof(model.MinLenght));
    }

    [Fact(DisplayName = "Should add an error when MaxLenght is greater than required")]
    public void Should_add_an_error_when_MaxLenght_is_greater_than_required()
    {
        var model = MakeValidModel() with { MaxLenght = "InvalidInvalid" };
        var resultData = MakeAndValidateSut(model);

        resultData.FieldErrors.Single().Key.Should().BeEquivalentTo(nameof(model.MaxLenght));
    }
}
