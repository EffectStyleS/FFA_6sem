using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace FFA6sem.Model.Models
{
    public class PlannedExpensesModel
    {
        public PlannedExpensesModel()
        {

        }
        public PlannedExpensesModel(PlannedExpenses plannedExpenses)
        {
            Id             = plannedExpenses.Id;
            Sum            = plannedExpenses.Sum;
            ExpenseTypeId  = plannedExpenses.ExpenseTypeId;
            BudgetId       = plannedExpenses.BudgetId;
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указана сумма")]
        public Nullable<decimal> Sum { get; set; }

        [Required(ErrorMessage = "Не указан тип расходов")]
        public int ExpenseTypeId { get; set; }

        [Required(ErrorMessage = "Не указан бюджет")]
        public int BudgetId { get; set; }

        public string ExpenseType { get; set; }
    }
}
