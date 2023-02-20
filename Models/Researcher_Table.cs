using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Document_Manager_Project.Models
{
    public class Researcher_Table
    {
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResearcherId { get; set; }

        [Required(ErrorMessage = "Please enter First Name")]
        [StringLength(30, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        [StringLength(30, MinimumLength = 3)]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Please your UID")]
        public string UID { get; set; }

        [Required(ErrorMessage = "Please enter your Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }


        public string PIPossibility { get; set; } = "N";

        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
