
namespace Cook_Plan.Core.Observer
{
    public class CookPlanSubject : ISubject
    {
        private readonly List<IObserver> _observers = new();

        public void Attach(IObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Detach(IObserver observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        public void Notify(CookPlanNotification notification)
        {
            foreach (var observer in _observers.ToList())
            {
                observer.Update(notification);
            }
        }
    }
}
