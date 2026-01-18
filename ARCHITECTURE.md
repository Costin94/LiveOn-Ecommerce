# Phase 1 Architecture Guide

## ??? Clean Architecture Layers

```
???????????????????????????????????????????????????????????????
?                       API Layer                              ?
?  (Controllers, Middleware, Filters, DI Configuration)        ?
?  Dependencies: All other layers                              ?
???????????????????????????????????????????????????????????????
                            ?
???????????????????????????????????????????????????????????????
?                  Infrastructure Layer                        ?
?  (EF DbContext, Repositories, External Services)             ?
?  Dependencies: Domain + Application                          ?
???????????????????????????????????????????????????????????????
                            ?
???????????????????????????????????????????????????????????????
?                  Application Layer                           ?
?  (CQRS Handlers, DTOs, Validators, Business Workflows)       ?
?  Dependencies: Domain only                                   ?
???????????????????????????????????????????????????????????????
                            ?
???????????????????????????????????????????????????????????????
?                    Domain Layer                              ?
?  (Entities, Value Objects, Domain Logic, Interfaces)         ?
?  Dependencies: NONE (Pure business logic)                    ?
???????????????????????????????????????????????????????????????
```

## ?? Dependency Flow Rule

**The Golden Rule:** Dependencies point INWARD only!

- ? API can reference Infrastructure, Application, Domain
- ? Infrastructure can reference Application, Domain
- ? Application can reference Domain only
- ? Domain references NO other layers
- ? Lower layers NEVER reference upper layers

---

## ?? Layer Responsibilities

### Domain Layer (Core Business Logic)
**Contains:**
- Entities (Product, Order, Customer)
- Value Objects (Money, Address, Email)
- Enums (OrderStatus, PaymentMethod)
- Domain Interfaces (IRepository, IUnitOfWork)
- Domain Exceptions
- Business Rules & Validation

**Example:** A Product entity knows if its price is valid, if it's in stock, and how to apply a discount.

**Dependencies:** NONE - completely isolated

---

### Application Layer (Use Cases)
**Contains:**
- Commands (CreateProductCommand, PlaceOrderCommand)
- Command Handlers (CreateProductHandler)
- Queries (GetProductByIdQuery, GetAllProductsQuery)
- Query Handlers (GetProductByIdHandler)
- DTOs (ProductDto, OrderDto)
- Validators (CreateProductValidator)
- Mappings (AutoMapper profiles)
- Application Interfaces (IEmailService, IFileStorage)

**Example:** CreateProductCommand knows HOW to create a product (workflow), but doesn't know WHERE data is stored.

**Dependencies:** Domain only

---

### Infrastructure Layer (Implementation Details)
**Contains:**
- DbContext & EF Configurations
- Repository Implementations
- Service Implementations (EmailService, BlobStorage)
- Identity/Authentication setup
- Database Migrations
- External API integrations

**Example:** ProductRepository implements IProductRepository and knows how to save to SQL Server using EF.

**Dependencies:** Domain + Application

---

### API Layer (Entry Point)
**Contains:**
- Controllers (ProductsController, OrdersController)
- Middleware (Exception handling, logging)
- Filters (Authorization, validation)
- DI Container setup
- API versioning & routing
- Swagger configuration

**Example:** ProductsController receives HTTP requests and delegates to MediatR handlers.

**Dependencies:** All layers (composition root)

---

## ?? Key Patterns Explained

### 1. Repository Pattern
**Why?** Abstracts data access, makes domain testable without database

```csharp
// Domain defines interface
public interface IProductRepository : IRepository<Product>
{
    Task<Product> GetBySkuAsync(string sku);
}

// Infrastructure implements it
public class ProductRepository : Repository<Product>, IProductRepository
{
    public async Task<Product> GetBySkuAsync(string sku)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Sku == sku);
    }
}

// Application uses the interface
public class CreateProductHandler
{
    private readonly IProductRepository _productRepo;
    // Domain doesn't care about EF, SQL, or any database!
}
```

---

### 2. Unit of Work Pattern
**Why?** Manages transactions across multiple repositories

