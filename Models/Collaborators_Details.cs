using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Document_Manager_Project.Models
{
    public class Collaborators_Details
    {
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollaboratorId { get; set; }

        public int Projectid { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public int Effort { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Personnel { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Role { get; set; }


        public string FullName()
        {
            return FirstName + " " + LastName;
        }

       
    }
}
    