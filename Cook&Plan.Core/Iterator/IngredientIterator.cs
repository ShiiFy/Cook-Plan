using Cook_Plan.Core.Composite;
using Cook_Plan.Domain.Models;

namespace Cook_Plan.Core.Iterator
{
    public class IngredientIterator : IIterator<Ingredient>
    {
        private readonly List<Ingredient> _ingredients;
        private int _currentIndex = 0;

        public IngredientIterator(IMealComponent mealComponent)
        {
            _ingredients = mealComponent.GetIngredientsList();
        }

        public void First()
        {
            _currentIndex = 0;
        }

        public void Next()
        {
            _currentIndex++;
        }

        public bool IsDone()
        {
            return _currentIndex >= _ingredients.Count;
        }

        public Ingredient CurrentItem()
        {
            if (IsDone())
                throw new InvalidOperationException("Итератор вышел за пределы списка ингредиентов.");

            return _ingredients[_currentIndex];
        }
    }
}
