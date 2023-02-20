using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Document_Manager_Project.Models
{
    public class Project_Details
    {
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Project_Id { get; set; }

        [Required(ErrorMessage = "Please enter Proposal Name")]
        [StringLength(200, MinimumLength = 3)]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Please enter PI's First Name")]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; } 

        [Required(ErrorMessage = "Please enter PI's Last Name")]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }
        // [Required]
        // [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        //   public string Email { get; set; }

        [Required(ErrorMessage = "Please enter PI's Effort Name")]
        public int Effort { get; set; }

        [Required(ErrorMessage = "Please enter Proposal Status")]
        [StringLength(10, MinimumLength = 3)]
        public string Status { get; set; }

        [Required(ErrorMessage = "Please enter Proposal Sponsor")]
        [StringLength(20, MinimumLength = 3)]
        public string Sponsor { get; set; }

        [Required(ErrorMessage = "Please enter Proposal Start Date")]
        [DataType(DataType.Date)]
        public DateTime Start_date { get; set; }

        [Required(ErrorMessage = "Please enter Proposal End Date")]
        [DataType(DataType.Date)]
        public DateTime End_Date { get; set; }

        [Required(ErrorMessage = "Please enter Funding Amount Details")]
        [StringLength(10, MinimumLength = 3)]
        public string Funds { get; set; }

        [DefaultValue("00000")]
        public string KFS_Account { get; set; } = "00000";

        [Required(ErrorMessage = "Please enter the role of Researcher")]
        [StringLength(30, MinimumLength = 3)]
        public string Role { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
