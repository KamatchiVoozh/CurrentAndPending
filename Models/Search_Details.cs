using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Document_Manager_Project.Models
{
    public class Search_Details
    {
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idUser { get; set; }
        [Required(ErrorMessage = "Please enter the PIs First name")]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter the PIs Last name")]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }


        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string? Email { get; set; } = null;

        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
