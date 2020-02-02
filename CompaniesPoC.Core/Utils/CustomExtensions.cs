using System.Collections.Generic;
using System.Linq;

namespace CompaniesPoC.Core.Utils
{
    public static class CustomExtensions
    {
        public static bool NotNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }
    }
}
