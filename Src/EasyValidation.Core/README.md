# How to use EasyValidation.Core

## Step #1 Create your Command
```cs
public record PersonCommand(string FirstName, string LastName, int Age, AddressCommand Address1, AddressCommand Address2);
public record AddressCommand(string Street, string City, string Neighborhood);
```

## Step #2 Create your Validator
```cs
public class PersonValidator : Validation<PersonCommand>
{
    public override void Validate()
    {
        ForMember(x => x.FirstName)
            .IsRequired()
            .When(x => x is not null && x.Length <= 3).WithMessage("Should has more than 3 chars");

        ForMember(x => x.LastName)
            .IsRequired()
            .When(x => x is not null && x.Length <= 3).WithMessage("Should has more than 3 chars");

        ForMember(x => x.Age)
            .When(x => x < 18).WithMessage("Invalid age");

        AssignMember<AddressValidator, AddressCommand>(x => x.Address1, true);
        AssignMember<AddressValidator, AddressCommand>(x => x.Address2, true);
    }
}

public class AddressValidator : Validation<AddressCommand>
{
    public override void Validate()
    {
        ForMember(x => x.Street)
            .IsRequired()
            .When(x => x is not null && x.Length <= 3).WithMessage("Should has more than 3 chars");

        ForMember(x => x.City)
            .IsRequired()
            .When(x => x is not null && x.Length <= 3).WithMessage("Should has more than 3 chars");

        ForMember(x => x.Neighborhood)
            .IsRequired()
            .When(x => x is not null && x.Length <= 3).WithMessage("Should has more than 3 chars");
    }
}
```

## Step #3 Validate your Command with the Validator
```cs
[HttpPost]
public IActionResult Post([FromBody] PersonCommand command)
{
    var validator = new PersonValidator();

    validator.SetValue(command);
    validator.Validate();

    var jsonResult = validator.ResultData.ToJson();

    if (validator.HasErrors)
        return BadRequest(jsonResult);

    return Ok(jsonResult);
}
```

### Sending this JSON
``` json
{
  "address1": {
  },
  "address2": {
  }
}
```
## Produces this JSON Result
``` json
{
  "Errors": {
    "FirstName": "Is required",
    "LastName": "Is required",
    "Age": "Invalid age",
    "Address1": {
      "Street": "Is required",
      "City": "Is required",
      "Neighborhood": "Is required"
    },
    "Address2": {
      "Street": "Is required",
      "City": "Is required",
      "Neighborhood": "Is required"
    }
  },
  "IsValid": false
}
```

### Author
Made by Guilherme Paschoal 
[![Linkedin Badge](https://img.shields.io/badge/-Guilherme-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/guilherme-paschoal/)](www.linkedin.com/in/guilherme-paschoal/) 
