using EasyValidation.Core.Extensions;

namespace EasyValidation.Core.Tests.Stubs;

public class PeopleStubValidation : Validation<PeopleStubCommand>
{
    public override void Validate()
    {
        ForMember(x => x.FirstName).IsRequired();

        ForMember(x => x.LastName).IsRequired();

        ForMember(x => x.Age).IsRequired();

        AssignMember<AddressStubValidation, AddressStubCommand>(x => x.Address1, isRequired: false);

        AssignMember<AddressStubValidation, AddressStubCommand>(x => x.Address2, isRequired: false);
    }
}