using EasyValidation.Core.Extensions;
using EasyValidation.Core.Results;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace EasyValidation.Core.Tests.Extensions;

public class StringExtensionsTests
{
    private record StringCommandStub(
        string MinLenght, 
        string MaxLenght,
        string ShouldBeBetweenLenght);

    private class StringValidatorStub : Validation<StringCommandStub>
    {
        public override void Validate()
        {
            ForMember(x => x.MinLenght).HasMinLenght(10);
            ForMember(x => x.MaxLenght).HasMaxLenght(10);
            ForMember(x => x.ShouldBeBetweenLenght).ShouldBeBetweenLenght(10, 20);
        }
    }

    private static StringCommandStub MakeValidModel()
    {
        return new(
            MinLenght: "ValidMinLenght",
            MaxLenght: "ValidMax",
            ShouldBeBetweenLenght: "ValidBetweenMessage"
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

    [Fact(DisplayName = "Should add an error when MaxLenght is greater than max required")]
    public void Should_add_an_error_when_MaxLenght_is_greater_than_max_required()
    {
        var model = MakeValidModel() with { ShouldBeBetweenLenght = "InvalidInvalidInvalid" };
        var resultData = MakeAndValidateSut(model);

        resultData.FieldErrors.Single().Key.Should().BeEquivalentTo(nameof(model.ShouldBeBetweenLenght));
    }

    [Fact(DisplayName = "Should add an error when MaxLenght is minor than min required")]
    public void Should_add_an_error_when_MaxLenght_is_minor_than_min_required()
    {
        var model = MakeValidModel() with { ShouldBeBetweenLenght = "Invalid" };
        var resultData = MakeAndValidateSut(model);

        resultData.FieldErrors.Single().Key.Should().BeEquivalentTo(nameof(model.ShouldBeBetweenLenght));
    }
}
