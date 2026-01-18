using System;

namespace LiveOn.Ecommerce.Domain.Exceptions
{
    /// <summary>
    /// Base domain exception - all custom exceptions should inherit from this
    /// </summary>
    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message)
        {
        }

        protected DomainException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }

    public class EntityNotFoundException : DomainException
    {
        public EntityNotFoundException(string entityName, object key)
            : base($"Entity '{entityName}' with key '{key}' was not found.")
        {
        }
    }

    public class InvalidEntityException : DomainException
    {
        public InvalidEntityException(string message) : base(message)
        {
        }
    }

    public class BusinessRuleViolationException : DomainException
    {
        public BusinessRuleViolationException(string message) : base(message)
        {
        }
    }
}
