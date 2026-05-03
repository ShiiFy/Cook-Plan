using Cook_Plan.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Composite
{
    public interface IMealComponent
    {
        List<Ingredient> GetIngredientsList();

        void Add(IMealComponent component);

        void Remove(IMealComponent component);

        IMealComponent GetChild(int index);
    }
}
