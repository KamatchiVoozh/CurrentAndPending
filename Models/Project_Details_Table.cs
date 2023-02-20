using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Document_Manager_Project.Models
{
    public class Project_Details_Table
    {
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }


        [StringLength(30, MinimumLength = 3)]
        public string GrantNo { get; set; }

        [Required(ErrorMessage = "Please enter Proposal Name")]
        [StringLength(300, MinimumLength = 3)]
        public string ProjectName { get; set; }

   
        [StringLength(200, MinimumLength = 3)]
        public string ProjectShortName { get; set; }
        

        [Required(ErrorMessage = "Please enter Proposal Status")]
        [StringLength(10, MinimumLength = 3)]
        public string ProjectStatus { get; set; }

        [Required(ErrorMessage = "Please enter Proposal Sponsor")]
        [StringLength(30, MinimumLength = 3)]
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


        [Required(ErrorMessage = "Please enter Place Of Performance")]
        [StringLength(30, MinimumLength = 3)]
        public string PlaceOfPerformance { get; set; }


        [DefaultValue("No")]
        public string IsInkind { get; set; } = "No";



        //   [Required(ErrorMessage = "Please enter PI's First Name")]
        [StringLength(30, MinimumLength = 3)]
        public string PIFirstName { get; set; }

     //   [Required(ErrorMessage = "Please enter PI's Last Name")]
        [StringLength(30, MinimumLength = 3)]
        public string PILastName { get; set; }

     //   [Required(ErrorMessage = "Please enter PI's Full Name")]
     //   [StringLength(30, MinimumLength = 3)]
     //   public string FullName { get; set; }

       // public string FullName()
        //{
        //    return FirstName + " " + LastName;
       // }
    }
}
