FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 443
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Authentication.Server.csproj", "Authentication.Server/"]
# restore the Authentication.Server.csproj in the source code not the copied one
RUN dotnet restore "Authentication.Server/Authentication.Server.csproj" 
COPY . .
WORKDIR "/src/Authentication.Server"
RUN dotnet build "Authentication.Server.csproj" -c Release -o /app/build


