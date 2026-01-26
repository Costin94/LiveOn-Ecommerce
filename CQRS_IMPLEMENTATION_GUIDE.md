# CQRS Pattern Implementation Guide

## ?? What is CQRS?

**CQRS (Command Query Responsibility Segregation)** is a design pattern that separates read and write operations into different models.

### Core Principle
- **Commands**: Write operations that modify state (Create, Update, Delete)
- **Queries**: Read operations that retrieve data without modification

### Benefits
? **Separation of Concerns** - Read and write logic are isolated  
? **Scalability** - Scale reads and writes independently  
? **Performance** - Optimize queries separately from commands  
? **Security** - Better control over data modification  
? **Interview Ready** - Shows architectural understanding  

---

## ??? Architecture Overview

```
Application Layer (CQRS)
??? DTOs/                          # Data Transfer Objects
?   ??? ProductDto.cs
?   ??? CategoryDto.cs
?   ??? UserDto.cs
?
??? Interfaces/                    # Core CQRS interfaces
?   ??? ICommand<TResult>         # Marker for commands
?   ??? IQuery<TResult>           # Marker for queries
?   ??? ICommandHandler<T, R>     # Handles commands
?   ??? IQueryHandler<T, R>       # Handles queries
?
??? Commands/                      # Write operations
?   ??? Products/
?   ?   ??? CreateProductCommand.cs
?   ?   ??? UpdateProductCommand.cs
?   ?   ??? DeleteProductCommand.cs
?   ?   ??? UpdateProductStockCommand.cs
?   ??? Categories/...
?
??? Queries/                       # Read operations
?   ??? Products/
?   ?   ??? GetProductByIdQuery.cs
?   ?   ??? GetAllProductsQuery.cs
?   ?   ??? GetProductBySkuQuery.cs
?   ??? Categories/...
?
??? Handlers/                      # Business logic
    ??? CommandHandlers/
    ?   ??? Products/
    ?       ??? CreateProductCommandHandler.cs
    ?       ??? UpdateProductCommandHandler.cs
    ?       ??? DeleteProductCommandHandler.cs
    ?       ??? UpdateProductStockCommandHandler.cs
    ??? QueryHandlers/
        ??? Products/
            ??? GetProductByIdQueryHandler.cs
            ??? GetAllProductsQueryHandler.cs
            ??? GetProductBySkuQueryHandler.cs
```

---

## ?? Key Components Explained

### 1. **Commands** (Write Operations)
Commands represent an **intent to change state**.

```csharp
// Example: CreateProductCommand
public class CreateProductCommand : ICommand<int>
{
    public string Name { get; set; }
    public string SKU { get; set; }
    public decimal Price { get; set; }
    // Returns: Product ID (int)
}
```

**Characteristics:**
- Contains data needed for the operation
- Implements `ICommand<TResult>` where TResult is the return type
- Should be immutable (all properties with get/set for now, but consider init in newer .NET)
- Named with verb + noun (CreateProduct, UpdateCategory)

### 2. **Queries** (Read Operations)
Queries represent a **request for data**.

```csharp
// Example: GetProductByIdQuery
public class GetProductByIdQuery : IQuery<ProductDto>
{
    public int Id { get; set; }
    // Returns: ProductDto
}
```

**Characteristics:**
- Contains parameters for filtering/searching
- Implements `IQuery<TResult>` where TResult is the data returned
- Returns DTOs, never domain entities
- Named with Get + description (GetProductById, GetAllProducts)

### 3. **Command Handlers**
Command handlers contain the **business logic for write operations**.

```csharp
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public int Handle(CreateProductCommand command)
    {
        // 1. Create domain entity
        var product = new Product(
            name: command.Name,
            sku: command.SKU,
            price: command.Price,
            categoryId: command.CategoryId,
            description: command.Description
        );

        // 2. Apply business rules
        if (command.InitialStock > 0)
            product.IncreaseStock(command.InitialStock);

        // 3. Persist changes
        _unitOfWork.Product.Add(product);
        _unitOfWork.Complete();

        // 4. Return result
        return product.Id;
    }

    public async Task<int> HandleAsync(CreateProductCommand command)
    {
        // Same logic but async
        // ...
    }
}
```

**Responsibilities:**
- Validate business rules
- Create/modify domain entities
- Use Unit of Work to persist changes
- Return success/failure result

### 4. **Query Handlers**
Query handlers contain the **logic for retrieving data**.

```csharp
public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ProductDto Handle(GetProductByIdQuery query)
    {
        // 1. Fetch from repository
        var product = _unitOfWork.Product.GetById(query.Id);
        if (product == null)
            return null;

        // 2. Map to DTO
        return MapToDto(product);
    }

    private ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            SKU = product.SKU,
            Price = product.Price,
            // ... map all properties
        };
    }
}
```

