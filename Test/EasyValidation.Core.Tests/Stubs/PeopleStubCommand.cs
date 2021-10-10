namespace EasyValidation.Core.Tests.Stubs;

public record PeopleStubCommand(string FirstName, string LastName, int Age, AddressStubCommand Address1, AddressStubCommand Address2);