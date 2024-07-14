FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
