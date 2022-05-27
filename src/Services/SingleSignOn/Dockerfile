FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/SingleSignOn/SingleSignOn.csproj", "Services/SingleSignOn/"]
COPY ["src/Modules/Toolkit/Toolkit.csproj", "Modules/Toolkit/"]
RUN dotnet restore "Services/SingleSignOn/SingleSignOn.csproj"

COPY ["src/Services/SingleSignOn/", "Services/SingleSignOn/"]
COPY ["src/Modules/Toolkit/", "Modules/Toolkit/"]
RUN dotnet build "Services/SingleSignOn/SingleSignOn.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services/SingleSignOn/SingleSignOn.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ADD sources.list /etc/apt
RUN apt-get update && apt-get install -y curl

ENTRYPOINT ["dotnet", "SingleSignOn.dll"]

