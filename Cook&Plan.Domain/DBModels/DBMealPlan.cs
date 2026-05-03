using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cook_Plan.Domain.DBModels
{
    [Table("MealPlans")]
    public class DBMealPlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime WeekStartDate { get; set; }

        public virtual List<DBMealPlanEntry> Entries { get; set; } = new();
    }
}
