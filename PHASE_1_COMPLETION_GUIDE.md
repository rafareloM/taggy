# Phase 1: Project Setup & Folder Structure - Completion Guide

## Status: ✅ COMPLETE

Date Completed: April 29, 2026  
.NET Version: 10.0.201  
Solution Name: taggyManagement  
Location: `c:\Users\Marcelo\Desktop\taggy\taggyManagement.slnx`

---

## What You Built

A complete .NET 10 solution following **Clean Architecture** principles with 4 properly layered projects:

```
taggyManagement.slnx (solution file)
├── taggyManagement.Domain/          (No dependencies - pure entities)
├── taggyManagement.Application/     (Depends on Domain - services & DTOs)
├── taggyManagement.Infrastructure/  (Depends on Domain + Application - data access)
└── taggyManagement.API/             (Depends on Application + Infrastructure - HTTP)
```

---

## Success Criteria - ALL MET ✅

| Criteria | Status | Verification |
|----------|--------|---|
| `dotnet --version` returns 10.0+ | ✅ | 10.0.201 confirmed |
| Solution file exists | ✅ | taggyManagement.slnx present |
| 4 projects created | ✅ | Domain, Application, Infrastructure, API |
| Correct project references | ✅ | No circular dependencies |
| NuGet packages installed | ✅ | All 6 packages successfully added |
| Folder structures created | ✅ | All subfolders in place |
| `dotnet build` succeeds | ✅ | 0 errors, all 4 DLLs compiled |

---

## What Each Project Does

### taggyManagement.Domain
**Purpose:** Define what the system IS (entities, rules, value objects)  
**No Dependencies:** Can be used standalone  
**Contains:**
- `Entities/` — Vehicle, Tag, TollPlaza, TollTariff, Trip, TollTransaction (created in Phase 2)
- `ValueObjects/` — PropulsionType, TagStatus, TripStatus enums (created in Phase 2)
- `Interfaces/` — IRepository contract (created in Phase 3)

**Key Rule:** Domain knows nothing about HTTP, databases, or external libraries.

### taggyManagement.Application
**Purpose:** Define how the system WORKS (business logic, use cases)  
**Depends On:** Domain only  
**Contains:**
- `Services/` — VehicleService, RouteCalculatorService, TripSimulationService, etc. (Phases 4-8)
- `DTOs/` — VehicleDto, RouteDto, TripDto, ReportDto, etc. (Phases 4-8)
- `Exceptions/` — Custom exception types (Phase 8)

**Key Rule:** Application orchestrates business logic using Domain entities. Never touches HTTP or database directly.

### taggyManagement.Infrastructure
**Purpose:** Handle technical details (database, external services)  
**Depends On:** Domain + Application  
**Contains:**
- `Data/` — DbContext, database configuration (Phase 3)
- `Repositories/` — GenericRepository, VehicleRepository implementations (Phase 3)
- `Services/` — MapsApiService, TollPricingService, WebhookService (Phases 4-6)

**Key Rule:** Infrastructure implements interfaces defined in Application. Swappable (could replace EF Core with Dapper without touching Application or Domain).

### taggyManagement.API
**Purpose:** HTTP boundary layer (controllers, routing, middleware)  
**Depends On:** Application + Infrastructure  
**Contains:**
- `Controllers/` — VehicleController, RouteController, TripController, ReportController (Phases 4-7)
- `Middlewares/` — ErrorHandlingMiddleware (Phase 8)
- `Program.cs` — Dependency injection setup, middleware pipeline
- `appsettings.json` — Configuration (connection strings, JWT settings)

**Key Rule:** Controllers are thin. They call services (Application layer), format responses, and handle HTTP. No business logic.

---

## NuGet Packages Added

| Package | Layer | Why | Version |
|---------|-------|-----|---------|
| Microsoft.EntityFrameworkCore | Infrastructure | ORM for database access | 10.0.7 |
| System.IdentityModel.Tokens.Jwt | Infrastructure | JWT token generation/validation | 8.17.0 |
| Microsoft.IdentityModel.Tokens | Application | Token validation helpers | 8.17.0 |
| Swashbuckle.AspNetCore | API | Auto-generated Swagger UI docs | 10.1.7 |
| Microsoft.AspNetCore.Authentication.JwtBearer | API | [Authorize] attribute support | 10.0.7 |

**Note:** Use the SQL Server provider in Phase 3 so the infrastructure matches the README and current app direction.

---

