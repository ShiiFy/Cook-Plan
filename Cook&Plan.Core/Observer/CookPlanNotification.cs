using Cook_Plan.Domain.Enums;

namespace Cook_Plan.Core.Observer
{
    public class CookPlanNotification
    {
        public CookPlanEventType Type { get; }
        public string Message { get; }
        public object? Data { get; }
        public DateTime CreatedAt { get; }

        public CookPlanNotification(
            CookPlanEventType type,
            string message,
            object? data = null)
        {
            Type = type;
            Message = message;
            Data = data;
            CreatedAt = DateTime.Now;
        }
    }
}
