using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Factories
{
    public abstract class ShoppingListFactory<T>
    {
        public abstract ShoppingList Create(T source);
        public string GetSummary (T source)
        {
            var list = Create(source);
            return $"Список покупок: {list.Items.Count} позиций, создан {list.CreatedAt}";
        }
    }
}
