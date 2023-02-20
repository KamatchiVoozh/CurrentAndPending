using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Document_Manager_Project.Models
{
    
    public class Researcher_Collaborator_Table
    {
      //  public List<Researcher_Collaborator_Table>? Reseachers { get; set; }
       /* public Researcher_Collaborator_Table() {
            FirstName = "";
            LastName = "";
            UID = "";
            Email = "";
            Effort = 0;
            Role = "";
            Personnel = "";

        }   */

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

        public decimal Effort { get; set; }

        public string Role { get; set; }

        public string Personnel { get; set; }

        //public List<Researcher_Collaborator_Table> Reseachers { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
