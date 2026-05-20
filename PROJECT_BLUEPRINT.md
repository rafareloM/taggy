# Taggy Project Blueprint: Toll, Vehicle, and Impact Management API

## Overview

Taggy is a vehicle and toll management backend for a Flutter client. It manages vehicle registration, tag status, route planning, toll pricing, auto-refill, simulated toll passage, transaction history, and environmental impact reporting.

**Reference Project:** Taggy (GitHub: rafareloM/taggy) - used only as a quality benchmark, not as a template to copy.

---

## Product Scope

### Core User Flow
1. Register a vehicle and its propulsion parameters.
2. Plan a route from origin to destination.
3. Query maps and toll data for distance and plazas on the path.
4. Calculate the total travel cost, including tolls and estimated fuel or battery use.
5. Validate tag status and balance before the trip starts.
6. Auto-refill the balance if needed.
7. Simulate toll passage, debit the balance, and store the transaction.
8. Generate the impact statement with carbon saved or battery efficiency.

### Core Features

1. Vehicle CRUD for combustion and electric vehicles.
2. Route planning with maps and toll lookup.
3. Total cost calculation for fuel, energy, and tolls.
4. Real-time tag and balance validation.
5. Auto-refill trigger when balance is insufficient.
6. Toll passage simulator with transaction logging.
7. Environmental impact report and admin summary endpoints.

---

## Architecture Philosophy

### Why These Layers?

The codebase is split into four projects for separation of concerns:

| Layer | Purpose | Why |
|-------|---------|-----|
| Domain | Entities, value objects, business rules | Defines what Taggy is |
| Application | Use cases, DTOs, orchestration | Defines how Taggy works |
| Infrastructure | EF Core, SQL Server, maps integration, repositories | Technical details only |
| API | Controllers, HTTP routing, middleware, DI | HTTP boundary only |

### Why Not MVC?

MVC is for server-rendered applications. Taggy is a stateless REST API consumed by Flutter, so view rendering would only add unnecessary complexity.

### Why DTOs?

DTOs create a boundary between HTTP and the application layer:
- Controllers return `VehicleDto` or `TripDto`, not entities.
- The client does not need navigation properties or internal persistence details.
- Domain can evolve without breaking the API contract.

---

## Implementation Phases: Order and Reasoning

### Phase 1: Project Setup and Folder Structure

**What to do:** Create the solution, set up the 4 projects, install packages, and create the base folders.

**Why Phase 1?**
- Foundation before implementation.
- Establishes dependency flow and naming conventions.
- Prevents architecture drift later.

**Outcome:** Compiling solution with clear layer boundaries.

---

### Phase 2: Domain Modeling

**What to do:** Define the core Taggy entities and value objects.

**Recommended Domain Model:**
- `Vehicle` - brand, model, year, plate, propulsion type, and propulsion-specific metrics.
- `Tag` - active or maintenance state, balance, minimum balance, last refill date.
- `TollPlaza` - plaza identity, location, concessionary reference.
- `TollTariff` - plaza pricing rules and effective dates.
- `Trip` - origin, destination, status, total cost, start and end timestamps.
- `TollTransaction` - debit record for each passage.

**Value Objects / Enums:**
- `PropulsionType` - Combustion, Electric.
- `TagStatus` - Active, Maintenance, Blocked.
- `TripStatus` - Planned, InProgress, Completed, Cancelled.

**Why Phase 2?**
- Define what exists before deciding how to store it.
- Domain entities should stay independent from SQL Server and HTTP.

**Outcome:** Core domain classes that reflect the README and flowchart.

---

### Phase 3: Persistence and Repository Contracts

**What to do:** Add the repository interfaces, build the DbContext, and map the entities to SQL Server.

**Why Phase 3?**
- Entities drive the schema, not the other way around.
- Repositories keep Application unaware of EF Core.
- This is where the database shape becomes real.

**What Adds:**
- `IRepository<T>` and specialized repository contracts.
- EF Core DbContext in Infrastructure.
- Entity configurations for vehicles, tags, trips, plazas, tariffs, and transactions.
- Migrations for SQL Server.

**Outcome:** Persistable model with clean abstraction boundaries.

---

### Phase 4: Route Planning and Cost Intelligence

**What to do:** Implement the route planner and the total cost calculator.

**What Adds:**
- Maps API integration for distance and toll plaza lookup.
- Fuel or battery consumption estimates.
- Toll cost aggregation.
- Eco-simulator for carbon avoided or battery efficiency comparison.

**Why Phase 4?**
- You need the domain model and persistence before higher-level calculations.
- Route planning is a deterministic application service and should be testable.

**Outcome:** The system can estimate the real travel cost before a trip begins.

---

### Phase 5: Validation and Auto-Refill

**What to do:** Add the status validator, balance checks, and automatic refill flow.

**What Adds:**
- Tag active or maintenance validation.
- Balance validation against total route cost.
- Auto-refill trigger when the balance is insufficient.
- Clear error responses for blocked or invalid travel conditions.

**Why Phase 5?**
- Safety checks must happen before a trip is allowed to start.
- Auto-refill is a business rule, not a controller concern.

**Outcome:** The API can block unsafe trips and recover low balances automatically.

---

### Phase 6: Trip Simulation and Transaction Logging

**What to do:** Emulate toll passage and store each debit in the database.

