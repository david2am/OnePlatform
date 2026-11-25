# Variables (like constants you reuse)
api   := "onePlatform.csproj"
infra := "src/Modules/Clearing/Infrastructure"

# Show all available commands when you just type `just`
default:
    @just --list

# Run the API locally
run:
    dotnet run --project {{api}}

# Try the placeholder endpoint
try-api:
    curl -X POST http://localhost:5221/api/payments \
      -H "Content-Type: application/json" \
      -d '{"message":"Hello, OnePlatform"}'

# Set the connection string in user secrets
secrets-init:
    dotnet user-secrets init
    dotnet user-secrets set "ConnectionStrings:Clearing" "Server=localhost,1433;Database=OnePlatformDb;User Id=sa;Password=MyStrongPassword123!;TrustServerCertificate=True;"

# Create the SQL Server container (first time only)
db-create:
    docker run \
      --name oneplatform-sql \
      -e "ACCEPT_EULA=Y" \
      -e "MSSQL_SA_PASSWORD=MyStrongPassword123!" \
      -p 1433:1433 \
      -d mcr.microsoft.com/mssql/server:2022-latest

# Start / stop the existing container
db-start:
    docker start oneplatform-sql

db-stop:
    docker stop oneplatform-sql

# Apply migrations
db-update:
    dotnet ef database update --project {{infra}} --startup-project .

# Create a new migration: just migration AddPayments
migration name:
    dotnet ef migrations add {{name}} --project {{infra}} --startup-project .

# Full reset: drop DB, delete migrations, recreate, apply
db-reset:
    dotnet ef database drop --project {{infra}} --startup-project . --force
    rm -rf {{infra}}/Migrations
    dotnet ef migrations add Initial --project {{infra}} --startup-project .
    dotnet ef database update --project {{infra}} --startup-project .