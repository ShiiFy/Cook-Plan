

namespace Cook_Plan.Core.Observer
{
    public interface IObserver
    {
        void Update(CookPlanNotification notification);
    }
}