## Project References (Dependency Flow)

Verified with `dotnet build` - no circular dependencies:

```
Domain (no references)
  ↓
Application → references Domain
  ↓
Infrastructure → references Domain, Application
  ↓
API → references Application, Infrastructure
```

This flow is critical. If you ever feel the urge to break it (e.g., Domain referencing Application), STOP and create an interface instead.

---

## Folder Structure - Complete Scaffold

### Domain
```
taggyManagement.Domain/
├── Entities/              (Vehicle.cs, Tag.cs, TollPlaza.cs, TollTariff.cs, Trip.cs, TollTransaction.cs)
├── ValueObjects/          (PropulsionType.cs, TagStatus.cs, TripStatus.cs)
├── Interfaces/            (IRepository.cs)
├── Class1.cs              (Delete this - auto-generated)
├── obj/                   (Build artifacts)
├── bin/                   (Compiled DLLs)
└── taggyManagement.Domain.csproj
```

### Application
```
taggyManagement.Application/
├── DTOs/                  (VehicleDto.cs, RouteDto.cs, TripDto.cs, ReportDto.cs, etc.)
├── Services/              (VehicleService.cs, RouteCalculatorService.cs, etc.)
├── Exceptions/            (ApplicationException.cs)
├── Class1.cs              (Delete this)
├── obj/
├── bin/
└── taggyManagement.Application.csproj
```

### Infrastructure
```
taggyManagement.Infrastructure/
├── Data/                  (TaggyManagementContext.cs)
├── Repositories/          (GenericRepository.cs, VehicleRepository.cs, TagRepository.cs)
├── Services/              (MapsApiService.cs, TollPricingService.cs, WebhookService.cs)
├── Class1.cs              (Delete this)
├── obj/
├── bin/
└── taggyManagement.Infrastructure.csproj
```

### API
```
taggyManagement.API/
├── Controllers/           (VehicleController.cs, RouteController.cs, TripController.cs, ReportController.cs)
├── Middlewares/           (ErrorHandlingMiddleware.cs)
├── Properties/            (launchSettings.json)
├── Program.cs             (Dependency injection, middleware pipeline)
├── appsettings.json       (Configuration)
├── appsettings.Development.json
├── obj/
├── bin/
└── taggyManagement.API.csproj
```

---

## Clean Architecture Principles Applied

### 1. Layered Architecture
- Each layer has **one job**
- Layers depend only on inner layers (never outward)
- Enables change independence: swap SQL Server settings or storage details without touching Application/Domain

### 2. Dependency Inversion
- Application defines interfaces (`IRepository`)
- Infrastructure implements them (`GenericRepository`)
- API uses DI to inject implementations at runtime
- Result: Application never knows about EF Core or database directly

### 3. Separation of Concerns
- **Domain:** "What exists?" (entities, rules)
- **Application:** "How does it work?" (services, business logic)
- **Infrastructure:** "Where and how is data stored?" (database, external APIs)
- **API:** "How do clients interact?" (HTTP endpoints)

### 4. Testability
- Services accept repositories as dependencies → can mock for unit tests
- No static methods → everything is injectable
- Domain has no external dependencies → easy to test in isolation

---

## Common Mistakes to Avoid

| ❌ Mistake | Why Wrong | ✅ Fix |
|-----------|-----------|-------|
| Putting HTTP logic in Domain | Domain becomes coupled to web framework | Keep Domain framework-agnostic |
| Returning entities in API responses | Breaks encapsulation, API contracts fragile | Always use DTOs |
| DbContext in Application services | Couples business logic to database | Use IRepository in Application, implement in Infrastructure |
| No error handling middleware | Cryptic 500 errors, hard to debug | Global middleware catches exceptions |
| Business logic in controllers | Controllers bloat, hard to test | Move to services |
| Circular dependencies | Won't compile or breaks at runtime | Respect layer boundaries, use interfaces |

---

## Phase 1 Checklist - All Items Complete

- [x] .NET 10 verified
- [x] Solution created (taggyManagement.slnx)
- [x] 4 projects created with correct templates (classlib/web)
- [x] Project references added in correct order
- [x] All NuGet packages installed without conflict
- [x] Folder structure scaffolded (Entities/, DTOs/, Services/, etc.)
- [x] `dotnet build` passes with 0 errors
- [x] No circular dependencies detected
- [x] Solution structure matches PROJECT_BLUEPRINT.md exactly

---

## What Happens Next: Phase 2 Preview

