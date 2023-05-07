using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace FFA6sem.Model.Models
{
    public class IncomeModel
    {
        public IncomeModel()
        {
        }

        public IncomeModel(Income income)
        {
            Id           = income.Id;
            Name         = income.Name;
            Value        = income.Value;
            Date         = income.Date;
            UserId       = income.UserId;
            IncomeTypeId = income.IncomeTypeId;
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указано значение")]
        public decimal Value { get; set; }

        [Required(ErrorMessage = "Не указана дата")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Не указан пользователь")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Не указан тип дохода")]
        public int IncomeTypeId { get; set; }
        public string IncomeType { get; set; }
        public string OnlyDate
        {
            get
            {
                return Date.ToShortDateString();
            }
        }

    }
}
