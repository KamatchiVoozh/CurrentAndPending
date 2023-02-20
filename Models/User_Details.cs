using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Document_Manager_Project.Models
{
    public class User_Details
    {
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idUser { get; set; }


        [Required(ErrorMessage = "Please enter your First name")]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your Last name")]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Please enter your Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]

        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Passwords do Not Match")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }


        public string Approval_status { get; set; } = "Pending";

      
        public string Approval_Type { get; set; }


        public string FullName()
        {
            return FirstName + " " + LastName;
        }

       
    }
}
    