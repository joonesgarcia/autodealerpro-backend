# Runtime base
FROM ubuntu:22.04 AS base
WORKDIR /app
EXPOSE 80
RUN apt-get update && apt-get install -y curl && \
    curl -fsSL https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -o packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update && apt-get install -y aspnetcore-runtime-8.0 && \
    rm -rf /var/lib/apt/lists/*

# Build stage
FROM ubuntu:22.04 AS build
WORKDIR /src
RUN apt-get update && apt-get install -y curl && \
    curl -fsSL https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -o packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update && apt-get install -y dotnet-sdk-8.0 && \
    rm -rf /var/lib/apt/lists/*

# Copy solution and project files
COPY ["AutoDealerPro.sln", "./"]

COPY ["src/AutoDealerPro.Api/AutoDealerPro.Api.csproj", "src/AutoDealerPro.Api/"]

COPY ["src/Shared/AutoDealerPro.Shared.Abstractions/AutoDealerPro.Shared.Abstractions.csproj", "src/Shared/AutoDealerPro.Shared.Abstractions/"]
COPY ["src/Shared/AutoDealerPro.Shared.Kernel/AutoDealerPro.Shared.Kernel.csproj", "src/Shared/AutoDealerPro.Shared.Kernel/"]

COPY ["src/Modules/Inventory/AutoDealerPro.Modules.Inventory.Core/AutoDealerPro.Modules.Inventory.Core.csproj", "src/Modules/Inventory/AutoDealerPro.Modules.Inventory.Core/"]
COPY ["src/Modules/Inventory/AutoDealerPro.Modules.Inventory.Application/AutoDealerPro.Modules.Inventory.Application.csproj", "src/Modules/Inventory/AutoDealerPro.Modules.Inventory.Application/"]
COPY ["src/Modules/Inventory/AutoDealerPro.Modules.Inventory.Infrastructure/AutoDealerPro.Modules.Inventory.Infrastructure.csproj", "src/Modules/Inventory/AutoDealerPro.Modules.Inventory.Infrastructure/"]

COPY ["src/Modules/Leads/AutoDealerPro.Modules.Leads.Core/AutoDealerPro.Modules.Leads.Core.csproj", "src/Modules/Leads/AutoDealerPro.Modules.Leads.Core/"]
COPY ["src/Modules/Leads/AutoDealerPro.Modules.Leads.Application/AutoDealerPro.Modules.Leads.Application.csproj", "src/Modules/Leads/AutoDealerPro.Modules.Leads.Application/"]
COPY ["src/Modules/Leads/AutoDealerPro.Modules.Leads.Infrastructure/AutoDealerPro.Modules.Leads.Infrastructure.csproj", "src/Modules/Leads/AutoDealerPro.Modules.Leads.Infrastructure/"]

COPY ["src/Modules/Auth/AutoDealerPro.Modules.Auth.Core/AutoDealerPro.Modules.Auth.Core/AutoDealerPro.Modules.Auth.Core.csproj", "src/Modules/Auth/AutoDealerPro.Modules.Auth.Core/"]
COPY ["src/Modules/Auth/AutoDealerPro.Modules.Auth.Application/AutoDealerPro.Modules.Auth.Application.csproj", "src/Modules/Auth/AutoDealerPro.Modules.Auth.Application/"]
COPY ["src/Modules/Auth/AutoDealerPro.Modules.Auth.Infrastructure/AutoDealerPro.Modules.Auth.Infrastructure.csproj", "src/Modules/Auth/AutoDealerPro.Modules.Auth.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/AutoDealerPro.Api/AutoDealerPro.Api.csproj"

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