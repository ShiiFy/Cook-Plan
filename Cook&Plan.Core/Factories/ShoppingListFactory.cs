using Cook_Plan.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Factories
{
    public abstract class ShoppingListFactory<T>
    {
        public abstract ShoppingList Create(T source);
    }
}
