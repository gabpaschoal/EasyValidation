using EasyValidation.Core.Results;
using EasyValidation.Core.Tests.Stubs;
using Xunit;

namespace EasyValidation.Core.Tests.Results
{
    public class JsonExtensionTests
    {
        private static AddressStubCommand MakeAddress(
            string street = "2",
            string neighborhood = "D",
            string city = "N",
            int houseNumber = 99)
        {
            return new(street, neighborhood, city, houseNumber);
        }

        private static PeopleStubValidation MakeSut(
            string firstName = null,
            string lastName = null,
            int age = 0,
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

        [Fact(DisplayName = "Should return a ruler after use WithMessage")]
        public void Should_return_a_ruler_after_use_WithMessage()
        {
            var sut = MakeSut(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                address1: new AddressStubCommand(null, null, null, 0),
                address2: new AddressStubCommand(null, null, null, 0)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            );

            sut.Validate();

            var json = sut.ResultData.ToJson();
            json = json;
        }
    }
}
