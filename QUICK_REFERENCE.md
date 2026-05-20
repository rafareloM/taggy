# taggyManagement - Quick Reference Card

## Project Structure at a Glance

```
taggyManagement.Domain              → Pure entities, no dependencies
  ↓ depends on Domain only
taggyManagement.Application         → Business logic, services, DTOs
  ↓ depends on Domain + Application
taggyManagement.Infrastructure      → Database, external services
  ↓ depends on Application + Infrastructure
taggyManagement.API                 → HTTP layer, controllers, routing
```

## Where to Put Things

| What | Where | Example |
|------|-------|---------|
| Entity classes | Domain/Entities/ | Vehicle.cs, Tag.cs |
| Enums/Constants | Domain/ValueObjects/ | PropulsionType.cs |
| Repository interfaces | Domain/Interfaces/ | IRepository.cs |
| Service classes | Application/Services/ | RouteCalculatorService.cs |
| Data transfer objects | Application/DTOs/ | VehicleDto.cs |
| Custom exceptions | Application/Exceptions/ | ApplicationException.cs |
| DbContext | Infrastructure/Data/ | TaggyManagementContext.cs |
| Repository implementations | Infrastructure/Repositories/ | GenericRepository.cs |
| External service implementations | Infrastructure/Services/ | MapsApiService.cs |
| Controllers | API/Controllers/ | VehicleController.cs |
| Middleware | API/Middlewares/ | ErrorHandlingMiddleware.cs |

## Golden Rules

1. **Domain can never reference anything else**
2. **Application can only reference Domain**
3. **Infrastructure can reference Domain + Application**
4. **API can reference Application + Infrastructure**
5. **Never go backwards** - if you need to, create an interface

## Build Commands

```bash
# Build everything
dotnet build taggyManagement.slnx

# Check for errors
dotnet build taggyManagement.slnx 2>&1 | grep error

# Clean build artifacts
dotnet clean
```

## Current Status

| Item | Status |
|------|--------|
| Solution created | ✅ |
| Projects created | ✅ |
| References correct | ✅ |
| Packages installed | ✅ |
| Folder structure | ✅ |
| Build successful | ✅ |
| Next: Domain entities | 👉 Phase 2 |

## Key Concepts

- **Layered Architecture** = Each layer solves one problem
- **Dependency Inversion** = Depend on interfaces, not concrete classes
- **Separation of Concerns** = HTTP, business logic, and database are separate
- **Testability** = Everything is injectable; nothing is static
- **DTOs** = Boundary objects between API and domain

## Phase 2 Preview

Create the Taggy domain in Domain/Entities/:
- Vehicle
- Tag
- TollPlaza
- TollTariff
- Trip
- TollTransaction

Plus enums in Domain/ValueObjects/:
- PropulsionType (Combustion, Electric)
- TagStatus (Active, Maintenance, Blocked)
- TripStatus (Planned, InProgress, Completed, Cancelled)
