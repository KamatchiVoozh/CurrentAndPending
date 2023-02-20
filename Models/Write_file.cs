using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Document_Manager_Project.Models
{
    public class Write_file
    {
        public int project_id { get; set; }
        public string project_Summary { get; set; }
        public string over_Lap { get; set; }
    }
}
