# 🧾 Mini POS – ASP.NET Core (API + MVC)

Prueba técnica Fullstack C# – Punto de Venta (POS)

## 🧠 Descripción
Este proyecto implementa un **mini Punto de Venta (POS)** utilizando **Clean Architecture**, con:

- API en ASP.NET Core
- Frontend MVC (Razor Views)
- EF Core + SQLite
- Bootstrap 5 + Bootstrap Icons
- Flujo completo de ventas, pagos e inventario

---

## 🧱 Arquitectura

La solución está organizada siguiendo **Clean Architecture**:

├── Domain → Entidades y reglas de negocio
├── Application → DTOs, UseCases, Commands
├── Infrastructure → EF Core, DbContext, Repositories
├── Web.Api → API REST
└── Web.Front → MVC (POS y CRUD Productos)


---

## 🛠️ Stack Tecnológico

- .NET 8
- ASP.NET Core (API + MVC)
- Entity Framework Core
- SQLite
- Bootstrap 5
- Bootstrap Icons
- Swagger (API)

---

## ⚙️ Requisitos Previos

- .NET SDK 8 o superior
- Visual Studio 2022 / VS Code
- Git

---

## 🚀 Ejecución del Proyecto

### 1️⃣ Clonar repositorio

git clone https://github.com/tu-usuario/mini-pos.git
cd mini-pos


## Restaurar Dependencias

dotnet restore

### Base de Datos SQL Lite

cd src/Web.Api

dotnet ef database update

### Ejecutar la solución

dotnet run --project src/Web.Api

dotnet run --project src/Web.Front

### 🔍 API – Swagger
https://localhost:{puerto}/swagger

### 🧪 Flujo de Inventario

El stock se descuenta al pagar una venta

No se permite stock negativo

Cancelar una venta revierte el stock


### 👨‍💻 Autor

Juan M
Desarrollador .NET