**Phase 2: Domain Modeling (Vehicle and Toll Flow)**

You'll create the core Taggy domain classes in `taggyManagement.Domain/Entities/`:

1. **Vehicle.cs** — Brand, model, year, plate, propulsion type, and propulsion-specific metrics.
2. **Tag.cs** — Tag status, balance, minimum balance, and refill metadata.
3. **TollPlaza.cs** — Plaza identity and location data.
4. **TollTariff.cs** — Plaza tariff rules and effective dates.
5. **Trip.cs** — Origin, destination, status, and trip totals.
6. **TollTransaction.cs** — Each toll debit with timestamp and amount.

Plus the value objects and enums that drive the flow:
- **PropulsionType** — Combustion, Electric.
- **TagStatus** — Active, Maintenance, Blocked.
- **TripStatus** — Planned, InProgress, Completed, Cancelled.

**Why Phase 2?**
- The domain defines what Taggy is before the database or API shape is finalized.
- Services depend on these classes, so they must exist first.
- This is the clean place to move away from the old placeholder `Car` model.

**When to Start Phase 2:**
Ask me "Ready for Phase 2" and I'll guide you through creating these classes step-by-step, in the same order as the phase roadmap.

---

## Quick Reference Commands

```bash
# Build entire solution
dotnet build taggyManagement.slnx

# Build specific project
dotnet build taggyManagement.Domain/taggyManagement.Domain.csproj

# Run API locally (Phase 4+)
dotnet run --project taggyManagement.API/taggyManagement.API.csproj

# Add a NuGet package
dotnet add taggyManagement.API/taggyManagement.API.csproj package PackageName

# Create a migration (Phase 3+)
dotnet ef migrations add MigrationName --project taggyManagement.Infrastructure --startup-project taggyManagement.API

# Apply migration to database
dotnet ef database update --project taggyManagement.Infrastructure --startup-project taggyManagement.API
```

---

## Architecture Diagram

```
┌─────────────────────────────────────────────────────┐
│                  HTTP Client (Flutter)              │
└─────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────┐
│ taggyManagement.API (Controllers, Middleware, DI)   │
│ • VehicleController, RouteController, etc.         │
│ • ExceptionHandlingMiddleware                       │
│ • Program.cs (dependency injection setup)           │
└─────────────────────────────────────────────────────┘
                          ↓
        ┌─────────────────────────────────────┐
        │ taggyManagement.Application         │
        │ • Services (RouteCalculator, etc.)  │
        │ • DTOs (VehicleDto, TripDto, etc.)  │
        │ • Interfaces (IRepository)          │
        └─────────────────────────────────────┘
         ↙                               ↖
┌──────────────────────────┐  ┌──────────────────────────┐
│ taggyManagement.Domain   │  │taggyManagement.Infra...  │
│ • Entities (Vehicle, Tag)│  │ • DbContext              │
│ • ValueObjects (Status)  │  │ • Repositories           │
│ • Interfaces             │  │ • MapsApiService         │
└──────────────────────────┘  └──────────────────────────┘
                                        ↓
                          ┌──────────────────────────┐
                          │     SQL Server Database  │
                          └──────────────────────────┘
```

---

## Learning Outcomes - Phase 1

By completing Phase 1, you now understand:

1. **Why layering matters** — Each layer solves one problem; changes are localized
2. **Dependency Inversion Principle** — Depend on abstractions, not concrete implementations
3. **Project structure as documentation** — Folder names tell developers where code belongs
4. **Circular dependency dangers** — Why Domain can't reference Application
5. **NuGet package management** — Why packages go where they do
6. **Clean separation of concerns** — HTTP, database, and business logic are independent

---

## You're Ready For Phase 2

Your foundation is solid. The solution structure is correct, dependencies flow properly, and everything builds. You can now move to **Phase 2: Domain Modeling** to create the vehicle, tag, trip, and toll entities.

**Next Steps:**
1. Review this document if you need to refresh on architecture
2. When ready, say "Ready for Phase 2" and I'll guide you through creating Vehicle, Tag, TollPlaza, TollTariff, Trip, and TollTransaction entities
3. Each entity will teach you about route pricing, validation, transactions, and reporting

---

## Questions Before Phase 2?

If anything in Phase 1 is unclear:
- Why is Domain independent?
- How do repositories work?
- What's the difference between entities and DTOs?
- Why can't Application reference API?

Ask and I'll explain. Understanding architecture now saves debugging headaches later.
