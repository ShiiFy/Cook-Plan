using Cook_Plan.Core.Composite;

namespace Cook_Plan.Core.Iterator
{
    public class MealPlanIterator : IIterator<IMealComponent>
    {
        private readonly IMealAggregate _aggregate;
        private int _currentIndex = 0;

        public MealPlanIterator(IMealAggregate aggregate)
        {
            _aggregate = aggregate;
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
            return _currentIndex >= _aggregate.Count;
        }

        public IMealComponent CurrentItem()
        {
            if (IsDone())
                throw new InvalidOperationException("Итератор вышел за пределы коллекции.");

            return _aggregate.GetChild(_currentIndex);
        }
    }
}
