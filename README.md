# Sistema de Votaciones - Setup Backend & Frontend

Este proyecto es un sistema simple de encuestas y votaciones con autenticación JWT, backend en ASP.NET Core y frontend en React.

---

## Requisitos

* .NET 8 SDK o superior
* [Node.js (v18 o superior)](https://nodejs.org/)
* [PostgreSQL](https://www.postgresql.org/download/)

---

## Instalación Backend

1. Clonar el repositorio:

```bash
git clone https://github.com/tu_usuario/tu_repositorio.git
cd tu_repositorio/backend
```

2. Restaurar paquetes NuGet:

```bash
dotnet restore
```

3. Crear base de datos y aplicar migraciones:

Edita el archivo `appsettings.json` con tu cadena de conexión PostgreSQL:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=votingdb;Username=usuario;Password=contraseña"
}
```

Aplicar migraciones:

```bash
dotnet ef database update
```

4. Ejecutar el backend:

```bash
dotnet run
```

Accede a la API: `http://localhost:5027/swagger`

---

## Instalación Frontend

1. Ir al directorio del frontend:

```bash
cd ../frontend
```

2. Instalar dependencias:

```bash
pnpm install   # o npm install
```

3. Variables de entorno:
   Crear archivo `.env`:

```env
VITE_API_URL=http://localhost:5027/api
```

4. Ejecutar frontend:

```bash
pnpm dev   # o npm run dev
```

Accede a: `http://localhost:5173`

---

## Paquetes clave utilizados

### Backend

* Entity Framework Core
* PostgreSQL (Npgsql)
* JWT Authentication
* QuestPDF (para exportación de resultados)

### Frontend

* React + Vite
* React Hook Form
* Shadcn UI (UI components)
* Recharts (gráficos de votos)

---

## Comandos adicionales

Regenerar migraciones:

```bash
dotnet ef migrations add NombreMigracion
```

---

## Autor

Daniel Oliveira Vargas - [GitHub](https://github.com/tu_usuario)
