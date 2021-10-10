namespace EasyValidation.WebApi.Sample.Commands;

public record PersonCommand(string FirstName, string LastName, int Age, AddressCommand Address1, AddressCommand Address2);