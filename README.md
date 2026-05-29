# AutoDealerPro Backend

A modern, modular ASP.NET Core backend for managing automobile dealership operations. Built with .NET 8, following Clean Architecture principles with a modular, domain-driven design.

## 🎯 Overview

AutoDealerPro is a comprehensive inventory and lead management system designed for car dealerships. It provides features for tracking vehicles, managing customer inquiries (leads), and handling staff authentication with role-based access control.

The backend is built as a **modular monolith** using a **Plugin Architecture** pattern, allowing independent feature modules to be developed, tested, and maintained separately while running in the same process.

## 🚀 How do I run this ?

### Prerequisites

- .NET 8 SDK
- Docker Desktop

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/joonesgarcia/autodealerpro-backend.git
   cd autodealerpro-backend
   ```

2. **Up the environment with docker**
   ```bash
   docker compose up
   ```

3. **Access Swagger Documentation**
   ```
   http://localhost:5001/swagger
   ```

4. **Test open endpoints or follow next steps for authentication**

## 🔐 Authentication & Authorization

### JWT Token Generation

1. **Register a new user account**
   ```bash
   POST /auth/register
   {
     "username": "you_really_cool_username",
     "email": "yourname@yourdomain.com",
     "password": "SecurePassword123!"
   }
   ```

2. **Login and get token**
   ```bash
   POST /auth/login
   {
     "username": "you_really_cool_username",
     "password": "SecurePassword123!!"
   }

   or use a admin one:

   POST /auth/login
   {
     "username": "theadministrator",
     "password": "astrongpassword"
   }
   ```
   Response:
   ```json
   {
     "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
   }
   ```

3. **Use token in requests, you can use it at swagger Authorize button (: **
   ```bash
   Authorization: Bearer <token>
   ```

## 🏗️ Architecture

### Design Principles

- **Clean Architecture**: Separation of concerns across layers (Core, Application, Infrastructure)
- **Domain-Driven Design**: Rich domain models with business logic encapsulation
- **Modular Monolith**: Plugin-based architecture for feature modules
- **CQRS-Inspired**: Clear separation between command (write) and query (read) operations
- **Event-Driven**: Inter-module communication through domain events

### Project Structure

```
src/
├── AutoDealerPro.Api/                    # Main API entry point
├── Shared/
│   ├── AutoDealerPro.Shared.Kernel/      # Domain model base classes
│   ├── AutoDealerPro.Shared.Abstractions/# Interfaces and abstractions
│   └── AutoDealerPro.Shared.Infrastructure/ # Shared infrastructure
└── Modules/
    ├── Auth/                              # Authentication & Authorization
    │   ├── AutoDealerPro.Modules.Auth.Core/
    │   └── AutoDealerPro.Modules.Auth.Infrastructure/
    ├── Inventory/                         # Vehicle Inventory Management
    │   ├── AutoDealerPro.Modules.Inventory.Core/
    │   ├── AutoDealerPro.Modules.Inventory.Application/
    │   └── AutoDealerPro.Modules.Inventory.Infrastructure/
    └── Leads/                             # Customer Leads Management
        ├── AutoDealerPro.Modules.Leads.Core/
        ├── AutoDealerPro.Modules.Leads.Application/
        └── AutoDealerPro.Modules.Leads.Infrastructure/
