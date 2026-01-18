using System;
using LiveOn.Ecommerce.Domain.Common;
using LiveOn.Ecommerce.Domain.Enums;
using LiveOn.Ecommerce.Domain.Exceptions;

namespace LiveOn.Ecommerce.Domain.Entities
{
    /// <summary>
    /// Represents a product in the e-commerce system
    /// This is an Aggregate Root with rich business logic
    /// </summary>
    public class Product : BaseEntity
    {
        private string _name;
        private string _sku;
        private string _description;
        private decimal _price;
        private int _stockQuantity;
        private string _imageUrl;

        // Private constructor for EF
        private Product()
        {
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        public Product(string name, string sku, decimal price, int categoryId, string description = null)
        {
            SetName(name);
            SetSku(sku);
            SetPrice(price);
            CategoryId = categoryId;
            SetDescription(description);
            _stockQuantity = 0;
            Status = ProductStatus.Draft;
        }

        /// <summary>
        /// Product name (required, max 200 characters)
        /// </summary>
        public string Name
        {
            get => _name;
            private set => _name = value;
        }

        /// <summary>
        /// Stock Keeping Unit - unique identifier (required, max 50 characters)
        /// </summary>
        public string SKU
        {
            get => _sku;
            private set => _sku = value;
        }

        /// <summary>
        /// Product description (optional, max 2000 characters)
        /// </summary>
        public string Description
        {
            get => _description;
            private set => _description = value;
        }

        /// <summary>
        /// Product price (must be positive)
        /// </summary>
        public decimal Price
        {
            get => _price;
            private set => _price = value;
        }

        /// <summary>
        /// Available stock quantity (cannot be negative)
        /// </summary>
        public int StockQuantity
        {
            get => _stockQuantity;
            private set => _stockQuantity = value;
        }

        /// <summary>
        /// Category ID (foreign key)
        /// </summary>
        public int CategoryId { get; private set; }

        /// <summary>
        /// Navigation property to Category
        /// </summary>
        public virtual Category Category { get; private set; }

        /// <summary>
        /// Product status (Draft, Active, OutOfStock, Discontinued, Archived)
        /// </summary>
        public ProductStatus Status { get; private set; }

        /// <summary>
        /// Product image URL (optional)
        /// </summary>
        public string ImageUrl
        {
            get => _imageUrl;
            private set => _imageUrl = value;
        }

        /// <summary>
        /// Weight in kilograms (for shipping calculations)
        /// </summary>
        public decimal? Weight { get; private set; }

        /// <summary>
        /// Whether the product is featured on homepage
        /// </summary>
        public bool IsFeatured { get; private set; }

        /// <summary>
        /// Discount percentage (0-100)
        /// </summary>
        public decimal DiscountPercentage { get; private set; }

        // Computed properties

        /// <summary>
        /// Calculates the final price after discount
        /// </summary>
        public decimal FinalPrice
        {
            get
            {
                if (DiscountPercentage <= 0)
                    return Price;

                var discount = Price * (DiscountPercentage / 100m);
                return Price - discount;
            }
        }

        /// <summary>
        /// Checks if the product is in stock
        /// </summary>
        public bool IsInStock => StockQuantity > 0;

        /// <summary>
        /// Checks if the product is available for purchase
        /// </summary>
        public bool IsAvailable => Status == ProductStatus.Active && IsInStock;

        // Business methods

        /// <summary>
        /// Updates the product name
        /// </summary>
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidEntityException("Product name is required");

            if (name.Length > 200)
                throw new InvalidEntityException("Product name cannot exceed 200 characters");

            _name = name.Trim();
            MarkAsUpdated();
        }

        /// <summary>
        /// Updates the SKU
        /// </summary>
        public void SetSku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new InvalidEntityException("Product SKU is required");

            if (sku.Length > 50)
                throw new InvalidEntityException("Product SKU cannot exceed 50 characters");

            _sku = sku.Trim().ToUpperInvariant();
            MarkAsUpdated();
        }

        /// <summary>
        /// Updates the product description
        /// </summary>
        public void SetDescription(string description)
        {
            if (description != null && description.Length > 2000)
                throw new InvalidEntityException("Product description cannot exceed 2000 characters");

            _description = description?.Trim();
            MarkAsUpdated();
        }

