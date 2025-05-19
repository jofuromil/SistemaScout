# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish "BackendScout.csproj" -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80

# ⬅️ Cambiamos esta línea:
# ENTRYPOINT ["dotnet", "BackendScout.dll"]

# ⬇️ Por esta:
ENTRYPOINT ["sh", "-c", "dotnet ef database update && dotnet BackendScout.dll"]
