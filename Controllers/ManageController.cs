using Document_Manager_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
//using System.Web.Mvc;
using Xunit;

namespace Document_Manager_Project.Controllers
{
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private DataBase_Connetion _db = new DataBase_Connetion();

        private readonly ILogger<ManageController> _logger;

        //   public ProjectsInformationController(ILogger<AccountController> logger)
        //   {
        //       _logger = logger;
        //   }

        public ManageController()
        {
        }




        [HttpGet("{id}")]
        public IActionResult Update(int id)
        {

            _db.Configuration.ValidateOnSaveEnabled = false;
            var data = _db.ProjectDetails.Where(a => a.Project_Id.Equals(id));
            // var data1 = _db.CollaboratorDetails.Where(s => s.Projectid.Equals(id));
            foreach (var item in data)
            {
                ViewBag.project_Details = item;
            }
            //   if (data1.Count() > 0) { 
            //    foreach (var item in data1)
            //    {
            //       ViewBag.collaborator_details = data1;
            //    }
            //    }
            //    else
            //    {
            ViewBag.collaborators_count = 0;
            //    }
            if (data.Count() > 0)//|| data1.Count() > 0)
            {
                return View("Update_Page");
            }
            else
            {
                ViewBag.error = "Something is wrong, Let us recheck our million files";
                return View("UpdateSearchSuccess");
            }

        }

        [HttpPost]
        public IActionResult UpdateDetails(Project_Details project_Details)
        {
            _db.Configuration.ValidateOnSaveEnabled = false;

            return View();
        }


        [HttpGet("{id}")]
        public IActionResult View(int id)
        {
            Full_Project_Details full_Project_Details = new Full_Project_Details();
            _db.Configuration.ValidateOnSaveEnabled = false;
            var data = _db.project_Details_Table.Where(a => a.ProjectId.Equals(id));
           
            var PI_data = data.FirstOrDefault();


            full_Project_Details.ProjectName = PI_data.ProjectName;
            full_Project_Details.GrantNo = PI_data.GrantNo;
            full_Project_Details.projectShortName = PI_data.ProjectShortName;
            full_Project_Details.FirstName = PI_data.PIFirstName;
            full_Project_Details.LastName = PI_data.PILastName;
            full_Project_Details.KFS_Account = PI_data.KFS_Account;
            full_Project_Details.Sponsor = PI_data.Sponsor;
            full_Project_Details.Start_date = PI_data.Start_date;
            full_Project_Details.End_Date = PI_data.End_Date;
            full_Project_Details.Status = PI_data.ProjectStatus;
            full_Project_Details.PlaceofPerformance = PI_data.PlaceOfPerformance;
            full_Project_Details.Funds = PI_data.Funds;

            var data1 = _db.project_Researcher_Tables.Where(s => s.ProjectId.Equals(id));
            foreach (var t in data1)
            {
                var researcher = _db.researcher_Table.Where(a => a.ResearcherId.Equals(t.ResearcherId)).FirstOrDefault();
                if (researcher != null && researcher.FirstName.Contains(PI_data.PIFirstName.Trim()) && researcher.LastName.Contains(PI_data.PILastName.Trim())) {
                    full_Project_Details.Effort = t.Effort;
                    full_Project_Details.Role = t.Role;
                    full_Project_Details.Personnel = t.Personnel;
                }
                else if (researcher != null)
                {
                    Collaborators_Details_full collaborators_det = new Collaborators_Details_full();
                    collaborators_det.Personnel = t.Personnel;
                    collaborators_det.Effort = t.Effort;
                    collaborators_det.Role = t.Role;
                    collaborators_det.FirstName = researcher.FirstName;
                    collaborators_det.LastName = researcher.LastName;

                    full_Project_Details.Collaborators_Details.Add(collaborators_det);
                }

            }

            if (data.Count() > 0 || data1.Count() > 0)
            {
                ViewBag.Full_project = full_Project_Details;
                return View("View_Project_Details");
            }
            else
            {
                ViewBag.error = "Something is wrong, Let us recheck our million files";
                return View("UpdateSearchSuccess");
            }

        }

        public IActionResult Add_Collab_edit(){
            var project_ID = Int32.Parse(TempData["ProjectID"].ToString());
            ViewBag.ProjectID = project_ID;
            return RedirectToAction("Collaborators", "ProjectsInformation");
        }

        public IActionResult Collab_edit(List <string> FirstName, List<string> LastName, List<int>Effort, List<string> Role, List<int> Id)
        {
            TempData["ProjectID"] = TempData["ProjectID"];
            var Project_id = TempData["ProjectID"]; 
            var no_of_collab = Id.Count();
            for(int i = 0; i < no_of_collab; i++)
            {
 
                int Id_val = Id[i];
                var details = _db.project_Researcher_Tables.Where(a => a.ResearcherId.Equals(Id_val)).ToList().FirstOrDefault();
                _db.Configuration.ValidateOnSaveEnabled = false;
                if (Effort[i] != null )
                {
                    details.Effort = Effort[i];
                }
                if (Role[i] != null)
                {
                    details.Role = Role[i];
                }
                if (Effort[i] !=null || Role[i] != null)
                {
                    _db.project_Researcher_Tables.AddOrUpdate(details);
                    _db.SaveChanges();
                }
                
            }
            Edit_Details(null, "Edit", null);
            return View("Collabs_Edit");
        }

