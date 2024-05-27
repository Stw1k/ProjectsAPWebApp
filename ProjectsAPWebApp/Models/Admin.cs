using System.Text.Json.Serialization;

namespace ProjectsAPWebApp.Models
{
    public class Admin
    {
        public Admin() {
            Project = new List<Project>();
            
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        [JsonIgnore]
        public virtual ICollection<Project> Project {  get; set; }


    }
}