```

### Layer Breakdown

Each module follows a 3-layer structure:

#### Core Layer
- **Domain Models**: Rich entity classes with business logic
- **Repositories**: Interfaces for data access abstraction
- **Events**: Domain events for inter-module communication
- **Enums**: Domain constants and enumerations
- **No External Dependencies**: Pure business logic

#### Application Layer
- **Request/Response DTOs**: API contract models
- **Validators**: FluentValidation rules for request data
- **Services**: Application service interfaces
- **No Database Knowledge**: Orchestrates domain logic

#### Infrastructure Layer
- **Database Context**: EntityFrameworkCore DbContext
- **Repository Implementations**: Concrete data access
- **Services**: Concrete service implementations
- **Event Handlers**: Domain event subscribers
- **Endpoints**: HTTP endpoint mappings
- **Module Registration**: Dependency injection setup

## 📦 Modules

### 1. **Authentication & Authorization Module**

Handles user authentication using JWT tokens and role-based authorization.

**Key Features:**
- User account creation with password hashing
- JWT token generation and validation
- Role-based access control (Staff, Admin)
- In-memory user storage (extendable to database)

**Key Entities:**
```csharp
User
├── Id
├── Username
├── Email
├── PasswordHash
├── EmailConfirmed
└── Roles []
```

**Endpoints:**
- `POST /auth/register` - Create new staff account
- `POST /auth/login` - Authenticate and get JWT token

**Technologies:**
- JWT Bearer Authentication
- ASP.NET Core Identity (PasswordHasher)
- FluentValidation

---

### 2. **Inventory Module**

Manages vehicle inventory with comprehensive vehicle data and lifecycle tracking.

**Key Features:**
- Add vehicles to inventory with detailed specifications
- Update vehicle pricing, mileage, and photos
- Track vehicle status (Available, Sold, Reserved)
- View count tracking for customer engagement analytics
- Mark vehicles as sold (triggers lead closure via events)

**Key Entities:**
```csharp
Vehicle
├── Id
├── Make, Model, Year, Trim
├── PlateNumber
├── Mileage
├── ExteriorColor, InteriorColor
├── Transmission, FuelType, BodyType
├── PurchasePrice, AskingPrice, SellingPrice
├── Status (Available | Sold | Reserved)
├── PhotoUrls []
├── ViewCount
├── SoldAt
└── Notes
```

**Enums:**
- `VehicleStatus`: Available, Sold, Reserved

**Endpoints:**
- `POST /inventory/vehicles` - Add new vehicle
- `GET /inventory/vehicles` - List all vehicles with filtering
- `GET /inventory/vehicles/{id}` - Get vehicle details
- `PATCH /inventory/vehicles/{id}/price` - Update asking price
- `PATCH /inventory/vehicles/{id}/mileage` - Update mileage
- `POST /inventory/vehicles/{id}/photos` - Add vehicle photos
- `PATCH /inventory/vehicles/{id}/sold` - Mark vehicle as sold

**Events Published:**
- `VehicleSoldEvent` - Triggered when vehicle is marked as sold

**Database:**
- PostgreSQL with migrations in `leads` schema
- Supports multiple photo URLs per vehicle

---

### 3. **Leads Module**

Manages customer inquiries and test drive requests tied to vehicles.

**Key Features:**
- Create leads for general inquiries, test drives, or trade-ins
- Assign leads to staff members for follow-up
- Track lead status through the sales pipeline
- Manage follow-up interactions
- Auto-close leads when a vehicle is sold
- Support for trade-in vehicle information

**Key Entities:**
```csharp
Lead
├── Id
├── FirstName, LastName
├── Email, Phone
├── VehicleId
├── Type (GeneralInquiry | TestDrive | TradeIn)
├── Status (New | Contacted | Qualified | Closed)
├── Message
├── TradeInMake, TradeInModel, TradeInYear, TradeInMileage
├── AssignedToStaffId
├── ContactedAt
├── StaffNotes
└── FollowUps []

FollowUp
├── Id
├── LeadId
├── CreatedAt
├── Notes
└── UpdatedAt
```

**Enums:**
- `LeadType`: GeneralInquiry, TestDrive, TradeIn
- `LeadStatus`: New, Contacted, Qualified, Closed

**Endpoints:**
- `POST /leads` - Create new lead
- `GET /leads` - List all leads
- `GET /leads/{id}` - Get lead details
- `PATCH /leads/{id}/assign` - Assign to staff member
- `PATCH /leads/{id}/contacted` - Mark as contacted
- `POST /leads/{id}/follow-ups` - Add follow-up note
- `PATCH /leads/{id}/close` - Close lead

**Event Handlers:**
- `CloseLeadsOnVehicleSold` - Subscribes to `VehicleSoldEvent` and closes all open leads for that vehicle

**Database:**
- PostgreSQL with migrations in `leads` schema
- Maintains lead history through follow-ups

---


## 🎯 Domain Events

The example uses **in-process event dispatching** for inter-module communication (ready for RabbitMQ/Azure Service Bus upgrade).

### Published Events

- **VehicleSoldEvent** (Inventory Module)
  - Triggered: When a vehicle is marked as sold
  - Subscribers: Leads module closes all open leads for that vehicle
  - Payload: `VehicleId`, `SoldAt`, `SellingPrice`

### Event Flow Example

```
User marks Vehicle as sold
    ↓
InventoryModule publishes VehicleSoldEvent
    ↓
LeadsModule receives event via CloseLeadsOnVehicleSold handler
    ↓
All open leads for that vehicle are closed
```

## 🔄 Extending a new module

1. Follow module structure:
   ```
   Modules/NewFeature/
   ├── NewFeature.Core/
   ├── NewFeature.Application/
   └── NewFeature.Infrastructure/
   ```

2. Implement `IModule` interface in Infrastructure layer

3. Register in `Program.cs`:
   ```csharp
   modules.Add(new NewFeatureModule());
   ```

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 8 (.NET 8)
- **Database**: PostgreSQL with Entity Framework Core 9.0
- **Authentication**: JWT Bearer tokens
- **Validation**: FluentValidation 11.9.2
- **API Documentation**: Swagger/OpenAPI
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection

## 👤 Author

**João Garcia**  
GitHub: [@joonesgarcia](https://github.com/joonesgarcia)

---