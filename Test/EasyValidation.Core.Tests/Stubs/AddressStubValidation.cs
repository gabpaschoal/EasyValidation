using EasyValidation.Core.Extensions;

namespace EasyValidation.Core.Tests.Stubs;

public class AddressStubValidation : Validation<AddressStubCommand>
{
    public override void Validate()
    {
        ForMember(x => x.Street).IsRequired().When(x => x is not null).WithMessage("testmessage");

        ForMember(x => x.City).IsRequired().When(x => x is not null).WithMessage("testmessage");

        ForMember(x => x.HouseNumber).IsRequired().When(x => x != default).WithMessage("testmessage");
    }
}
