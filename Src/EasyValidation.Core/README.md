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
### Produces this JSON Result
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

### Custom Validator - Generic
To create a generic method to reuse in many Validators, you can create a static class and method that extends IRuler<T, R> and you'll use the generics like in the samples below.
```cs
public static class CommonExtensions
{
    public static IRuler<T, R> IsRequired<T, R>(this IRuler<T, R> ruler, string message = "Is required")
            => ruler.When(x => x is null).WithMessage(message);
}
```

### Custom Validator - Typed
To create a typed method to reuse in many Validators, you can create a static class that extends IRuler<T, R>, but, to type your model property, is required to specify the second generic in the IRuler<,> like in the samples below.
```cs
public static class StringExtensions
{
    public static IRuler<T, string> HasMinLenght<T>(this IRuler<T, string> ruler, int minLenght, string message = "Should have more than {0} digits")
            => ruler.When(x => x.Length < minLenght).WithMessage(string.Format(message, minLenght));
}
public static class GuidExtensions
{
    public static IRuler<T, Guid> IsNotEmpty<T>(this IRuler<T, Guid> ruler, string message = "Should not be empty")
        => ruler.When(x => x == Guid.Empty).WithMessage(message);
}
```


### Author
Made by Guilherme Paschoal (www.linkedin.com/in/guilherme-paschoal/)

GitHub Repository: https://github.com/gabpaschoal/EasyValidation