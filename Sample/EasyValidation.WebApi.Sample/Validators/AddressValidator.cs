using EasyValidation.Core;
using EasyValidation.Core.Extensions;
using EasyValidation.WebApi.Sample.Commands;

namespace EasyValidation.WebApi.Sample.Validators;
public class AddressValidator : Validation<AddressCommand>
{
    public override void Validate()
    {
        ForMember(x => x.Street)
            .IsRequired()
            .When(x => x is not null && x.Length < 3).WithMessage("Should has more than 3 chars");

        ForMember(x => x.City)
            .IsRequired()
            .When(x => x is not null && x.Length < 3).WithMessage("Should has more than 3 chars");

        ForMember(x => x.Neighborhood)
            .IsRequired()
            .When(x => x is not null && x.Length < 3).WithMessage("Should has more than 3 chars");
    }
}