```csharp
public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    Task<int> CompleteAsync(); // Saves all changes atomically
}

// Usage in a handler:
public async Task Handle(PlaceOrderCommand request)
{
    var order = new Order(...);
    _unitOfWork.Orders.Add(order);
    
    // Update stock for all products
    foreach (var item in order.Items)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
        product.DecreaseStock(item.Quantity);
    }
    
    // Save everything together or rollback if anything fails
    await _unitOfWork.CompleteAsync();
}
```

---

### 3. CQRS (Command Query Responsibility Segregation)
**Why?** Separates reads from writes, optimizes each independently

```csharp
// Commands (Write operations)
public class CreateProductCommand : IRequest<int>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
{
    public async Task<int> Handle(CreateProductCommand request)
    {
        // Business logic, validation, persistence
    }
}

// Queries (Read operations)
public class GetProductByIdQuery : IRequest<ProductDto>
{
    public int Id { get; set; }
}

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery request)
    {
        // Just fetch and return data
    }
}
```

---

## ?? Week 1-2 Learning Roadmap

### Day 1-2: Domain Entities
- [ ] Create Product entity with rich behavior
- [ ] Create Category entity
- [ ] Create Customer entity
- [ ] Understand entity identity and equality

### Day 3-4: Value Objects
- [ ] Create Money value object
- [ ] Create Address value object
- [ ] Create Email value object with validation
- [ ] Understand value object immutability

### Day 5-6: Enums & Relationships
- [ ] Define ProductStatus, OrderStatus enums
- [ ] Set up entity relationships (Product ? Category)
- [ ] Understand aggregates and aggregate roots

### Day 7-8: Repository Interfaces
- [ ] Create IProductRepository
- [ ] Create IOrderRepository
- [ ] Understand interface segregation principle

### Day 9-10: Entity Framework Setup
- [ ] Create EcommerceDbContext
- [ ] Configure entities with Fluent API
- [ ] Create first migration
- [ ] Understand Code-First approach

### Day 11-12: Repository Implementation
- [ ] Implement generic Repository<T>
- [ ] Implement specific repositories
- [ ] Implement Unit of Work
- [ ] Test with real database

### Day 13-14: First CQRS Handler
- [ ] Create CreateProductCommand
- [ ] Create CreateProductHandler
- [ ] Create GetProductsQuery
- [ ] Create GetProductsHandler
- [ ] Understand MediatR pipeline

---

## ?? Pro Tips

### Tip 1: Keep Domain Pure
```csharp
// ? BAD - Domain depends on EF
public class Product : BaseEntity
{
    [Required] // EF annotation
    [MaxLength(100)] // EF annotation
    public string Name { get; set; }
}

// ? GOOD - Domain is independent
public class Product : BaseEntity
{
    private string _name;
    
    public string Name 
    { 
        get => _name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidEntityException("Product name is required");
            if (value.Length > 100)
                throw new InvalidEntityException("Product name too long");
            _name = value;
        }
    }
}

// EF configuration in Infrastructure
public class ProductConfiguration : EntityTypeConfiguration<Product>
{
    public ProductConfiguration()
    {
        Property(p => p.Name).IsRequired().HasMaxLength(100);
    }
}
```

### Tip 2: Use Private Setters
```csharp
// Encapsulation - control HOW properties change
public class Product : BaseEntity
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new BusinessRuleViolationException("Price cannot be negative");
        
        Price = newPrice;
        MarkAsUpdated();
    }
}
```

### Tip 3: Async All The Way
```csharp
// Use async/await consistently from API to database
public async Task<ProductDto> Handle(GetProductByIdQuery request)
{
    var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
    if (product == null)
        throw new EntityNotFoundException(nameof(Product), request.Id);
    
    return _mapper.Map<ProductDto>(product);
}
```

---

## ?? You're Ready!

The foundation is set. Now it's time to build your first feature!

**Next Step:** Create your first Domain Entity - the Product class.

Think about:
- What properties does a product need?
- What business rules apply?
- What behaviors should it have?

Let's code! ??
