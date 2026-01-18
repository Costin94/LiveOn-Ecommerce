# ?? Domain Entities Created!

## ? What's Been Built

I've created three core domain entities with **rich business logic** following Clean Architecture and Domain-Driven Design principles.

---

## ?? Entities Created

### 1. **Product Entity** (`Entities/Product.cs`)
**Aggregate Root** - Represents products in your e-commerce system

#### Key Features:
- ? **Rich Domain Model** - Business logic encapsulated in the entity
- ? **Private Setters** - Properties can only be changed through business methods
- ? **Validation** - All business rules enforced at the domain level
- ? **Computed Properties** - `FinalPrice`, `IsInStock`, `IsAvailable`

#### Properties:
```csharp
- Name (required, max 200 chars)
- SKU (required, unique, max 50 chars, auto-uppercase)
- Description (optional, max 2000 chars)
- Price (must be > 0 and < 1,000,000)
- StockQuantity (cannot be negative)
- CategoryId (foreign key)
- Status (Draft, Active, OutOfStock, Discontinued, Archived)
- ImageUrl (optional, max 500 chars)
- Weight (optional, for shipping)
- IsFeatured (homepage display)
- DiscountPercentage (0-100)
```

#### Business Methods (Examples of Rich Behavior):
```csharp
// Stock Management
product.IncreaseStock(100);
product.DecreaseStock(5);  // Throws if insufficient stock
product.SetStock(50);      // Direct inventory update

// Pricing
product.SetPrice(29.99m);  // Validates price > 0
product.ApplyDiscount(15); // 15% off
product.RemoveDiscount();

// Status Management
product.Activate();        // Makes product available
product.Discontinue();     // Marks as discontinued

// Validation
bool canOrder = product.CanBeOrdered(quantity: 3);
decimal finalPrice = product.FinalPrice; // After discount
```

#### Smart Behavior:
- Automatically changes status to `OutOfStock` when stock reaches 0
- Automatically changes status to `Active` when stock is added
- Calculates final price with discount
- Prevents negative stock
- Validates price range

---

### 2. **Category Entity** (`Entities/Category.cs`)
**Aggregate Root** - Represents product categories with hierarchical support

#### Key Features:
- ? **Hierarchical Structure** - Support for parent/child categories
- ? **URL-Friendly Slugs** - Auto-generated from name
- ? **Display Order** - For sorting categories
- ? **Active/Inactive State** - Control visibility

#### Properties:
```csharp
- Name (required, max 100 chars)
- Description (optional, max 500 chars)
- Slug (auto-generated URL-friendly)
- ParentCategoryId (nullable, for sub-categories)
- IsActive (visibility control)
- DisplayOrder (for sorting)
- Products (navigation property)
- SubCategories (navigation property)
```

#### Business Methods:
```csharp
// Name & Description
category.SetName("Electronics");
category.SetDescription("All electronic items");

// Hierarchy
category.SetParentCategory(parentId: 5);

// Visibility
category.Activate();
category.Deactivate();

// Sorting
category.SetDisplayOrder(10);
```

