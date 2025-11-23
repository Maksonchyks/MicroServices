using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Domain.Common
{
    public abstract class ValueObject
    {
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            return GetEqualityComponents().SequenceEqual(((ValueObject)obj).GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x?.GetHashCode() ?? 0)
                .Aggregate((x, y) => x ^ y);
        }

        protected abstract IEnumerable<object?> GetEqualityComponents();
    }
}