**Responsibilities:**
- Fetch data from repositories
- Apply filters/search criteria
- Map domain entities to DTOs
- Never modify data

### 5. **DTOs (Data Transfer Objects)**
DTOs are simple data containers for transferring data between layers.

```csharp
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SKU { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public ProductStatus Status { get; set; }
    // ... no business logic, just data
}
```

**Why DTOs?**
- **Decouple** API layer from domain entities
- **Security** - Don't expose internal domain structure
- **Performance** - Return only what's needed
- **Versioning** - API can change without affecting domain

---

## ?? How to Use CQRS

### In Your API Controller (Next Step)

```csharp
public class ProductsController : ApiController
{
    private readonly ICommandHandler<CreateProductCommand, int> _createHandler;
    private readonly IQueryHandler<GetProductByIdQuery, ProductDto> _getByIdHandler;

    // Constructor injection
    public ProductsController(
        ICommandHandler<CreateProductCommand, int> createHandler,
        IQueryHandler<GetProductByIdQuery, ProductDto> getByIdHandler)
    {
        _createHandler = createHandler;
        _getByIdHandler = getByIdHandler;
    }

    // POST api/products
    [HttpPost]
    public async Task<IHttpActionResult> CreateProduct(CreateProductCommand command)
    {
        var productId = await _createHandler.HandleAsync(command);
        return Ok(new { Id = productId });
    }

    // GET api/products/5
    [HttpGet]
    public async Task<IHttpActionResult> GetProduct(int id)
    {
        var query = new GetProductByIdQuery(id);
        var product = await _getByIdHandler.HandleAsync(query);
        
        if (product == null)
            return NotFound();
            
        return Ok(product);
    }
}
```

---

## ?? What We Built

### ? Products - Complete CQRS Implementation
- **Commands**: Create, Update, Delete, UpdateStock
- **Queries**: GetById, GetAll (with filtering), GetBySku
- **Handlers**: Full implementation with both sync and async

### ? Categories - Partial Implementation
- **Commands**: Create, Update, Delete
- **Queries**: GetById, GetAll
- **Handlers**: You should implement these following the Product pattern

---

## ?? Next Steps for You

### 1. **Implement Category Handlers**
Create handlers for categories following the Product pattern:
- `CreateCategoryCommandHandler`
- `UpdateCategoryCommandHandler`
- `DeleteCategoryCommandHandler`
- `GetCategoryByIdQueryHandler`
- `GetAllCategoriesQueryHandler`

### 2. **Add User Commands/Queries**
Implement CQRS for Users:
- `RegisterUserCommand`
- `UpdateUserCommand`
- `GetUserByIdQuery`
- `GetUserByEmailQuery`

### 3. **Dependency Injection (DI)**
Next major step - configure DI in your API project:
- Register all handlers
- Register Unit of Work
- Register repositories

### 4. **API Controllers**
Build RESTful endpoints using your handlers.

### 5. **Validation**
Add FluentValidation to validate commands before handling.

### 6. **Error Handling**
Implement Result pattern or exceptions handling.

---

## ?? Interview Talking Points

When discussing CQRS in interviews:

1. **What is it?**
   - "CQRS separates read and write operations into different models"

2. **Why use it?**
   - "Better separation of concerns, independent scaling, optimized queries"

3. **When to use it?**
   - "Complex domains, different read/write patterns, high scalability needs"

4. **Trade-offs?**
   - "More code, complexity, eventual consistency (if using separate databases)"

5. **Your experience:**
   - "I implemented CQRS in my e-commerce project, separating Product commands and queries with dedicated handlers and DTOs"

---

## ?? Code Quality Tips

### ? DO:
- Keep commands/queries simple (just data)
- Put business logic in handlers
- Return DTOs from queries (never entities)
- Use async methods for I/O operations
- Validate in handlers before processing

### ? DON'T:
- Put business logic in commands/queries
- Return domain entities from queries
- Mix command and query responsibilities
- Make handlers dependent on each other
- Skip validation

---

## ?? Further Learning

### Optional Enhancements:
1. **MediatR Library** - Simplifies CQRS with a mediator pattern
2. **FluentValidation** - Declarative validation for commands
3. **Result Pattern** - Better error handling
4. **Specification Pattern** - Complex query building
5. **Event Sourcing** - Advanced CQRS with events

---

## Summary

You now have:
- ? Complete CQRS structure
- ? Commands for write operations
- ? Queries for read operations
- ? Handlers with business logic
- ? DTOs for data transfer
- ? Product implementation (complete example)
- ? Category structure (for practice)

**Next: Implement Category handlers, then move to Dependency Injection and API Controllers!**
