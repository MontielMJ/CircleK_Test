#!/bin/bash
echo "Inicializando base de datos SQLite..."

# Crear directorio para datos si no existe
mkdir -p ./sqlite-data

# Ejecutar migraciones en el contenedor de la API
docker-compose exec web-api dotnet ef database update --project /app

echo "Base de datos inicializada correctamente"