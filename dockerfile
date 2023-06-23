#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7239
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "./"]
COPY ["NuGet.Config", "TCG.CatalogService.API/"]
COPY ["NuGet.Config", "TCG.CatalogService.Application/"]
COPY ["NuGet.Config", "TCG.CatalogService.Domain/"]
COPY ["NuGet.Config", "TCG.CatalogService.Persistence/"]
COPY ["TCG.CatalogService.API/TCG.CatalogService.API.csproj", "TCG.CatalogService.API/"]
COPY ["TCG.CatalogService.Application/TCG.CatalogService.Application.csproj", "TCG.CatalogService.Application/"]
COPY ["TCG.CatalogService.Domain/TCG.CatalogService.Domain.csproj", "TCG.CatalogService.Domain/"]
COPY ["TCG.CatalogService.Persistence/TCG.CatalogService.Persistence.csproj", "TCG.CatalogService.Persistence/"]
RUN dotnet restore "TCG.CatalogService.API/TCG.CatalogService.API.csproj"
RUN dotnet restore "TCG.CatalogService.Application/TCG.CatalogService.Application.csproj"
RUN dotnet restore "TCG.CatalogService.Domain/TCG.CatalogService.Domain.csproj"
RUN dotnet restore "TCG.CatalogService.Persistence/TCG.CatalogService.Persistence.csproj"
COPY . .
WORKDIR "/src/TCG.CatalogService.API"
RUN dotnet build "TCG.CatalogService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TCG.CatalogService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TCG.CatalogService.API.dll"]