# 🔹 WebUI için .NET 8 SDK imajı
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ConfigurationWebUI/ConfigurationWebUI.csproj", "ConfigurationWebUI/"]
COPY ["ConfigurationLibrary/ConfigurationLibrary.csproj", "ConfigurationLibrary/"]
RUN dotnet restore "ConfigurationWebUI/ConfigurationWebUI.csproj"

COPY . .
WORKDIR "/src/ConfigurationWebUI"
RUN dotnet build "ConfigurationWebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConfigurationWebUI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConfigurationWebUI.dll"]
