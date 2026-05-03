# ETAPA 1: Compilación con el SDK de .NET 10
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivos de proyecto usando tu estructura NEXUS
COPY ["Nexus.Api/Nexus.Api.csproj", "Nexus.Api/"]
COPY ["Nexus.Application/Nexus.Application.csproj", "Nexus.Application/"]
COPY ["Nexus.Domain/Nexus.Domain.csproj", "Nexus.Domain/"]
COPY ["Nexus.Infrastructure/Nexus.Infrastructure.csproj", "Nexus.Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "Nexus.Api/Nexus.Api.csproj"

# Copiar todo el código y compilar
COPY . .
WORKDIR "/src/Nexus.Api"
RUN dotnet build "Nexus.Api.csproj" -c Release -o /app/build

# ETAPA 2: Publicación
FROM build AS publish
RUN dotnet publish "Nexus.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ETAPA 3: Runtime de .NET 10 (Imagen súper ligera)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .

# Ejecución
ENTRYPOINT ["dotnet", "Nexus.Api.dll"]