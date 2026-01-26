# Refactoring: Eliminating Code Duplication with Mappers

## ?? The Problem You Identified

**Great catch!** You noticed that the `MapToDto` method was duplicated across multiple handlers:

```csharp
// ? BEFORE: Duplicated in 3 different files
// GetProductByIdQueryHandler.cs
private ProductDto MapToDto(Product product) { /* mapping logic */ }

// GetAllProductsQueryHandler.cs
private ProductDto MapToDto(Product product) { /* same logic */ }

// GetProductBySkuQueryHandler.cs
private ProductDto MapToDto(Product product) { /* same logic again */ }
```

### Problems with This Approach:
- ? **Violates DRY principle** (Don't Repeat Yourself)
- ? **Maintenance nightmare** - Change DTO? Update 3 places!
- ? **Risk of inconsistency** - Easy to forget one place
- ? **More code to test**
- ? **Harder to read** - Same logic repeated

---

## ? The Solution: Centralized Mappers

We created **Extension Method Mappers** to centralize the mapping logic.

### What Are Extension Methods?
Extension methods let you "add" methods to existing types without modifying them.

```csharp
// Instead of:
MapToDto(product)

// You can write:
product.ToDto()  // Looks like Product has a ToDto method!
```

---

## ?? New Structure

Created two mapper files:

### 1. ProductMapper.cs
```csharp
namespace LiveOn.Ecommerce.Application.Mappers
{
    public static class ProductMapper
    {
        // Extension method - note the "this" keyword
        public static ProductDto ToDto(this Product product)
        {
            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                // ... all mapping logic in ONE place
            };
        }
    }
}
```

**Key Points:**
- `static class` - Required for extension methods
- `static method` - Extension methods must be static
- `this Product product` - The "this" makes it an extension method

### 2. CategoryMapper.cs
Same pattern for Category ? CategoryDto mapping.

---

## ?? How We Refactored

### Before (Duplicated)
```csharp
// GetProductByIdQueryHandler.cs
public ProductDto Handle(GetProductByIdQuery query)
{
    var product = _unitOfWork.Product.GetById(query.Id);
    if (product == null)
        return null;
    
    return MapToDto(product);  // Private method in this file
}

private ProductDto MapToDto(Product product)
{
    return new ProductDto { /* 15 lines of mapping */ };
}
```

### After (Centralized)
```csharp
// GetProductByIdQueryHandler.cs
using LiveOn.Ecommerce.Application.Mappers;  // ? Import mapper

public ProductDto Handle(GetProductByIdQuery query)
{
    var product = _unitOfWork.Product.GetById(query.Id);
    return product?.ToDto();  // ? Extension method from mapper
}

// No more private MapToDto method! ??
```

**Benefits:**
- ? **Cleaner** - Handler is now 40% shorter
- ? **Null-safe** - `product?.ToDto()` returns null if product is null
- ? **Single source of truth** - One place to maintain mapping logic
- ? **Reusable** - Any class can use `.ToDto()` after adding the using statement

---

## ?? Impact Summary

### Files Changed:
1. **Created:**
   - `ProductMapper.cs` - Centralized Product?ProductDto mapping
   - `CategoryMapper.cs` - Centralized Category?CategoryDto mapping

2. **Refactored:**
   - `GetProductByIdQueryHandler.cs` - Removed 15 lines of duplicate code
   - `GetAllProductsQueryHandler.cs` - Removed 15 lines of duplicate code
   - `GetProductBySkuQueryHandler.cs` - Removed 15 lines of duplicate code
   - `GetCategoryByIdQueryHandler.cs` - Now uses mapper
   - `GetAllCategoriesQueryHandler.cs` - Removed duplicate code

### Code Reduction:
- **Removed ~75 lines** of duplicated mapping code
- **Added ~35 lines** in centralized mappers
- **Net result**: ~40 lines less code + better maintainability

---

## ?? Interview Talking Points

When discussing this refactoring:

1. **Identified the problem:**
   > "I noticed the MapToDto method was duplicated across multiple query handlers, violating the DRY principle."

2. **Solution approach:**
   > "I created centralized mapper classes using extension methods, which provided a fluent API and single source of truth for mapping logic."

3. **Benefits achieved:**
   > "This reduced code duplication, improved maintainability, and made the handlers cleaner and easier to test."

4. **Alternative approaches:**
   > "In production, I'd consider AutoMapper for more complex scenarios, but custom mappers give more control and better performance."

---

## ?? Extension Methods Deep Dive

### Syntax Breakdown
```csharp
public static ProductDto ToDto(this Product product)
//     ?       ?           ?    ?       ?
//  static  return      method this   parameter
//          type        name   keyword  type
```

### How They Work
```csharp
// When you write:
product.ToDto()

// The compiler translates it to:
ProductMapper.ToDto(product)

// So these are equivalent:
var dto1 = product.ToDto();
var dto2 = ProductMapper.ToDto(product);
```

### Usage in LINQ
```csharp
// Clean and readable
products.Select(p => p.ToDto()).ToList()

// Without extension method (ugly):
products.Select(p => ProductMapper.ToDto(p)).ToList()
```

---

## ?? Alternative Approaches

### 1. **Static Mapper Class (Without Extension Methods)**
```csharp
public static class ProductMapper
{
    public static ProductDto ToDto(Product product)  // No "this"
    {
        // ... mapping
    }
}

// Usage:
var dto = ProductMapper.ToDto(product);  // Less fluent
```

### 2. **AutoMapper Library** (Industry Standard)
```csharp
// Configuration once:
CreateMap<Product, ProductDto>();

// Usage everywhere:
var dto = _mapper.Map<ProductDto>(product);
```

**AutoMapper Pros:**
- ? Handles complex scenarios automatically
- ? Convention-based (auto-maps matching property names)
- ? Widely used in industry

**AutoMapper Cons:**
- ? Extra dependency
- ? Less control
- ? Harder to debug
- ? Performance overhead for simple mappings

### 3. **Mapper Service (Dependency Injection)**
```csharp
public interface IProductMapper
{
    ProductDto ToDto(Product product);
}

public class ProductMapper : IProductMapper
{
    public ProductDto ToDto(Product product) { /* ... */ }
}

// Inject in handlers:
public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IProductMapper mapper)
```

---

## ?? Best Practices

### ? DO:
- Keep mappers simple and focused
- Handle null checks in mappers
- Put mappers in dedicated `Mappers` folder
- Use extension methods for fluent syntax
- Test mappers independently

### ? DON'T:
- Put business logic in mappers (only map data)
- Make mappers depend on repositories or services
- Create circular dependencies (Entity?DTO?Entity)
- Map sensitive data (passwords, etc.) without filtering

---

## ?? What's Next?

Now that you have centralized mappers, you can:

1. **Add more mappers** - UserMapper for User?UserDto
2. **Reverse mapping** - Add `ToEntity` methods if needed
3. **Consider AutoMapper** - For more complex scenarios
4. **Profile-based DTOs** - Different DTOs for different contexts
   - `ProductSummaryDto` - Minimal info for lists
   - `ProductDetailDto` - Full info for details page

---

## ?? Summary

### What We Did:
1. ? Identified code duplication (great observation!)
2. ? Created centralized mapper classes
3. ? Used extension methods for fluent API
4. ? Refactored all handlers to use mappers
5. ? Removed ~40 lines of duplicate code
6. ? Improved maintainability

### Key Takeaway:
> **Always look for patterns of duplication and consolidate them into reusable components. This is a hallmark of senior-level code quality.**

### For Interviews:
You can now discuss:
- ? DRY principle
- ? Extension methods
- ? Code refactoring
- ? Mapper pattern
- ? AutoMapper (awareness)

**Excellent work identifying this issue!** ??
