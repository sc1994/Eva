FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Demands/Demands.csproj", "Services/Demands/"]
COPY ["src/Modules/Toolkit/Toolkit.csproj", "Modules/Toolkit/"]
RUN dotnet restore "Services/Demands/Demands.csproj"

COPY ["src/Services/Demands/", "Services/Demands/"]
COPY ["src/Modules/Toolkit/", "Modules/Toolkit/"]
RUN dotnet build "Services/Demands/Demands.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services/Demands/Demands.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demands.dll"]
