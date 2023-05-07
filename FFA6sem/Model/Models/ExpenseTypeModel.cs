using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace FFA6sem.Model.Models
{
    public class ExpenseTypeModel
    {
        public ExpenseTypeModel() { }
        public ExpenseTypeModel(ExpenseType expenseType)
        {
            Id   = expenseType.Id;
            Name = expenseType.Name;
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string Name { get; set; }
    }
}
