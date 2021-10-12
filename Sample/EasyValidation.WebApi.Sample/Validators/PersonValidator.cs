using EasyValidation.Core;
using EasyValidation.Core.Extensions;
using EasyValidation.WebApi.Sample.Commands;

namespace EasyValidation.WebApi.Sample.Validators;
public class PersonValidator : Validation<PersonCommand>
{
    public override void Validate()
    {
        ForMember(x => x.FirstName)
            .IsRequired()
            .When(x => x is not null && x.Length < 3).WithMessage("Should has more than 3 chars");

        ForMember(x => x.LastName)
            .IsRequired()
            .When(x => x is not null && x.Length < 3).WithMessage("Should has more than 3 chars");

        ForMember(x => x.Age)
            .When(x => x < 18).WithMessage("Invalid age");

        AssignMember<AddressValidator, AddressCommand>(x => x.Address1, true);
        AssignMember<AddressValidator, AddressCommand>(x => x.Address2, true);
    }
}