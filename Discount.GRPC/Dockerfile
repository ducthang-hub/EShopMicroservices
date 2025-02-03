FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 6003
EXPOSE 6053

# Copy SSL certificate
WORKDIR /https
COPY server.pfx /https/server.pfx

# Install CA certificates for trusting SSL certificates
COPY server.crt /usr/local/share/ca-certificates/server.crt
RUN apt-get update && apt-get install -y ca-certificates && update-ca-certificates

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN dotnet dev-certs https --trust
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
COPY ["BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["Discount.GRPC/Discount.GRPC.csproj", "Discount.GRPC/"]
RUN dotnet restore "BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj"
RUN dotnet restore "BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj"
RUN dotnet restore "Discount.GRPC/Discount.GRPC.csproj"
COPY . .
WORKDIR "/src/Discount.GRPC"
RUN dotnet build "Discount.GRPC.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Discount.GRPC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discount.GRPC.dll"]
