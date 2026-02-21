FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["AutoDealerPro.sln", "./"]
COPY ["src/AutoDealerPro.Api/AutoDealerPro.Api.csproj", "src/AutoDealerPro.Api/"]
COPY ["src/Shared/AutoDealerPro.Shared.Abstractions/AutoDealerPro.Shared.Abstractions.csproj", "src/Shared/AutoDealerPro.Shared.Abstractions/"]
COPY ["src/Shared/AutoDealerPro.Shared.Infrastructure/AutoDealerPro.Shared.Infrastructure.csproj", "src/Shared/AutoDealerPro.Shared.Infrastructure/"]
COPY ["src/Shared/AutoDealerPro.Shared.Kernel/AutoDealerPro.Shared.Kernel.csproj", "src/Shared/AutoDealerPro.Shared.Kernel/"]
COPY ["src/Modules/Inventory/AutoDealerPro.Modules.Inventory.Core/AutoDealerPro.Modules.Inventory.Core.csproj", "src/Modules/Inventory/AutoDealerPro.Modules.Inventory.Core/"]
COPY ["src/Modules/Inventory/AutoDealerPro.Modules.Inventory.Application/AutoDealerPro.Modules.Inventory.Application.csproj", "src/Modules/Inventory/AutoDealerPro.Modules.Inventory.Application/"]
COPY ["src/Modules/Inventory/AutoDealerPro.Modules.Inventory.Infrastructure/AutoDealerPro.Modules.Inventory.Infrastructure.csproj", "src/Modules/Inventory/AutoDealerPro.Modules.Inventory.Infrastructure/"]

# Restore dependencies
RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR "/src/src/AutoDealerPro.Api"
RUN dotnet build "AutoDealerPro.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AutoDealerPro.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutoDealerPro.Api.dll"]