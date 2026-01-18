namespace LiveOn.Ecommerce.Domain.Enums
{
    /// <summary>
    /// User roles for authorization
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Guest user (not authenticated)
        /// </summary>
        Guest = 0,

        /// <summary>
        /// Regular customer
        /// </summary>
        Customer = 1,

        /// <summary>
        /// Store manager with limited admin rights
        /// </summary>
        Manager = 2,

        /// <summary>
        /// Full administrator
        /// </summary>
        Administrator = 3
    }
}
