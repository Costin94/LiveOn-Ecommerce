# LiveOn E-Commerce - Clean Architecture Project

## ?? Project Structure Overview

This solution follows **Clean Architecture** principles with clear separation of concerns:

```
LiveOn.Ecommerce/
??? src/
?   ??? LiveOn.Ecommerce.Domain          ? Core business logic (NO dependencies)
?   ??? LiveOn.Ecommerce.Application     ? Use cases (depends on Domain only)
?   ??? LiveOn.Ecommerce.Infrastructure  ? Data access (depends on Domain & Application)
?   ??? LiveOn.Ecommerce.API             ? Web API (depends on all layers)
??? tests/
    ??? LiveOn.Ecommerce.Domain.Tests
    ??? LiveOn.Ecommerce.Application.Tests
```

---

## ?? What's Been Set Up (Boilerplate)

### ? Domain Layer (`LiveOn.Ecommerce.Domain`)
**Purpose:** Contains core business logic, entities, value objects, and domain rules

**Created Files:**
- `Common/BaseEntity.cs` - Base class for all entities with Id, timestamps, soft delete
- `Common/ValueObject.cs` - Base class for value objects (objects without identity)
- `Interfaces/IRepository.cs` - Generic repository pattern interface
- `Interfaces/IUnitOfWork.cs` - Unit of Work pattern interface
- `Exceptions/DomainExceptions.cs` - Custom domain exceptions

**Key Concepts:**
- **BaseEntity**: Provides identity (Id), audit fields (CreatedAt, UpdatedAt), and soft delete capability
- **ValueObject**: Objects compared by their values, not identity (e.g., Money, Address)
- **Repository Pattern**: Abstracts data access, making the domain independent of infrastructure
- **Unit of Work**: Coordinates multiple repositories and manages transactions

---

### ? Application Layer (`LiveOn.Ecommerce.Application`)
**Purpose:** Contains business workflows, CQRS handlers, DTOs, and validators

**Folder Structure:**
- `Commands/` - Write operations (Create, Update, Delete)
- `Queries/` - Read operations (Get, List, Search)
- `DTOs/` - Data Transfer Objects for API communication
- `Mappings/` - AutoMapper profiles
- `Validators/` - FluentValidation validators
- `Behaviors/` - MediatR pipeline behaviors
- `Interfaces/` - Application-level interfaces

**Will Use:**
- MediatR (for CQRS pattern)
- AutoMapper (for object mapping)
- FluentValidation (for validation)

---

### ? Infrastructure Layer (`LiveOn.Ecommerce.Infrastructure`)
**Purpose:** Implements interfaces from Domain/Application, handles data persistence

**Folder Structure:**
- `Data/Context/` - EF DbContext
- `Data/Configurations/` - Fluent API configurations
- `Data/Migrations/` - EF migrations
- `Repositories/` - Repository implementations
- `Services/` - Infrastructure services (email, file storage, etc.)
- `Identity/` - Authentication/Authorization

**Will Use:**
- Entity Framework 6.x (ORM)
- SQL Server LocalDB
- Identity Framework (authentication)

---

### ? API Layer (`LiveOn.Ecommerce.API`)
**Purpose:** ASP.NET Web API - Entry point for HTTP requests

**Created Files:**
- `Global.asax` / `Global.asax.cs` - Application startup
- `App_Start/WebApiConfig.cs` - Web API routing configuration
- `Web.config` - Configuration file

**Folder Structure:**
- `Controllers/` - API controllers
- `Middleware/` - Custom middleware
- `Filters/` - Action filters, exception filters
- `Extensions/` - Extension methods for DI setup

**Will Use:**
- ASP.NET Web API 5.2.9
- Swagger (API documentation)
- Autofac (Dependency Injection)

---

## ?? Phase 1: What You'll Learn & Implement

### **Week 1: Domain Modeling & Design Patterns**

#### ????? **YOUR TASKS:**

1. **Create Domain Entities** (Learning: Rich Domain Models)
   - `Product` entity with business logic
   - `Category` entity
   - `Customer` entity
   - Implement validation rules inside entities

2. **Create Value Objects** (Learning: DDD Value Objects)
   - `Money` value object (amount + currency)
   - `Address` value object
   - `Email` value object with validation

3. **Define Enums** (Learning: Type safety)
   - `OrderStatus`
   - `PaymentMethod`
   - `ProductCategory`

4. **Understand Concepts:**
   - Why do entities have identity while value objects don't?
   - What makes a "rich" domain model vs "anemic" model?
   - How does BaseEntity.Equals() work for entity identity?

---

### **Week 2: Repository Pattern & Entity Framework**

#### ????? **YOUR TASKS:**

1. **Create Specific Repository Interfaces** (Learning: Interface Segregation)
   - `IProductRepository` (extends IRepository<Product>)
   - Add custom methods like `GetByCategory()`, `GetFeaturedProducts()`

2. **Implement EF DbContext** (Learning: EF Code-First)
   - Create `EcommerceDbContext`
   - Configure entity relationships using Fluent API
   - Set up connection string

3. **Implement Generic Repository** (Learning: Generic programming)
   - Create `Repository<T>` class in Infrastructure
   - Implement all IRepository methods using EF

4. **Implement Unit of Work** (Learning: Transaction management)
   - Create `UnitOfWork` class
   - Add repository properties
   - Implement Complete() method

5. **Create Fluent API Configurations** (Learning: EF Fluent API)
   - `ProductConfiguration`
   - Define column types, max lengths, relationships
   - Seed initial data

#### ?? **I'LL HANDLE:**
- NuGet package installation (EF, AutoMapper, MediatR)
- Creating empty folders
- Basic CRUD DTOs
- Mapping profiles (once entities are defined)

---

## ?? Next Steps

### **Ready to Start?**

**Step 1:** Let's begin with **Domain Entities**. I'll guide you through creating the `Product` entity with rich behavior.

**Questions to Think About:**
1. What properties should a Product have?
2. What business rules apply to a Product? (e.g., price must be positive)
3. Should Product have a reference to Category, or just CategoryId?
4. What behaviors might Product need? (e.g., `ApplyDiscount()`, `UpdateStock()`)

---

## ?? Resources & Learning Points

### **Design Patterns You'll Master:**
- ? Repository Pattern - Data access abstraction
- ? Unit of Work - Transaction coordination
- ? CQRS - Command Query Responsibility Segregation
- ? Mediator - Decoupling requests from handlers
- ? Specification - Reusable query logic

### **Best Practices:**
- ? Dependency Inversion (depend on abstractions, not implementations)
- ? Single Responsibility (each class has one reason to change)
- ? Domain-Driven Design basics
- ? Clean Code principles
- ? SOLID principles in practice

---

## ??? Development Workflow

1. **For each feature:**
   - Define domain entities/value objects
   - Create repository interfaces if needed
   - Implement repository in Infrastructure
   - Create CQRS commands/queries in Application
   - Create API controller endpoints
   - Test the endpoint

2. **Always:**
   - Keep the Domain layer independent
   - Use interfaces for dependencies
   - Follow async/await best practices
   - Write clean, self-documenting code

---

**?? Ready to dive in? Let me know when you want to start with Step 1: Creating Domain Entities!**