        public IActionResult Collaborators()
        {
            return RedirectToAction("Collaborators", "ProjectsInformation");
        }

        [HttpGet("{id}")]
        public IActionResult Edit(int id)
        {
            Full_Project_Details full_Project_Details = new Full_Project_Details();
            _db.Configuration.ValidateOnSaveEnabled = false;
            var data = _db.project_Details_Table.Where(a => a.ProjectId.Equals(id));
            Collaborators_Details_full collaborators_det = new Collaborators_Details_full();
            var PI_data = data.FirstOrDefault();


            full_Project_Details.ProjectName = PI_data.ProjectName;
            full_Project_Details.GrantNo = PI_data.GrantNo;
            full_Project_Details.projectShortName = PI_data.ProjectShortName;
            full_Project_Details.FirstName = PI_data.PIFirstName;
            full_Project_Details.LastName = PI_data.PILastName;
            full_Project_Details.KFS_Account = PI_data.KFS_Account;
            full_Project_Details.Sponsor = PI_data.Sponsor;
            full_Project_Details.Start_date = PI_data.Start_date;
            full_Project_Details.End_Date = PI_data.End_Date;
            full_Project_Details.Status = PI_data.ProjectStatus;
            full_Project_Details.PlaceofPerformance = PI_data.PlaceOfPerformance;
            full_Project_Details.Funds = PI_data.Funds;

            var data1 = _db.project_Researcher_Tables.Where(s => s.ProjectId.Equals(id));
            foreach (var t in data1)
            {
                var researcher = _db.researcher_Table.Where(a => a.ResearcherId.Equals(t.ResearcherId)).FirstOrDefault();
                if (researcher != null && researcher.FirstName.Contains(PI_data.PIFirstName.Trim()) && researcher.LastName.Contains(PI_data.PILastName.Trim()))
                {
                    full_Project_Details.Effort = t.Effort;
                    full_Project_Details.Role = t.Role;
                    full_Project_Details.Personnel = t.Personnel;
                }
                else if (researcher != null)
                {
                    collaborators_det.Personnel = t.Personnel;
                    collaborators_det.Effort = t.Effort;
                    collaborators_det.Role = t.Role;
                    collaborators_det.FirstName = researcher.FirstName;
                    collaborators_det.LastName = researcher.LastName;

                    full_Project_Details.Collaborators_Details.Add(collaborators_det);
                }

            }

            if (data.Count() > 0 || data1.Count() > 0)
            {
                ViewBag.Project_ID = id;
                ViewBag.Full_project = full_Project_Details;
                return View("Edit_Project_Details");
            }
            else
            {
                ViewBag.error = "Something is wrong, Let us recheck our million files";
                return View("UpdateSearchSuccess");
            }

        }

