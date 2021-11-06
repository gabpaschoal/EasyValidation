using EasyValidation.Core.Extensions;
using EasyValidation.Core.Results;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace EasyValidation.Core.Tests.Extensions;

public class StringExtensionsTests
{
    private record StringCommandStub(
        string HasLenght,
        string MinLenght,
        string MaxLenght,
        string ShouldBeBetweenLenght,
        string IsNotNullOrEmpty,
        string IsNotNullOrWhiteSpace);

    private class StringValidatorStub : Validation<StringCommandStub>
    {
        public override void Validate()
        {
            ForMember(x => x.HasLenght).HasLenght(05);
            ForMember(x => x.MinLenght).HasMinLenght(10);
            ForMember(x => x.MaxLenght).HasMaxLenght(10);
            ForMember(x => x.ShouldBeBetweenLenght).ShouldBeBetweenLenght(10, 20);
            ForMember(x => x.IsNotNullOrEmpty).IsNotNullOrEmpty();
            ForMember(x => x.IsNotNullOrWhiteSpace).IsNotNullOrWhiteSpace();
        }
    }

    private static StringCommandStub MakeValidModel()
    {
        return new(
            HasLenght: "Valid",
            MinLenght: "ValidMinLenght",
            MaxLenght: "ValidMax",
            ShouldBeBetweenLenght: "ValidBetweenMessage",
            IsNotNullOrEmpty: "Valid",
            IsNotNullOrWhiteSpace: "Valid"
        );
    }

    private static IResultData MakeAndValidateSut(StringCommandStub commandSut)
    {
        var stub = new StringValidatorStub();
        stub.SetValue(commandSut);
        stub.Validate();

        return stub.ResultData;
    }

    [Fact(DisplayName = "Should add an error when HasLenght is different than required")]
    public void Should_add_an_error_when_HasLenght_is_different_than_required()
    {
        var model = MakeValidModel() with { HasLenght = "Invalid" };
        var resultData = MakeAndValidateSut(model);

        resultData.FieldErrors.Single().Key.Should().BeEquivalentTo(nameof(model.HasLenght));
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

    [Fact(DisplayName = "Should add an error when ShouldBeBetweenLenght is greater than max required")]
    public void Should_add_an_error_when_ShouldBeBetweenLenght_is_greater_than_max_required()
    {
        var model = MakeValidModel() with { ShouldBeBetweenLenght = "InvalidInvalidInvalid" };
        var resultData = MakeAndValidateSut(model);

        resultData.FieldErrors.Single().Key.Should().BeEquivalentTo(nameof(model.ShouldBeBetweenLenght));
    }

    [Fact(DisplayName = "Should add an error when ShouldBeBetweenLenght is minor than min required")]
    public void Should_add_an_error_when_ShouldBeBetweenLenght_is_minor_than_min_required()
    {
        var model = MakeValidModel() with { ShouldBeBetweenLenght = "Invalid" };
        var resultData = MakeAndValidateSut(model);

        resultData.FieldErrors.Single().Key.Should().BeEquivalentTo(nameof(model.ShouldBeBetweenLenght));
    }

    [Fact(DisplayName = "Should add an error when IsNotNullOrEmpty isNull or Empty")]
    public void Should_add_an_error_when_IsNotNullOrEmpty_isNull_or_Empty()
    {
        var modelNullValue = MakeValidModel() with { IsNotNullOrEmpty = null };
        var resultDataNullValue = MakeAndValidateSut(modelNullValue);
        resultDataNullValue.FieldErrors.Single().Key.Should().BeEquivalentTo(nameof(modelNullValue.IsNotNullOrEmpty));

        var modelEmptyValue = MakeValidModel() with { IsNotNullOrEmpty = "" };
        var resultDataEmptyValue = MakeAndValidateSut(modelEmptyValue);
        resultDataEmptyValue.FieldErrors.Single().Key.Should().BeEquivalentTo(nameof(modelEmptyValue.IsNotNullOrEmpty));
    }

    [Fact(DisplayName = "Should add an error when IsNotNullOrWhiteSpace isNull or WhiteSpace")]
    public void Should_add_an_error_when_IsNotNullOrWhiteSpace_isNull_or_WhiteSpace()
    {
        var modelNullValue = MakeValidModel() with { IsNotNullOrWhiteSpace = null };
        var resultDataNullValue = MakeAndValidateSut(modelNullValue);
        resultDataNullValue.FieldErrors.Single().Key.Should().BeEquivalentTo(nameof(modelNullValue.IsNotNullOrWhiteSpace));

        var modelEmptyValue = MakeValidModel() with { IsNotNullOrWhiteSpace = "" };
        var resultDataEmptyValue = MakeAndValidateSut(modelEmptyValue);
        resultDataEmptyValue.FieldErrors.Single().Key.Should().BeEquivalentTo(nameof(modelEmptyValue.IsNotNullOrWhiteSpace));

        var modelWhiteSpaceValue = MakeValidModel() with { IsNotNullOrWhiteSpace = "      " };
        var resultDataWhiteSpaceValue = MakeAndValidateSut(modelEmptyValue);
        resultDataEmptyValue.FieldErrors.Single().Key.Should().BeEquivalentTo(nameof(modelEmptyValue.IsNotNullOrWhiteSpace));
    }
}
