using System.Text.Json.Serialization;

namespace ProjectsAPWebApp.Models
{
    public class Project
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public int WorkerId { get; set; }
        //[JsonIgnore]
        public virtual Admin Admin { get; set; }
        //[JsonIgnore]
        public virtual Worker Worker { get; set;}
    }
}
