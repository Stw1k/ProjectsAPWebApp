using System.Text.Json.Serialization;

namespace ProjectsAPWebApp.Models
{
    public class Worker
    {
        public Worker()
        {
            Project = new List<Project>();
            Company = new Company();
            CompanyId = 1;
            Company.Name = "a";
            Company.Description = "b";
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Exp { get; set; }
        public int? CompanyId { get; set; }
        [JsonIgnore]
        public virtual Company Company { get; set; }
        [JsonIgnore]
        public virtual ICollection<Project> Project { get; set; }
    }
}
