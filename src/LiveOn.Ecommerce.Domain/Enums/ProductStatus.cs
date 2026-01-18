namespace LiveOn.Ecommerce.Domain.Enums
{
    /// <summary>
    /// Represents the lifecycle status of a product
    /// </summary>
    public enum ProductStatus
    {
        /// <summary>
        /// Product is in draft state, not visible to customers
        /// </summary>
        Draft = 0,

        /// <summary>
        /// Product is active and available for purchase
        /// </summary>
        Active = 1,

        /// <summary>
        /// Product is out of stock but will be restocked
        /// </summary>
        OutOfStock = 2,

        /// <summary>
        /// Product is discontinued and will not be restocked
        /// </summary>
        Discontinued = 3,

        /// <summary>
        /// Product is archived (soft deleted)
        /// </summary>
        Archived = 4
    }
}
