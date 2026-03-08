namespace Cook_Plan.Domain.Models
{
    /// Позиция в списке покупок. Хранит продукт,
    public class ShoppingListItem
    {
        public int Id { get; set; }

        /// Идентификатор списка покупок, которому принадлежит позиция
        public int ShoppingListId { get; set; }

        /// Идентификатор продукта из справочника
        public int ProductId { get; set; }

        /// Продукт из справочника с названием и единицей измерения
        public Product? Product { get; set; }

        /// Суммарное количество продукта по всем рецептам плана питания
        public double Amount { get; set; }

        /// Признак того, что продукт уже куплен. Отмечается пользователем вручную
        public bool IsPurchased { get; set; }
    }
}