        [HttpPost]
        public IActionResult Edit_Details(Project_Details_Table project_Details,string id,string Effort)
        {
            
            var project_ID = Int32.Parse(TempData["ProjectID"].ToString());
    
            var details = _db.project_Details_Table.Where(a => a.ProjectId.Equals(project_ID)).FirstOrDefault();
            _db.Configuration.ValidateOnSaveEnabled = false;
            var researcher_data = _db.project_Researcher_Tables.Where(s => s.ProjectId.Equals(project_ID));

            if (id==null || !id.Equals("Edit"))
             { 

                    var GrantNo = project_Details.GrantNo;
                    if (GrantNo != null)
                    {
                        details.GrantNo = GrantNo;
            
                    }
                    var ProjectName = project_Details.GrantNo;
                    if (ProjectName != null)
                    {
                        details.ProjectName = ProjectName;
              
                    }
                    var ProjectShortName = project_Details.ProjectShortName;
                    if (ProjectShortName != null)
                    {
                        details.ProjectShortName = ProjectShortName;
               
                    }
                    var Sponsor = project_Details.Sponsor;
                    if (Sponsor != null)
                    {
                        details.Sponsor = Sponsor;
            
                    }
                    var Start_date = project_Details.Start_date;
                    var check = Start_date.ToString();
                    if (!Start_date.ToString().Equals("1/1/0001 12:00:00 AM"))
                    {
                        details.Start_date = Start_date;
                
                    }
                    var End_Date = project_Details.End_Date;
                    if (!End_Date.ToString().Equals("1/1/0001 12:00:00 AM"))
                    {
                        details.Start_date = End_Date;
                
                    }
                    var Funds = project_Details.Funds;
                    if (Funds != null)
                    {
                        details.Funds = Funds;
               
                    }
       
                    var PlaceOfPerformance = project_Details.PlaceOfPerformance;
                    if (PlaceOfPerformance != null)
                    {
                        details.PlaceOfPerformance = PlaceOfPerformance;
               
                    }

                    var status_Change = project_Details.ProjectStatus.ToString();
                    if ((status_Change != null) && status_Change.Equals("Current"))
                    {
                
                        details.ProjectStatus = project_Details.ProjectStatus;
                
                        if(project_Details.KFS_Account != null) {
                        details.KFS_Account = project_Details.KFS_Account;
                        }


                    }
                    else if((status_Change != null))
                    {
                       // var details = _db.project_Details_Table.Where(a => a.ProjectId.Equals(project_ID)).FirstOrDefault();
                        //var details = new Project_Details_Table { ProjectId = project_ID, ProjectStatus = status_Change };
                        // _db.project_Details_Table.Attach(details);
                        //  _db.Entry(details).Property(r => r.ProjectStatus).IsModified = true;
                        details.PlaceOfPerformance = details.PlaceOfPerformance.ToString().Trim();
                        details.ProjectStatus = project_Details.ProjectStatus.ToString();
                        //_db.project_Details_Table.AddOrUpdate(details);
                
                    }
                    //Need to add effort into the table
                    try
                    {
               
                        _db.project_Details_Table.AddOrUpdate(details);
                        _db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        var exception = e;
                    }
            
                    if (Effort != null)
                    {
                        //var PI_Information = _db.project_Researcher_Tables.Where(s => s.ProjectId.Equals(project_ID) && );
                        //if (researcher != null && researcher.FirstName.Contains(details.PIFirstName.Trim()) && researcher.LastName.Contains(PI_data.PILastName.Trim()))
                        foreach (var t in researcher_data)
                        {
                            var researcher = _db.researcher_Table.Where(a => a.ResearcherId.Equals(t.ResearcherId)).FirstOrDefault();
                            if (researcher.FirstName.Contains(details.PIFirstName.Trim()) && researcher.LastName.Contains(details.PILastName.Trim()))
                            {
                                t.Effort = Int32.Parse(Effort);
                                _db.project_Researcher_Tables.AddOrUpdate(t);
                                _db.SaveChanges();
                            }
                        }
                    }
                    if (id!=null && id.Equals("5"))
                    {
                   
                        
                        List<Collaborators_Details_full> collaborators_det_full = new List<Collaborators_Details_full>();
                        foreach (var t in researcher_data)
                        {
                            var researcher = _db.researcher_Table.Where(a => a.ResearcherId.Equals(t.ResearcherId)).FirstOrDefault();
                            if (!researcher.FirstName.Contains(details.PIFirstName.Trim()) && !researcher.LastName.Contains(details.PILastName.Trim()))
                            {
                            Collaborators_Details_full collaborators_det = new Collaborators_Details_full();
                            collaborators_det.researcher_id = t.ResearcherId;
                                collaborators_det.Personnel = t.Personnel;
                                collaborators_det.Effort = t.Effort;
                                collaborators_det.Role = t.Role;
                                collaborators_det.FirstName = researcher.FirstName;
                                collaborators_det.LastName = researcher.LastName;

                                collaborators_det_full.Add(collaborators_det);
                            }

                        }

                        ViewBag.Project_ID = project_ID;
                        HttpContext.Session.SetInt32("ProjectID", project_ID);
                        ViewBag.Collab = collaborators_det_full;

                        return View("Collabs_Edit");
                    }
                    else { 
                    return View("Edit_Details_Success");
                    }
            }
            else
            {
                
                List<Collaborators_Details_full> collaborators_det_full = new List<Collaborators_Details_full>();
                foreach (var t in researcher_data)
                {
                    var researcher = _db.researcher_Table.Where(a => a.ResearcherId.Equals(t.ResearcherId)).FirstOrDefault();
                    if (!researcher.FirstName.Contains(details.PIFirstName.Trim()) && !researcher.LastName.Contains(details.PILastName.Trim()))
                    {
                        Collaborators_Details_full collaborators_det = new Collaborators_Details_full();
                        collaborators_det.researcher_id = t.ResearcherId;
                        collaborators_det.Personnel = t.Personnel;
                        collaborators_det.Effort = t.Effort;
                        collaborators_det.Role = t.Role;
                        collaborators_det.FirstName = researcher.FirstName;
                        collaborators_det.LastName = researcher.LastName;

                        collaborators_det_full.Add(collaborators_det);
                    }

                }

                ViewBag.Project_ID = project_ID;
                HttpContext.Session.SetInt32("ProjectID", project_ID);
                ViewBag.Collab = collaborators_det_full;

                return View("Collabs_Edit");
            }
        }

        [HttpGet]
        public IActionResult Edit_Details_Collab()
        {
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}