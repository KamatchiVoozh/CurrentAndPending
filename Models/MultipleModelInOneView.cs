using NuGet.DependencyResolver;

namespace Document_Manager_Project.Models
{
    public class MultipleModelInOneView
    {
        public Project_Details Project_Details { get; set; }
        public Collaborators_Details Collab { get; set; }
    }
}
