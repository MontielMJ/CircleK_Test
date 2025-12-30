FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los archivos .csproj desde la raíz
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infraestructure/Infraestructure.csproj", "Infraestructure/"]
COPY ["Web.Front/Web.Front.csproj", "Web.Front/"]

# Restaura dependencias para Web.Front
RUN dotnet restore "Web.Front/Web.Front.csproj"

# Copia el resto del código
COPY . .

# Construye el proyecto
WORKDIR "/src/Web.Front"
RUN dotnet build "Web.Front.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.Front.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Variables de entorno por defecto
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "Web.Front.dll"]