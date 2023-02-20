using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Document_Manager_Project.Models
{
    public class Project_Researcher_Table
    {
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

       
        public int ProjectId { get; set; }

        public int ResearcherId { get; set; }


        public decimal Effort { get; set; }

        public string Role { get; set; }

        public string Personnel { get; set; }

       
    }
}
