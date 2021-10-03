﻿using EasyValidation.Core.Extensions;

namespace EasyValidation.Core.Tests.Stubs;

public class AddressStubValidation : Validation<AddressStubCommand>
{
    public AddressStubValidation(AddressStubCommand value) : base(value)
    { }

    public override void Validate()
    {
        ForMember(x => x.Street).IsRequired();

        ForMember(x => x.City).IsRequired();

        ForMember(x => x.HouseNumber).IsRequired();
    }
}
