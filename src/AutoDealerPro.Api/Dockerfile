FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY . .
RUN dotnet publish "AutoDealerPro.sln" -o /publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "AutoDealerPro.Api.dll"]