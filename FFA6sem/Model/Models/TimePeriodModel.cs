using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace FFA6sem.Model.Models
{
    public class TimePeriodModel
    {
        public TimePeriodModel() { }

        public TimePeriodModel(TimePeriod timePeriod)
        {
            Id   = timePeriod.Id;
            Name = timePeriod.Name;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string Name { get; set; }
    }
}
