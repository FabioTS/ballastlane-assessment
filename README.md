# E-Commerce Technical Assessment

This repository implements a small full-stack e-commerce catalog using Clean Architecture, ASP.NET Core Web API, Entity Framework Core with SQLite, JWT authentication, and an Angular frontend.

## Project structure

```text
ballastlane/
├── .vscode/
├── ECommerce.Api/
│   ├── Controllers/
│   ├── appsettings.json
│   ├── ECommerce.Api.csproj
│   └── Program.cs
├── ECommerce.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   ├── Services/
│   └── ECommerce.Application.csproj
├── ECommerce.Domain/
│   ├── Entities/
│   ├── Enums/
│   ├── Exceptions/
│   └── ECommerce.Domain.csproj
├── ECommerce.Infrastructure/
│   ├── Data/
│   ├── Repositories/
│   ├── Services/
│   └── ECommerce.Infrastructure.csproj
├── ECommerce.Tests/
│   ├── Api/
│   ├── Application/
│   ├── Domain/
│   ├── Infrastructure/
│   └── ECommerce.Tests.csproj
├── ecommerce-frontend/
│   ├── src/
│   └── package.json
├── .gitignore
├── ECommerceAssessment.sln
├── README.md
├── Net - BLA - Technical Interview Exercise - V6 (1).pdf
└── .vscode/
```

## Architecture

- ECommerce.Domain: entities, enums, domain validation, and domain exceptions
- ECommerce.Application: DTOs, use cases, and business logic contracts
- ECommerce.Infrastructure: EF Core, repositories, JWT token generation, and data seeding
- ECommerce.Api: controllers, auth, OpenAPI, and HTTP endpoints
- ecommerce-frontend: Angular client for authentication and product management

## Features

- User registration and login
- JWT-based authenticated and unauthenticated endpoints
- Product catalog CRUD operations
- Admin-only product creation, update, and delete
- Seeded demo credentials and sample products
- Unit tests for domain, application, infrastructure, and API layers

## Demo credentials

Admin
- Email: admin@ecommerce.local
- Password: Admin123!

User
- Email: user@ecommerce.local
- Password: User123!

## Running the backend

From the repository root:

```bash
dotnet restore
dotnet run --project ECommerce.Api/ECommerce.Api.csproj
```

API available at: https://localhost:7151

OpenAPI document: https://localhost:7151/openapi/v1.json

## Running the frontend

From the repository root:

```bash
cd ecommerce-frontend
npm install
npm start
```

Angular app runs at: http://localhost:4200

## API summary

Public endpoints:
- GET /api/products
- GET /api/products/{id}
- POST /api/auth/register
- POST /api/auth/login

Admin-only endpoints:
- POST /api/products
- PUT /api/products/{id}
- PATCH /api/products/{id}/stock
- DELETE /api/products/{id}

## Testing

```bash
dotnet test ECommerce.Tests/ECommerce.Tests.csproj --nologo
```

## Notes

- The project uses SQLite for local development simplicity and fast setup.
- The app is intentionally small so it stays easy to explain during a technical interview and code review.
- The frontend is designed to demonstrate auth flow and CRUD against the backend without adding unnecessary complexity.
