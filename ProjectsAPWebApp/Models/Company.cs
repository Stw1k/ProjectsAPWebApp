using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectsAPWebApp.Models
{
    public class Company
    {
        public Company() {
        Workers = new List<Worker>();
          
        }
        public int Id { get; set; }
        [Required(ErrorMessage ="Поле не може бути пустим")]
        [Display(Name ="Компанія")]
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public virtual ICollection<Worker> Workers { get; set; }
    }
}
