using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace FFA6sem.Model.Models
{
    public class IncomeTypeModel
    {
        public IncomeTypeModel()
        {
        }

        public IncomeTypeModel(IncomeType incomeType)
        {
            Id   = incomeType.Id;
            Name = incomeType.Name;
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string Name { get; set; }
    }
}
