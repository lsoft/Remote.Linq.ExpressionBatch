using System;
using System.Linq;

namespace Shared
{
    public interface IQueryableByTypeProvider
    {
        Func<Type, IQueryable> QueryableByTypeProvider
        {
            get;
        }
    }
}
