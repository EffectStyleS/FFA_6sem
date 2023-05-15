using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace FFA6sem.Model.Models
{
    public class ExpenseModel
    {
        public ExpenseModel() { }

        public ExpenseModel(Expense expense)
        {
            Id            = expense.Id;
            Name          = expense.Name;
            Value         = expense.Value;
            Date          = expense.Date;
            ExpenseTypeId = expense.ExpenseTypeId;
            UserId        = expense.UserId;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указано значение")]
        public decimal Value { get; set; }

        [Required(ErrorMessage = "Не указана дата")]
        public System.DateTime Date { get; set; }

        [Required(ErrorMessage = "Не указан тип расхода")]
        public int ExpenseTypeId { get; set; }

        [Required(ErrorMessage = "Не указан пользователь")]
        public string UserId { get; set; }
        public string ExpenseType { get; set; }
        public string OnlyDate
        {
            get
            {
                return Date.ToShortDateString();
            }
        }
    }
}
