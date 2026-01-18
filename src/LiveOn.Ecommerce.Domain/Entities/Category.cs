using System;
using System.Collections.Generic;
using LiveOn.Ecommerce.Domain.Common;
using LiveOn.Ecommerce.Domain.Enums;
using LiveOn.Ecommerce.Domain.Exceptions;

namespace LiveOn.Ecommerce.Domain.Entities
{
    /// <summary>
    /// Represents a product category in the e-commerce system
    /// This is an Aggregate Root
    /// </summary>
    public class Category : BaseEntity
    {
        private string _name;
        private string _description;
        private string _slug;
        private readonly List<Product> _products;

        // Private constructor for EF
        private Category()
        {
            _products = new List<Product>();
        }

        /// <summary>
        /// Creates a new category
        /// </summary>
        public Category(string name, string description, int? parentCategoryId = null) : this()
        {
            SetName(name);
            SetDescription(description);
            ParentCategoryId = parentCategoryId;
            IsActive = true;
            _slug = GenerateSlug(name);
        }

        /// <summary>
        /// Category name (required, max 100 characters)
        /// </summary>
        public string Name
        {
            get => _name;
            private set => _name = value;
        }

        /// <summary>
        /// Category description (optional, max 500 characters)
        /// </summary>
        public string Description
        {
            get => _description;
            private set => _description = value;
        }

        /// <summary>
        /// URL-friendly slug for the category
        /// </summary>
        public string Slug
        {
            get => _slug;
            private set => _slug = value;
        }

        /// <summary>
        /// Parent category ID for hierarchical categories (nullable for root categories)
        /// </summary>
        public int? ParentCategoryId { get; private set; }

        /// <summary>
        /// Navigation property for parent category
        /// </summary>
        public virtual Category ParentCategory { get; private set; }

        /// <summary>
        /// Navigation property for child categories
        /// </summary>
        public virtual ICollection<Category> SubCategories { get; private set; }

        /// <summary>
        /// Whether the category is active and visible to customers
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Products in this category
        /// </summary>
        public virtual ICollection<Product> Products => _products.AsReadOnly();

        /// <summary>
        /// Display order for sorting categories
        /// </summary>
        public int DisplayOrder { get; private set; }

        // Business methods

        /// <summary>
        /// Updates the category name
        /// </summary>
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidEntityException("Category name is required");

            if (name.Length > 100)
                throw new InvalidEntityException("Category name cannot exceed 100 characters");

            _name = name.Trim();
            _slug = GenerateSlug(_name);
            MarkAsUpdated();
        }

        /// <summary>
        /// Updates the category description
        /// </summary>
        public void SetDescription(string description)
        {
            if (description != null && description.Length > 500)
                throw new InvalidEntityException("Category description cannot exceed 500 characters");

            _description = description?.Trim();
            MarkAsUpdated();
        }

        /// <summary>
        /// Sets the parent category for hierarchical organization
        /// </summary>
        public void SetParentCategory(int? parentCategoryId)
        {
            if (parentCategoryId.HasValue && parentCategoryId.Value == Id)
                throw new BusinessRuleViolationException("A category cannot be its own parent");

            ParentCategoryId = parentCategoryId;
            MarkAsUpdated();
        }

        /// <summary>
        /// Activates the category
        /// </summary>
        public void Activate()
        {
            IsActive = true;
            MarkAsUpdated();
        }

        /// <summary>
        /// Deactivates the category
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
            MarkAsUpdated();
        }

        /// <summary>
        /// Sets the display order for sorting
        /// </summary>
        public void SetDisplayOrder(int order)
        {
            if (order < 0)
                throw new InvalidEntityException("Display order cannot be negative");

            DisplayOrder = order;
            MarkAsUpdated();
        }

        /// <summary>
        /// Generates a URL-friendly slug from the category name
        /// </summary>
        private string GenerateSlug(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            return name.ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("&", "and")
                // Remove special characters (simple implementation)
                .Replace("'", "")
                .Replace("\"", "");
        }
    }
}
