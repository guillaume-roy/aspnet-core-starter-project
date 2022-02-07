# ASP.NET Core - Starter Project

ASP.NET Core 6 starter project including cookie authentication, database access, hexagonal architecture, DDD & CQRS.

Inspired by https://github.com/jasontaylordev/CleanArchitecture

## Technologies

* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
* [MediatR](https://github.com/jbogard/MediatR)
* [BCrypt.Net](https://github.com/BcryptNet/bcrypt.net)
* [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
* [FluentValidation](https://fluentvalidation.net/)
* [xUnit.net](https://xunit.net/), [FluentAssertions](https://fluentassertions.com/)

## Installation

https://dotnet.microsoft.com/en-us/download/dotnet/6.0

## Launch

* On VSCode, press F5 to start server in debug mode

or

* `dotnet watch --project server-api`

or

* `dotnet run --project server-api`

## Build

```bash
dotnet build
```

## Tests

```bash
dotnet test
```

## License
[MIT](https://choosealicense.com/licenses/mit/)