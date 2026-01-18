using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveOn.Ecommerce.Domain.Common
{
    /// <summary>
    /// Base class for Value Objects - objects that don't have identity and are compared by their values
    /// Examples: Money, Address, Email
    /// </summary>
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }
            return left is null || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return GetEqualityComponentsEquals(GetEqualityComponents(), other.GetEqualityComponents());
        }

        private bool GetEqualityComponentsEquals(IEnumerable<object> components1, IEnumerable<object> components2)
        {
            using (var enumerator1 = components1.GetEnumerator())
            using (var enumerator2 = components2.GetEnumerator())
            {
                while (enumerator1.MoveNext())
                {
                    if (!enumerator2.MoveNext() || !Equals(enumerator1.Current, enumerator2.Current))
                    {
                        return false;
                    }
                }

                return !enumerator2.MoveNext();
            }
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public static bool operator ==(ValueObject one, ValueObject two)
        {
            return EqualOperator(one, two);
        }

        public static bool operator !=(ValueObject one, ValueObject two)
        {
            return NotEqualOperator(one, two);
        }
    }
}
