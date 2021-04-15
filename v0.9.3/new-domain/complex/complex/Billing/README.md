# Billing

This project was created with [Craftsman](https://github.com/pdevito3/craftsman).

## Get Started

Go to your solution directory:

```shell
cd Billing
```

Run your solution:

```shell
dotnet run --project webapi
```

## Running Integration Tests
To run integration tests:

1. Ensure that you have docker installed.
2. Go to your src directory: `cd C:\Users\Paul\Documents\repos\craftsman-examples\v0.9.3\new-domain\complex\complex\\Billing\src`
3. Confirm that you have migrations in your infrastructure project. If not you can add them by doing the following:
    1. Set an environment variable. It doesn't matter what that environment name is for these purposes.
        - Powershell: `$Env:ASPNETCORE_ENVIRONMENT = "IntegrationTesting"`
        - Bash: export `ASPNETCORE_ENVIRONMENT = IntegrationTesting`
    2. Run a Migration (necessary to set up the database) `dotnet ef migrations add "InitialMigration" --project Billing.Infrastructure --startup-project Billing.WebApi --output-dir Migrations`
4. Run the tests. They will take some time on the first run in the last 24 hours in order to set up the docker configuration.
