# EF Core client-side evaluation test cases

This is a test project for reproducing some Entity Framework Core issues related specifically to cases where SQL translation fails.
Some tests fail (meaning EF could not translate the query to SQL) while others pass to demonstrate similar queries which translate successfully.
To run the tests, clone the repo and run `dotnet test`.

Related EF Core issues:
* [#12728](https://github.com/aspnet/EntityFrameworkCore/issues/12728)
