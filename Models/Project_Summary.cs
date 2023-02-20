using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Document_Manager_Project.Models
{
    public class Project_Summary
    {
        public int ProjectId { get; set; }
        public string summary { get; set; }
    }
}
