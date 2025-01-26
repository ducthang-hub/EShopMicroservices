FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 6001
EXPOSE 6051

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
RUN dotnet dev-certs https --trust

COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
COPY ["BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["Catalog.API/Catalog.API.csproj", "Catalog.API/"]
RUN dotnet restore "BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj"
RUN dotnet restore "BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj"
RUN dotnet restore "Catalog.API/Catalog.API.csproj"
COPY . .
WORKDIR "/src/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/Catalog.API"
RUN dotnet publish "Catalog.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]