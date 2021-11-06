# How to use EasyValidation.DependencyInjection

## Step #0 Follow first the steps of EasyValidation.Core 😉

## Step #1 Add validators DependencyInjection
In your Program.cs in .net6, you'll add the following code to give the all Assemblies that contains Validators. 
PS. Note that the AddEasyValidationValidators receives a 'params Assembly[] assemblies', so you give all assemblies that uses contains the implementations
```cs
builder.Services.AddEasyValidationValidators(typeof(AddressValidator).Assembly);
```

## Step #2 Examples to the new usage

### Example #1: Using the Locator
```cs
[HttpPost]
public IActionResult Post(
    [FromServices] IValidatorLocator validator,
    [FromBody] PersonCommand command
)
{
    var resultData = validator.ValidateCommand(command);

    var jsonResult = resultData.ToJson();

    if (!resultData.IsValid)
        return BadRequest(jsonResult);

    return Ok(jsonResult);
}
```

### Example #2: Using the interface IValidation<>
```cs
[HttpPost]
public IActionResult Post(
    [FromServices] IValidation<PersonCommand> validator,
    [FromBody] PersonCommand command
)
{
    validator.SetValue(command);
    validator.Validate();

    var jsonResult = validator.ResultData.ToJson();

    if (validator.HasErrors)
        return BadRequest(jsonResult);

    return Ok(jsonResult);
}
```

### Author
Made by Guilherme Paschoal (www.linkedin.com/in/guilherme-paschoal/)

GitHub Repository: https://github.com/gabpaschoal/EasyValidation