        /// <summary>
        /// Updates the product price with validation
        /// </summary>
        public void SetPrice(decimal price)
        {
            if (price <= 0)
                throw new BusinessRuleViolationException("Product price must be greater than zero");

            if (price > 1000000)
                throw new BusinessRuleViolationException("Product price cannot exceed 1,000,000");

            _price = price;
            MarkAsUpdated();
        }

        /// <summary>
        /// Changes the product category
        /// </summary>
        public void ChangeCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new InvalidEntityException("Invalid category ID");

            CategoryId = categoryId;
            MarkAsUpdated();
        }

        /// <summary>
        /// Increases stock quantity
        /// </summary>
        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive", nameof(quantity));

            _stockQuantity += quantity;

            // Automatically activate if stock becomes available
            if (Status == ProductStatus.OutOfStock && _stockQuantity > 0)
            {
                Status = ProductStatus.Active;
            }

            MarkAsUpdated();
        }

        /// <summary>
        /// Decreases stock quantity (for orders)
        /// </summary>
        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive", nameof(quantity));

            if (_stockQuantity < quantity)
                throw new BusinessRuleViolationException($"Insufficient stock. Available: {_stockQuantity}, Requested: {quantity}");

            _stockQuantity -= quantity;

            // Automatically mark as out of stock
            if (_stockQuantity == 0 && Status == ProductStatus.Active)
            {
                Status = ProductStatus.OutOfStock;
            }

            MarkAsUpdated();
        }

        /// <summary>
        /// Sets the stock quantity directly (for inventory management)
        /// </summary>
        public void SetStock(int quantity)
        {
            if (quantity < 0)
                throw new BusinessRuleViolationException("Stock quantity cannot be negative");

            _stockQuantity = quantity;

            // Update status based on stock
            if (_stockQuantity == 0 && Status == ProductStatus.Active)
            {
                Status = ProductStatus.OutOfStock;
            }
            else if (_stockQuantity > 0 && Status == ProductStatus.OutOfStock)
            {
                Status = ProductStatus.Active;
            }

            MarkAsUpdated();
        }

        /// <summary>
        /// Applies a discount to the product
        /// </summary>
        public void ApplyDiscount(decimal discountPercentage)
        {
            if (discountPercentage < 0 || discountPercentage > 100)
                throw new BusinessRuleViolationException("Discount percentage must be between 0 and 100");

            DiscountPercentage = discountPercentage;
            MarkAsUpdated();
        }

        /// <summary>
        /// Removes the discount
        /// </summary>
        public void RemoveDiscount()
        {
            DiscountPercentage = 0;
            MarkAsUpdated();
        }

        /// <summary>
        /// Activates the product (makes it available for purchase)
        /// </summary>
        public void Activate()
        {
            if (Status == ProductStatus.Discontinued)
                throw new BusinessRuleViolationException("Cannot activate a discontinued product");

            Status = _stockQuantity > 0 ? ProductStatus.Active : ProductStatus.OutOfStock;
            MarkAsUpdated();
        }

        /// <summary>
        /// Marks the product as discontinued
        /// </summary>
        public void Discontinue()
        {
            Status = ProductStatus.Discontinued;
            MarkAsUpdated();
        }

        /// <summary>
        /// Sets the product as featured
        /// </summary>
        public void SetFeatured(bool isFeatured)
        {
            IsFeatured = isFeatured;
            MarkAsUpdated();
        }

        /// <summary>
        /// Sets the product image URL
        /// </summary>
        public void SetImageUrl(string imageUrl)
        {
            if (imageUrl != null && imageUrl.Length > 500)
                throw new InvalidEntityException("Image URL cannot exceed 500 characters");

            _imageUrl = imageUrl;
            MarkAsUpdated();
        }

        /// <summary>
        /// Sets the product weight
        /// </summary>
        public void SetWeight(decimal? weight)
        {
            if (weight.HasValue && weight.Value < 0)
                throw new BusinessRuleViolationException("Weight cannot be negative");

            Weight = weight;
            MarkAsUpdated();
        }

        /// <summary>
        /// Validates if the product can be ordered
        /// </summary>
        public bool CanBeOrdered(int requestedQuantity)
        {
            return Status == ProductStatus.Active
                   && IsInStock
                   && StockQuantity >= requestedQuantity;
        }
    }
}
