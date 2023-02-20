using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace Document_Manager_Project.Models
{
    public class Current_Pending_NIH { 



        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Status { get; set; }
        public decimal Effort { get; set; }
        public string Role { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_Date { get; set; }

        public string Project_Name { get; set; }

        public string PIFirstName { get; set; }

        public string PILastName { get; set; }

        public string sponsor { get; set; }

        public string overallObj { get; set; }

        public string Statement_of_Overlap { get; set; }

        public string GrantNo { get; set; } 

        public string primary_place_performance { get; set; }   

        public string funds { get; set; }

        public int start_year { get; set; }
        public int end_year { get; set; }

        public string inkind { get; set; }


    }
}
    