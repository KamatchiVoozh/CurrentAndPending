using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Document_Manager_Project.Models
{
    public class Full_Project_Details
    {

        public List<Collaborators_Details_full> Collaborators_Details = new List<Collaborators_Details_full>();

        public string PlaceofPerformance { get; set; }
        public string ProjectName { get; set; }

        public string projectShortName { get; set; }
        public string FirstName { get; set; } 

        public string LastName { get; set; }

        public decimal Effort { get; set; }

        public string GrantNo { get; set; }

        public string Status { get; set; }

        public string Sponsor { get; set; }

        [DataType(DataType.Date)]
        public DateTime Start_date { get; set; }

   
        [DataType(DataType.Date)]
        public DateTime End_Date { get; set; }

 
        public string Funds { get; set; }

        [DefaultValue("00000")]
        public string KFS_Account { get; set; } = "00000";

        public string Role { get; set; }
        
        public string Personnel { get; set; }

        public string Project_Overlap { get; set; }

        public string Project_Summary { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
