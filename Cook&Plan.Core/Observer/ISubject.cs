
namespace Cook_Plan.Core.Observer
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(CookPlanNotification notification);
    }
}
