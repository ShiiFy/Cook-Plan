namespace Cook_Plan.Domain.Models
{
    /// План питания на одну календарную неделю.
    public class MealPlan
    {
        public int Id { get; set; }

        /// Дата начала недели (всегда понедельник).
        /// Используется как ключ для поиска плана на конкретную неделю
        public DateOnly WeekStartDate { get; set; }

        /// Все приёмы пищи, запланированные на эту неделю
        public List<MealPlanEntry> Entries { get; set; } = new();
    }
}
