using DAL.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace FFA6sem.Model.Models
{
    public class BudgetModel
    {
        public BudgetModel() { }

        public BudgetModel(Budget budget)
        {
            Id           = budget.Id;
            StartDate    = budget.StartDate;
            TimePeriodId = budget.TimePeriodId;
            UserId       = budget.UserId;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Не указана дата начала")]
        public System.DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Не указан временной период")]
        public int TimePeriodId { get; set; }

        [Required(ErrorMessage = "Не указан пользователь")]
        public string UserId { get; set; }
        [BindNever]
        public string Properties { get; set; }

    }
}

