FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Gateways/ProxyAggregator/ProxyAggregator.csproj", "Gateways/ProxyAggregator/"]
COPY ["src/Modules/Toolkit/Toolkit.csproj", "Modules/Toolkit/"]
RUN dotnet restore "Gateways/ProxyAggregator/ProxyAggregator.csproj"

COPY ["src/Gateways/ProxyAggregator/", "Gateways/ProxyAggregator/"]
COPY ["src/Modules/Toolkit/", "Modules/Toolkit/"]
RUN dotnet build "Gateways/ProxyAggregator/ProxyAggregator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateways/ProxyAggregator/ProxyAggregator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN apt-get update && apt-get install -y curl

ENTRYPOINT ["dotnet", "ProxyAggregator.dll"]
