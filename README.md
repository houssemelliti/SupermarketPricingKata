# Supermarket Pricing
Supermarket Pricing TDD Kata


## Overview 
The problem domain is something seemingly simple: pricing goods at supermarkets.
Some things in supermarkets have simple prices: this can of beans costs $0.65. Other things have more complex prices. For example:
- three for a dollar (so what’s the price if I buy 4, or 5?)
- $1.99/pound (so what does 4 ounces cost?)
- buy two, get one free (so does the third item have a price?)

The aim is to experiment with a model that is flexible enough to deal with these (and other) pricing schemes, and at the same time are generally usable
how to keep an audit trail of pricing decisions.

## Technical environment
### Frameworks
- [ASP.NET Core 3.1](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-3.1)
- [Angular 10](https://angular.io/)
### Libraries
- [xUnit](https://xunit.net/)
- [Moq](https://www.nuget.org/packages/moq/)
- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [Swagger](https://swagger.io/)

## Architecture
Server side is developed with ASP.NET Core 3.1 using Service and Repository design patterns:
- Repositories: interface with the data store
- Services: perform business logic
- Controllers: present data to the user and take input from the user

Client side is developed using Angular.

## Build and Run

To build and run the kata, navigate to `./SupermarketPricingKata/` and run the following commands:

    dotnet restore
    dotnet build
    dotnet run

To run the tests, navigate to the `./SupermarketPricingKataTest/` directory
and type the following commands:

    dotnet restore
    dotnet build
    dotnet test

You must run `dotnet restore` in the `./SupermarketPricingKata/` directory before you can run
the tests. `dotnet build` will follow the dependency and build both the library and unit
tests projects, but it will not restore NuGet packages.

## API Documentation
API documentation is done using Swagger following the OpenAPI Specification.
You can access the Swagger UI to visualize and and interact with the API's resources by running the application and navigating to `https://localhost:[port_number]/swagger`.

