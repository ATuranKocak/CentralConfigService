# 🔹 API için .NET 8 SDK imajı
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ConfigurationAPI/ConfigurationAPI.csproj", "ConfigurationAPI/"]
COPY ["ConfigurationLibrary/ConfigurationLibrary.csproj", "ConfigurationLibrary/"]
RUN dotnet restore "ConfigurationAPI/ConfigurationAPI.csproj"

COPY . .
WORKDIR "/src/ConfigurationAPI"
RUN dotnet build "ConfigurationAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConfigurationAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConfigurationAPI.dll"]
