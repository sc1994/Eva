FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Gateways/Aggregator/Aggregator.csproj", "Gateways/Aggregator/"]
COPY ["src/Modules/Toolkit/Toolkit.csproj", "Modules/Toolkit/"]
RUN dotnet restore "Gateways/Aggregator/Aggregator.csproj"

COPY ["src/Gateways/Aggregator/", "Gateways/Aggregator/"]
COPY ["src/Modules/Toolkit/", "Modules/Toolkit/"]
RUN dotnet build "Gateways/Aggregator/Aggregator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateways/Aggregator/Aggregator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ADD sources.list /etc/apt
RUN apt-get update && apt-get install -y curl

ENTRYPOINT ["dotnet", "Aggregator.dll"]
