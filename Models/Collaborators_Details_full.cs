using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Document_Manager_Project.Models
{
    public class Collaborators_Details_full
    {

        public int researcher_id { get; set; }
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }
        [Required]
        public decimal Effort { get; set; }

        
        public string Personnel { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Role { get; set; }


        public string FullName()
        {
            return FirstName + " " + LastName;
        }

       
    }
}
    