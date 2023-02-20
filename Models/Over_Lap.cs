using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Document_Manager_Project.Models
{
    public class Over_Lap
    {
        public int ProjectId { get; set; }
        public string Over_lap { get; set; }
    }
}
