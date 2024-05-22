#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Core/MyAssistant.Core.Api/MyAssistant.Core.Api.csproj", "Core/MyAssistant.Core.Api/"]
COPY ["Core/MyAssistant.Core.Contract/MyAssistant.Core.Contract.csproj", "Core/MyAssistant.Core.Contract/"]
COPY ["Core/MyAssistant.Core.Data/MyAssistant.Core.Data.csproj", "Core/MyAssistant.Core.Data/"]
COPY ["Core/MyAssistant.Core.Service/MyAssistant.Core.Service.csproj", "Core/MyAssistant.Core.Service/"]
RUN dotnet restore "./Core/MyAssistant.Core.Api/MyAssistant.Core.Api.csproj"
COPY . .
WORKDIR "/src/Core/MyAssistant.Core.Api"
RUN dotnet build "./MyAssistant.Core.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyAssistant.Core.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyAssistant.Core.Api.dll"]