#### Smart Behavior:
- Auto-generates URL slug: "Gaming Laptops" ? "gaming-laptops"
- Prevents circular references (category can't be its own parent)
- Validates all inputs

---

### 3. **User Entity** (`Entities/User.cs`)
**Aggregate Root** - Represents customers/users in the system

#### Key Features:
- ? **Role-Based Access** - Customer, Manager, Administrator
- ? **Account Security** - Failed login tracking, account lockout
- ? **Email Verification** - Track email verification status
- ? **Audit Trail** - Registration date, last login

#### Properties:
```csharp
- Email (unique, required, validated, max 256 chars)
- FirstName (required, max 50 chars)
- LastName (required, max 50 chars)
- FullName (computed: FirstName + LastName)
- PhoneNumber (optional, max 20 chars)
- PasswordHash (hashed password, never plain text!)
- Role (Customer, Manager, Administrator)
- IsActive (account status)
- IsEmailVerified (email verification)
- RegistrationDate (when user signed up)
- LastLoginDate (last successful login)
- FailedLoginAttempts (for security)
- LockedUntil (account lockout)
- IsLocked (computed: currently locked?)
```

#### Business Methods:
```csharp
// Profile Management
user.SetEmail("john@example.com");
user.SetFirstName("John");
user.SetLastName("Doe");
user.SetPhoneNumber("+1234567890");

// Password Management
user.SetPasswordHash(hashedPassword);

// Role Management
user.ChangeRole(UserRole.Manager);

// Account Status
user.Activate();
user.Deactivate();
user.VerifyEmail();

// Security
user.RecordSuccessfulLogin();
user.RecordFailedLogin(); // Auto-locks after 5 attempts
user.Unlock();

// Authorization Checks
bool canLogin = user.CanLogin();
bool isAdmin = user.IsAdministrator();
bool hasAccess = user.IsManagerOrHigher();
```

#### Smart Behavior:
- Validates email format using `System.Net.Mail.MailAddress`
- Auto-locks account after 5 failed login attempts for 15 minutes
- Resets email verification when email changes
- Prevents setting role to `Guest`
- Full name computed automatically

---

## ?? Enums Created

### `ProductStatus` (`Enums/ProductStatus.cs`)
```csharp
- Draft         // Not visible to customers
- Active        // Available for purchase
- OutOfStock    // Temporarily unavailable
- Discontinued  // Permanently unavailable
- Archived      // Soft deleted
```

### `UserRole` (`Enums/UserRole.cs`)
```csharp
- Guest         // Not authenticated
- Customer      // Regular user
- Manager       // Limited admin
- Administrator // Full access
```

---

## ?? Clean Architecture Principles Applied

### 1. **Rich Domain Models** (NOT Anemic!)
```csharp
// ? ANEMIC MODEL (Bad)
public class Product
{
    public decimal Price { get; set; } // Anyone can set invalid price!
}

// ? RICH MODEL (Good) - What we built
public class Product
{
    public decimal Price { get; private set; }
    
    public void SetPrice(decimal price)
    {
        if (price <= 0)
            throw new BusinessRuleViolationException("Price must be positive");
        _price = price;
        MarkAsUpdated();
    }
}
```

### 2. **Encapsulation**
- All properties have **private setters**
- State changes through **business methods**
- Validation **enforced** at domain level
- Cannot create invalid entities

### 3. **Domain Exceptions**
- Uses custom exceptions: `InvalidEntityException`, `BusinessRuleViolationException`
- Clear error messages
- Fail-fast principle

### 4. **No External Dependencies**
- No EF attributes in domain (clean!)
- No UI concerns
- No infrastructure dependencies
- Pure business logic

### 5. **Aggregate Roots**
- Each entity is an Aggregate Root
- Controls its own consistency
- Enforces invariants

---

## ?? Example Usage

### Creating a Product:
```csharp
// Create product
var product = new Product(
    name: "Gaming Laptop",
    sku: "LAPTOP-001",
    price: 1299.99m,
    categoryId: 5,
    description: "High-performance gaming laptop"
);

// Add stock
product.IncreaseStock(50);

// Apply discount
product.ApplyDiscount(10); // 10% off

// Activate
product.Activate();

// Customer wants to buy 2
if (product.CanBeOrdered(2))
{
    product.DecreaseStock(2);
    var price = product.FinalPrice; // $1169.99 (after 10% discount)
}
```

### Creating a User:
```csharp
// Register new user
var user = new User(
    email: "john@example.com",
    firstName: "John",
    lastName: "Doe",
    passwordHash: hashedPassword,
    role: UserRole.Customer
);

// Later: User logs in successfully
user.RecordSuccessfulLogin();

// Or: Failed login attempt
user.RecordFailedLogin(); // After 5 attempts, account locks for 15 min

// Check if can login
if (user.CanLogin())
{
    // Allow login
}
```

### Creating Categories:
```csharp
// Create parent category
var electronics = new Category(
    name: "Electronics",
    description: "Electronic devices and accessories"
);

// Create sub-category
var laptops = new Category(
    name: "Laptops",
    description: "Laptop computers",
    parentCategoryId: electronics.Id
);

// Organize
laptops.SetDisplayOrder(1);
laptops.Activate();
```

---

## ?? What You Can Learn From This

### Key Patterns & Concepts:

1. **Private Constructors for EF**
```csharp
private Product() { } // EF needs parameterless constructor
public Product(params...) : this() { } // Public constructor for code
```

2. **Property Backing Fields**
```csharp
private string _name;
public string Name
{
    get => _name;
    private set => _name = value;
}
```

3. **Validation in Setters**
```csharp
public void SetPrice(decimal price)
{
    if (price <= 0)
        throw new BusinessRuleViolationException("Price must be positive");
    _price = price;
    MarkAsUpdated(); // From BaseEntity
}
```

4. **Computed Properties**
```csharp
public decimal FinalPrice => Price - (Price * DiscountPercentage / 100m);
public string FullName => $"{FirstName} {LastName}";
public bool IsInStock => StockQuantity > 0;
```

5. **Smart Business Logic**
```csharp
public void DecreaseStock(int quantity)
{
    // Validation
    if (_stockQuantity < quantity)
        throw new BusinessRuleViolationException("Insufficient stock");
    
    // Update state
    _stockQuantity -= quantity;
    
    // Auto-update related state
    if (_stockQuantity == 0 && Status == ProductStatus.Active)
        Status = ProductStatus.OutOfStock;
    
    // Audit
    MarkAsUpdated();
}
```

---

## ? Build Status

**All entities build successfully!** ?

Your domain layer is now complete with three production-ready entities.

---

## ?? Next Steps

Now that we have domain entities, you're ready for **Week 2: Repository Pattern & Entity Framework!**

### What's Next:
1. **Set up Entity Framework 6** in Infrastructure layer
2. **Create DbContext** with Fluent API configurations
3. **Implement Repository pattern** (generic + specific)
4. **Create database migrations**
5. **Seed initial data**

**Ready to move to Week 2?** Let me know! ??
