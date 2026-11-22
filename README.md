# ProductManagementSystem

A REST API for registering users and managing products, built with ASP.NET Core and Entity Framework Core. The application separates domain entities and contracts, application DTOs, infrastructure persistence, and the HTTP API into independent projects.

## Features

- ASP.NET Core Identity registration and login
- Admin and User roles
- Automatic creation of `Admin` and `User` roles
- Automatic seed of a default admin account
- Profile updates for authenticated users
- Product CRUD endpoints
- Role-based product visibility
- Ownership checks for product updates and deletion
- SQL Server persistence
- Swagger/OpenAPI documentation in the Development environment

## Stack

- C# and `.NET 10`
- ASP.NET Core Web API
- ASP.NET Core Identity
- Entity Framework Core 10
- SQL Server
- Swagger / Swashbuckle
- OpenAPI

## Project structure

```text
ProductManagementSystem.slnx       Solution file

Domain/
  Entities/                        User, Product, and Gender
  Interfaces/                      IProductRepository contract

Application/
  Dto/                             Registration, login, and profile DTOs

Infrastructure/
  DB/                              ApplicationDbContext
  Repository/                      EF Core product repository
  Migrations/                      Entity Framework Core migrations

ProductManagementSystem/
  Controllers/                     Authentication and product API controllers
  Program.cs                       Service registration and startup
  appsettings.json                 SQL Server configuration
```

## Architecture

`Program.cs` registers `ApplicationDbContext`, ASP.NET Core Identity, Swagger, controllers, and `IProductRepository` as a scoped service backed by `ProductRepository`.

`ApplicationDbContext` extends `IdentityDbContext<User>` and exposes `DbSet<Product>`. It defines a cascade relationship between users and their products.

`AuthController` handles registration, login, and profile updates through `UserManager<User>` and `SignInManager<User>`.

`ProduktController` handles product operations and applies role and ownership rules:

- Admin users can retrieve all products.
- Regular users retrieve only products associated with their user ID.
- Product creation assigns the authenticated user's ID.
- Updating and deleting require ownership or an Admin role.

## API endpoints

| Method | Endpoint | Authentication | Description |
| --- | --- | --- | --- |
| `POST` | `/api/Auth/register` | No | Register a new User account |
| `POST` | `/api/Auth/Login` | No | Sign in with email and password |
| `PUT` | `/api/Auth/update` | Yes | Update the current user's profile |
| `GET` | `/api/Produkt` | Yes | List products visible to the current user |
| `POST` | `/api/Produkt` | Yes | Create a product for the current user |
| `PUT` | `/api/Produkt/{id}` | Yes | Update an owned product or any product as Admin |
| `DELETE` | `/api/Produkt/{id}` | Yes | Delete an owned product or any product as Admin |

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server
- Entity Framework Core CLI tools (optional, for creating or applying migrations)

Install the EF CLI if needed:

```bash
dotnet tool install --global dotnet-ef
```

### Configure SQL Server

The application currently uses `DefaultConnection` from `ProductManagementSystem/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=LEART;Initial Catalog=ProductMenament;Integrated Security=True;TrustServerCertificate=True"
}
```

For local development, replace this value with your SQL Server instance and database, or provide `ConnectionStrings__DefaultConnection` as an environment variable.

Example:

```bash
export ConnectionStrings__DefaultConnection="Server=localhost;Database=ProductManagement;Integrated Security=True;TrustServerCertificate=True"
```

On Windows PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnection="Server=localhost;Database=ProductManagement;Integrated Security=True;TrustServerCertificate=True"
```

### Apply database migrations

The repository contains migrations in `Infrastructure/Migrations`. Apply them from the repository root:

```bash
dotnet ef database update --project Infrastructure/Infrastructure.csproj --startup-project ProductManagementSystem/ProductManagementSystem.csproj
```

### Run the API

```bash
dotnet restore
 dotnet run --project ProductManagementSystem/ProductManagementSystem.csproj
```

Available launch profiles:

- HTTP: `http://localhost:5044`
- HTTPS: `https://localhost:7260`

In Development, Swagger UI is served by ASP.NET Core with the `/swagger` route.

## Startup behavior

On startup, `Program.cs`:

1. Creates the `Admin` and `User` roles if they do not exist.
2. Creates a default admin account using `malokuleart@gmail.com`.
3. Assigns the seeded account to the `Admin` role.

The default admin password is currently empty in source. Provide a valid password through a reviewed configuration or code change before running the application.

## Domain models

`User` extends `IdentityUser` and includes:

- First name
- Last name
- Phone number
- Date of birth
- Gender

`Gender` supports `Male`, `Female`, and `Other`.

`Product` contains:

- Guid ID
- Name
- Description
- Price
- Category
- Owning user ID

## Development notes

- `RegisterDto.Password` requires at least eight characters, including uppercase, lowercase, a number, and a special character.
- Registration assigns every newly created account to the `User` role.
- Login uses persistent cookie sign-in through ASP.NET Core Identity.
- `DeleteProduct` currently validates ownership or the Admin role but does not call `_productRepository.DeleteAsync(id)` before returning success.
- The `.http` file references a stale `/weatherforecast/` endpoint and should be updated to exercise `/api/Auth` or `/api/Produkt`.
- Avoid committing machine-specific connection strings or account passwords.

## Build

```bash
dotnet build ProductManagementSystem.slnx
```