**What Adds:**
- Trip status changes from planned to in progress.
- Simulated toll antenna detection.
- Transaction insertion on each toll passage.
- Webhook or background service to refresh balance in real time.

**Why Phase 6?**
- Trip execution depends on the previous phases being stable.
- The simulator is a controlled way to emulate real-world toll behavior during development.

**Outcome:** The app records toll passages and keeps the balance synchronized.

---

### Phase 7: Reporting and Admin APIs

**What to do:** Build the impact report and the consolidated admin endpoints.

**What Adds:**
- Environmental impact extraction by vehicle or trip.
- Carbon saved or battery efficiency summaries.
- Admin endpoints for fleet-wide reporting.
- Optional CSV or PDF export.

**Why Phase 7?**
- Reporting is a read-focused layer built on top of stable data.
- It should reuse the calculation and transaction services instead of duplicating logic.

**Outcome:** Users and admins can review the environmental and financial impact of travel.

---

### Phase 8: Validation, Error Handling, and Testing

**What to do:** Add strong validation, centralized exception handling, and tests.

**What Adds:**
- DTO validation attributes.
- Global error middleware.
- Unit tests for services and calculators.
- Integration tests for API endpoints and repository behavior.

**Why Phase 8?**
- After the core behavior exists, validation and tests lock it in.
- Error handling should be consistent across the full API.

**Outcome:** Stable API responses and coverage for the critical flows.

---

## Folder Structure (Target State)

```
taggyManagement.slnx
├── taggyManagement.API/
│   ├── Controllers/
│   │   ├── VehicleController.cs
│   │   ├── TagController.cs
│   │   ├── RouteController.cs
│   │   ├── TripController.cs
│   │   └── ReportController.cs
│   ├── Middlewares/
│   │   └── ErrorHandlingMiddleware.cs
│   ├── Program.cs
│   └── appsettings.json
│
├── taggyManagement.Application/
│   ├── DTOs/
│   │   ├── VehicleDto.cs
│   │   ├── TagDto.cs
│   │   ├── RouteDto.cs
│   │   ├── TripDto.cs
│   │   └── ReportDto.cs
│   ├── Services/
│   │   ├── VehicleService.cs
│   │   ├── RouteCalculatorService.cs
│   │   ├── TagValidationService.cs
│   │   ├── AutoRefillService.cs
│   │   ├── TripSimulationService.cs
│   │   └── ReportService.cs
│   └── Exceptions/
│       └── ApplicationException.cs
│
├── taggyManagement.Domain/
│   ├── Entities/
│   │   ├── Vehicle.cs
│   │   ├── Tag.cs
│   │   ├── TollPlaza.cs
│   │   ├── TollTariff.cs
│   │   ├── Trip.cs
│   │   └── TollTransaction.cs
│   ├── ValueObjects/
│   │   ├── PropulsionType.cs
│   │   ├── TagStatus.cs
│   │   └── TripStatus.cs
│   └── Interfaces/
│       └── IRepository.cs
│
└── taggyManagement.Infrastructure/
    ├── Data/
    │   └── TaggyManagementContext.cs
    ├── Repositories/
    │   ├── GenericRepository.cs
    │   ├── VehicleRepository.cs
    │   ├── TagRepository.cs
    │   └── TripRepository.cs
    └── Services/
        ├── MapsApiService.cs
        ├── TollPricingService.cs
        └── WebhookService.cs
```

---

## Technology Choices and Reasoning

| Choice | Why |
|--------|-----|
| ASP.NET Core 10 | Minimal, fast, and well suited for REST APIs |
| SQL Server | Matches the current project direction and reporting needs |
| Entity Framework Core | Handles migrations and relational persistence cleanly |
| Maps API integration | Supplies route distance and toll path data |
| Dependency Injection | Keeps services testable and swappable |
| DTOs | Prevents domain leakage across the HTTP boundary |
| Middleware | Centralizes exception handling and response formatting |

---

## Key Principles Applied

### 1. Domain-First Design
- Define Vehicle, Tag, Trip, and TollTransaction before storage concerns.

### 2. Single Responsibility
- Vehicle service handles vehicle operations.
- Route service handles route cost and distance.
- Trip simulation service handles passage events.

### 3. Dependency Inversion
- Application depends on interfaces.
- Infrastructure implements them.

### 4. Fail-Fast Validation
- Reject invalid tags, insufficient balances, and bad routes early.

### 5. Testability First
- Calculation, validation, and simulation logic should be mockable and easy to verify.

---

## Success Criteria by Phase

| Phase | Success Looks Like |
|-------|--------------------|
| 1 | Solution builds and folder structure exists |
| 2 | Domain entities compile and match Taggy concepts |
| 3 | DbContext and repository contracts compile |
| 4 | Route cost and eco-simulation return expected values |
| 5 | Tag validation and auto-refill behave correctly |
| 6 | Trip simulation logs transactions and updates balance |
| 7 | Reports aggregate impact correctly |
| 8 | Validation, middleware, and tests cover the critical flows |

---

## Next Steps After This Blueprint

1. Review this document alongside [README.md](README.md) and the flowchart.
2. Start Phase 2 by replacing the placeholder vehicle model with the real domain entities.
3. Keep the layer boundaries intact: Domain first, then Application, then Infrastructure, then API.
