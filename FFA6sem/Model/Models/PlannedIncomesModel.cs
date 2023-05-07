using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace FFA6sem.Model.Models
{
    public class PlannedIncomesModel
    {
        public PlannedIncomesModel()
        {

        }

        public PlannedIncomesModel(PlannedIncomes plannedIncomes)
        {
            Id           = plannedIncomes.Id;
            Sum          = plannedIncomes.Sum;
            IncomeTypeId = plannedIncomes.IncomeTypeId;
            BudgetId     = plannedIncomes.BudgetId;
        }


        public int Id { get; set; }

        [Required(ErrorMessage = "Не указана сумма")]
        public Nullable<decimal> Sum { get; set; }

        [Required(ErrorMessage = "Не указан тип доходов")]
        public int IncomeTypeId { get; set; }

        [Required(ErrorMessage = "Не указан бюджет")]
        public int BudgetId { get; set; }
        public string IncomeType { get; set; }
    }
}
