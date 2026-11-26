
# ============================================================
# Configuration
# ============================================================

api      := "onePlatform.csproj"
clearing := "src/Modules/Clearing/OnePlatform.Clearing.csproj"

db_container := "oneplatform-sql"
db_name      := "OnePlatformDb"
db_password  := "MyStrongPassword123!"

connection_string := "Server=localhost,1433;Database=OnePlatformDb;User Id=sa;Password=MyStrongPassword123!;TrustServerCertificate=True;"

migration_dir := "Shared/Infrastructure/Persistence/SqlServer/Migrations"
db_context    := "ClearingDbContext"

# ============================================================
# General
# ============================================================

default:
    @just --list

build:
    dotnet build

test:
    dotnet test

clean:
    dotnet clean

# ============================================================
# API
# ============================================================

run:
    dotnet run --project {{api}}

watch:
    dotnet watch --project {{api}}

try-api:
    curl -X POST http://localhost:5221/api/payments \
      -H "Content-Type: application/json" \
      -d '{"message":"Hello, OnePlatform"}'

# ============================================================
# Configuration / Secrets
# ============================================================

secrets-init:
    dotnet user-secrets init --project {{api}}

secrets-set:
    dotnet user-secrets set "ConnectionStrings:Clearing" "{{connection_string}}" --project {{api}}

secrets-list:
    dotnet user-secrets list --project {{api}}

# ============================================================
# SQL Server
# ============================================================

db-create:
    docker run \
      --name {{db_container}} \
      -e "ACCEPT_EULA=Y" \
      -e "MSSQL_SA_PASSWORD={{db_password}}" \
      -p 1433:1433 \
      -d mcr.microsoft.com/mssql/server:2022-latest

db-start:
    docker start {{db_container}}

db-stop:
    docker stop {{db_container}}

db-logs:
    docker logs -f {{db_container}}

db-status:
    docker ps -a --filter "name={{db_container}}"

# ============================================================
# Entity Framework Core
# ============================================================

# Apply existing migrations
db-update:
    dotnet ef database update \
      --context {{db_context}} \
      --project {{clearing}} \
      --startup-project {{api}}

# Create a migration
migration name:
    dotnet ef migrations add {{name}} \
      --context {{db_context}} \
      --project {{clearing}} \
      --startup-project {{api}} \
      --output-dir {{migration_dir}}

# Remove the last unapplied migration
migration-remove:
    dotnet ef migrations remove \
      --context {{db_context}} \
      --project {{clearing}} \
      --startup-project {{api}}

# Drop the database
db-drop:
    dotnet ef database drop \
      --context {{db_context}} \
      --project {{clearing}} \
      --startup-project {{api}} \
      --force

# Drop and recreate the database using existing migrations
db-reset: db-drop db-update
