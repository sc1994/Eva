FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Gateways/HttpAggregator/HttpAggregator.csproj", "Gateways/HttpAggregator/"]
COPY ["src/Modules/Toolkit/Toolkit.csproj", "Modules/Toolkit/"]
RUN dotnet restore "Gateways/HttpAggregator/HttpAggregator.csproj"

COPY ["src/Gateways/HttpAggregator/", "Gateways/HttpAggregator/"]
COPY ["src/Modules/Toolkit/", "Modules/Toolkit/"]
RUN dotnet build "Gateways/HttpAggregator/HttpAggregator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateways/HttpAggregator/HttpAggregator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HttpAggregator.dll"]
