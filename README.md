# OnePlatform

OnePlatform is an educational .NET project exploring payment clearing and settlement between participating credit unions. It is an early-stage prototype and does not process real payments or move funds.

## Repository structure

```text
onePlatform.slnx                         Solution containing all four projects
onePlatform.csproj                       ASP.NET Core API and application entry point
src/
  Api/PaymentsController.cs              Payment API endpoint
  Clearing/
    OnePlatform.Clearing.Domain/         Clearing domain model
    OnePlatform.Clearing.Application/    Application use cases
    OnePlatform.Clearing.Infrastructure/ Infrastructure implementations
onePlatform.http                         Sample API request (VS Code REST Client)
```

The solution groups the API entry-point project with the three Clearing projects. The API references the Application and Infrastructure projects; both depend on Domain.

## Run locally

You need the **.NET 10 SDK**. From the repository root, run:

```bash
dotnet run --project onePlatform.csproj
```

The default `http` launch profile serves the API at `http://localhost:5221`. In Development, the OpenAPI document is available at [http://localhost:5221/openapi/v1.json](http://localhost:5221/openapi/v1.json).

## Try the API

Send a request with curl:

```bash
curl -X POST http://localhost:5221/api/payments \
  -H "Content-Type: application/json" \
  -d '{"message":"Hello, OnePlatform"}'
```

The current `POST /api/payments` endpoint is a placeholder: it returns the text `dumb message` and does not yet process the request body. You can also run the sample request in `onePlatform.http` with the VS Code REST Client extension.

## Create the DB

Install Docket Desktop, then create the docker image through:

```bash
docker run \
  --name oneplatform-sql \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=MyStrongPassword123!" \
  -p 1433:1433 \
  -d mcr.microsoft.com/mssql/server:2022-latest

```

# Check if SQL Server is running
docker ps

# Stop SQL Server
docker stop oneplatform-sql

# Start it again
docker start oneplatform-sql

# View logs
docker logs oneplatform-sql


## Store Secrets

Use dotnet user secrets instead of the appsettings file:

```bash
dotnet user-secrets init

dotnet user-secrets set \
  "ConnectionStrings:OnePlatformDb" \
  "Server=localhost,1433;Database=OnePlatformDb;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True;"

```

