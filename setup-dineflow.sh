#!/bin/bash

# Create main solution folder and solution file
dotnet new sln -n DineFlow
mkdir src

# List of services to scaffold
services=("AuthService" "FileService" "InvoiceService" "MenuService" "OrderService" "ReservationService" "AuditService" "GatewayService")

for SERVICE in "${services[@]}"
do
  echo "Scaffolding $SERVICE..."
  cd src
  mkdir $SERVICE
  cd $SERVICE

  dotnet new webapi -n $SERVICE.API
  dotnet new classlib -n $SERVICE.Application
  dotnet new classlib -n $SERVICE.Domain
  dotnet new classlib -n $SERVICE.Infrastructure
  dotnet new xunit -n $SERVICE.Tests

  cd ../..

  dotnet sln add src/$SERVICE/$SERVICE.API/$SERVICE.API.csproj
  dotnet sln add src/$SERVICE/$SERVICE.Application/$SERVICE.Application.csproj
  dotnet sln add src/$SERVICE/$SERVICE.Domain/$SERVICE.Domain.csproj
  dotnet sln add src/$SERVICE/$SERVICE.Infrastructure/$SERVICE.Infrastructure.csproj
  dotnet sln add src/$SERVICE/$SERVICE.Tests/$SERVICE.Tests.csproj

  dotnet add src/$SERVICE/$SERVICE.API/$SERVICE.API.csproj reference src/$SERVICE/$SERVICE.Application/$SERVICE.Application.csproj
  dotnet add src/$SERVICE/$SERVICE.Application/$SERVICE.Application.csproj reference src/$SERVICE/$SERVICE.Domain/$SERVICE.Domain.csproj
  dotnet add src/$SERVICE/$SERVICE.API/$SERVICE.API.csproj reference src/$SERVICE/$SERVICE.Infrastructure/$SERVICE.Infrastructure.csproj
  dotnet add src/$SERVICE/$SERVICE.Tests/$SERVICE.Tests.csproj reference src/$SERVICE/$SERVICE.Application/$SERVICE.Application.csproj
done

# Setup shared libraries
cd src
mkdir Shared
cd Shared

dotnet new classlib -n Shared.Domain
dotnet new classlib -n Shared.Infrastructure
dotnet new classlib -n Shared.Contracts

cd ../..
dotnet sln add src/Shared/Shared.Domain/Shared.Domain.csproj
dotnet sln add src/Shared/Shared.Infrastructure/Shared.Infrastructure.csproj
dotnet sln add src/Shared/Shared.Contracts/Shared.Contracts.csproj

echo "✅ DineFlow solution and projects are ready."
