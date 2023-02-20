using ceTe.DynamicPDF;
using Document_Manager_Project.Models;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;

namespace Document_Manager_Project.Controllers
{
    public class ProjectsInformationController : Controller
    {
        private DataBase_Connetion _db = new DataBase_Connetion();

        private readonly ILogger<AccountController> _logger;

     //   public ProjectsInformationController(ILogger<AccountController> logger)
     //   {
     //       _logger = logger;
     //   }

        public ProjectsInformationController()
        {
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Search()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateSearch()
        {
            return View("Search_Update");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(Search_Details _Search)
        {
            List<Project_Details_Table> projects_complete_list = new List<Project_Details_Table>();
            if (ModelState.IsValid)
            {
                var researcher_Details = _db.researcher_Table.Where(a => a.FirstName.Equals(_Search.FirstName) && a.LastName.Equals(_Search.LastName)).ToList();
                var researcher_id = researcher_Details.FirstOrDefault().ResearcherId;
                if (researcher_Details.Count() > 0)
                {
                    // ViewBag.project_Details = data;
                    // ViewBag.collaborator_details = data1;

                    var project_IDs = _db.project_Researcher_Tables.Where(a => a.ResearcherId.Equals(researcher_id)).ToList();

                    if (project_IDs.Count() > 0)
                    {
                        foreach (var project in project_IDs)
                        {
                            var project_List = _db.project_Details_Table.Where(a=>a.ProjectId.Equals(project.ProjectId) && (a.ProjectStatus.Contains("Pending")|| a.ProjectStatus.Contains("Current"))).ToList();

                            if (project_List.Count > 0) { 
                            projects_complete_list.Add(project_List.FirstOrDefault());
                            }

                        }
                            projects_complete_list= projects_complete_list.OrderBy(a => a.ProjectStatus).ToList();
                        ViewBag.project_Details = projects_complete_list;
                        ViewBag.Researcher_Id = researcher_id;
                        return View("Search_View_Projects");
                    }
                    else
                    {
                        ViewBag.error = "No records Found";
                        return View();
                    }

                    



                    /*
                    var data = _db.ProjectDetails.Where(a => a.FirstName.Equals(_Search.FirstName) && a.LastName.Equals(_Search.LastName)).ToList();
                    var data1 = _db.CollaboratorDetails.Where(s=>s.FirstName.Equals(_Search.FirstName) && s.LastName.Equals(_Search.LastName)).ToList();
                    foreach (var item in data1)
                    {
                        var projectId = item.Projectid;
                        var internal_val = _db.ProjectDetails.Where(a => a.Project_Id.Equals(projectId)).ToList();
                        var add_val = internal_val.FirstOrDefault();
                        add_val.Effort = item.Effort;
                        add_val.Role = item.Role;
                        add_val.FirstName = item.FirstName;
                        add_val.LastName = item.LastName;
                        data.Add(add_val);


                    }*/

                }
                else
                {
                    ViewBag.error = "No records Found";
                    return View();
                }
            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSearch(Search_Details _Search)
        {
   /*         if (ModelState.IsValid)
            {
                var data = _db.ProjectDetails.Where(a => a.FirstName.Equals(_Search.FirstName) && a.LastName.Equals(_Search.LastName)).ToList();
                var data1 = _db.CollaboratorDetails.Where(s => s.FirstName.Equals(_Search.FirstName) && s.LastName.Equals(_Search.LastName)).ToList();
                foreach (var item in data1)
                {
                    var projectId = item.Projectid;
                    var internal_val = _db.ProjectDetails.Where(a => a.Project_Id.Equals(projectId)).ToList();
                    var add_val = internal_val.FirstOrDefault();
                    add_val.Effort = item.Effort;
                    add_val.Role = item.Role;
                    add_val.FirstName = item.FirstName;
                    add_val.LastName = item.LastName;
                    data.Add(add_val);


                }
                if (data.Count() > 0 || data1.Count > 0)
                {
                    ViewBag.project_Details = data;
                    ViewBag.collaborator_details = data1;
                    return View("UpdateSearchSuccess");
                }
                else
                {
                    ViewBag.error = "No records Found";
                    return View();
                }
            }

            */
            return View();
        }

        [HttpGet("{id}")]
        public IActionResult Update(int id)
        {
            _db.Configuration.ValidateOnSaveEnabled = false;

            return View();
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult Add()
        {

            var PI_Details = _db.researcher_Table;
            ViewBag.PIProjectDetails = PI_Details;
            return View();


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Project_Details_Table projects, string PI_Full_Name, string Project_Summary, int PI_Effort, string Over_Lap, string InKind)
        {
            var parts = PI_Full_Name.Split(' ');
            var lastName = parts.LastOrDefault();
            var firstName = string.Join(" ", parts.Take(parts.Length - 1));
            projects.PIFirstName = firstName;
            projects.PILastName = lastName;
            projects.IsInkind= InKind;



            try
            {
                _db.project_Details_Table.Add(projects);
                _db.SaveChanges();

                var project_details_add = _db.project_Details_Table.Where(a => a.PIFirstName.Contains(firstName) && a.PILastName.Contains(lastName)&& a.ProjectName.Equals(projects.ProjectName)).ToList();
                var project_details_single = project_details_add.FirstOrDefault();

                ViewBag.ProjectID = project_details_single.ProjectId;

                writeintojson(project_details_single.ProjectId, Project_Summary, Over_Lap);


                HttpContext.Session.SetInt32("ProjectID", project_details_single.ProjectId);
                var researcher_details_add = _db.researcher_Table.Where(a => a.FirstName.Contains(firstName) && a.LastName.Contains(lastName)).ToList();
                var reseacher_details_single = researcher_details_add.FirstOrDefault();


                Project_Researcher_Table project_Researcher_Table = new Project_Researcher_Table(); 
                project_Researcher_Table.ProjectId = project_details_single.ProjectId;
                project_Researcher_Table.ResearcherId = reseacher_details_single.ResearcherId;
                project_Researcher_Table.Role = "Principal Investigator";
                project_Researcher_Table.Personnel = "Senior Personnel";
                project_Researcher_Table.Effort = PI_Effort;
                _db.project_Researcher_Tables.Add(project_Researcher_Table);
                _db.SaveChanges();
  
                return View("Collaborators");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.error = "Please enter proper details, addition failed failed";
                return RedirectToAction("Add");
            }

        }

        private void writeintojson(int projectId, string project_Summary, string OverLap)
        {
            var jsonData = System.IO.File.ReadAllText("Misc/Project_String.json");
            // vals = JsonConvert.DeserializeObject<List<KeyValuePair<int, string>>>(jsonToRead);
            //var project_sum = JsonConvert.DeserializeObject<List<Write_file>>(jsonData)
             //         ?? new List<Write_file>();
            var objResponse1 = JsonConvert.DeserializeObject<List<Write_file>>(jsonData);
            objResponse1.Add(new Write_file() { 
                   project_id = projectId,
                   project_Summary = project_Summary,
                   over_Lap = OverLap
              });
            jsonData = JsonConvert.SerializeObject(objResponse1);
            System.IO.File.WriteAllText("Misc/Project_String.json", jsonData);
            //throw new NotImplementedException();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        public IActionResult Collaborators()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Collaborators(Researcher_Collaborator_Table model,string what)
        {
            _db.Configuration.ValidateOnSaveEnabled = false;

           // var projectID = 0;
            var value = TempData;
            //foreach (var item in value)
            // {
             var projectID = HttpContext.Session.GetInt32("ProjectID");
           // }

                var researcher_details_add = _db.researcher_Table.Where(a => a.FirstName.Contains(model.FirstName) && a.LastName.Contains(model.LastName)).ToList();
                if (researcher_details_add.Any())
                {
                    Project_Researcher_Table project_Researcher_Table = new Project_Researcher_Table();
                    var researcher_Id = researcher_details_add[0].ResearcherId;
                    project_Researcher_Table.ResearcherId = researcher_Id;
                    project_Researcher_Table.ProjectId = (int)projectID;
                    project_Researcher_Table.Effort = model.Effort;
                    project_Researcher_Table.Role = model.Role;
                    project_Researcher_Table.Personnel = model.Personnel;
                    _db.project_Researcher_Tables.Add(project_Researcher_Table);
                    _db.SaveChanges();
                    return RedirectToAction("Collaborators");
  
            }
                else if(researcher_details_add.Count() == 0 && model.UID.Any() && model.Email.Any())
                {
                    Researcher_Table researcher = new Researcher_Table();
                    researcher.FirstName = model.FirstName;
                    researcher.LastName = model.LastName;
                    researcher.Email = model.Email;
                    researcher.UID = model.UID;
                    _db.researcher_Table.Add(researcher);
                    _db.SaveChanges();

                    Project_Researcher_Table project_Researcher_Table = new Project_Researcher_Table();
                    var researcher_details_new = _db.researcher_Table.Where(a => a.FirstName.Contains(model.FirstName) && a.LastName.Contains(model.LastName)).ToList();
                    var researcher_Id = researcher_details_new[0].ResearcherId;
                    project_Researcher_Table.ResearcherId = researcher_Id;
                    project_Researcher_Table.ProjectId = (int)projectID;
                    project_Researcher_Table.Effort = model.Effort;
                    project_Researcher_Table.Role = model.Role;
                    project_Researcher_Table.Personnel = model.Personnel;
                    _db.project_Researcher_Tables.Add(project_Researcher_Table);
                    _db.SaveChanges();

                    return RedirectToAction("Collaborators");
      

                }
                else
                {
                    ViewBag.ProjectID = projectID;
                    ViewBag.error = "Please enter the email and UID for researcher not found";
                    return RedirectToAction("Collaborators");
                }

        }


        [HttpPost]
        public IActionResult CollaboratorsSave(Researcher_Collaborator_Table model, string what)
        {
            _db.Configuration.ValidateOnSaveEnabled = false;

          //  var projectID = 0;
            var value = TempData;
            //  foreach (var item in value)
            //  {
            var projectID = HttpContext.Session.GetInt32("ProjectID");
            // }

            var researcher_details_add = _db.researcher_Table.Where(a => a.FirstName.Contains(model.FirstName) && a.LastName.Contains(model.LastName)).ToList();
            if (researcher_details_add.Any())
            {
                Project_Researcher_Table project_Researcher_Table = new Project_Researcher_Table();
                var researcher_Id = researcher_details_add[0].ResearcherId;
                project_Researcher_Table.ResearcherId = researcher_Id;
                project_Researcher_Table.ProjectId = (int)projectID;
                project_Researcher_Table.Effort = model.Effort;
                project_Researcher_Table.Role = model.Role;
                project_Researcher_Table.Personnel = model.Personnel;
                _db.project_Researcher_Tables.Add(project_Researcher_Table);
                _db.SaveChanges();
                return View("AddSuccess");
            }
            else if (researcher_details_add.Count() == 0 && model.UID!=null && model.Email!=null & model.FirstName!=null)
            {
                Researcher_Table researcher = new Researcher_Table();
                researcher.FirstName = model.FirstName;
                researcher.LastName = model.LastName;
                researcher.Email = model.Email;
                researcher.UID = model.UID;
                _db.researcher_Table.Add(researcher);
                _db.SaveChanges();

                Project_Researcher_Table project_Researcher_Table = new Project_Researcher_Table();
                var researcher_details_new = _db.researcher_Table.Where(a => a.FirstName.Contains(model.FirstName) && a.LastName.Contains(model.LastName)).ToList();
                var researcher_Id = researcher_details_new[0].ResearcherId;
                project_Researcher_Table.ResearcherId = researcher_Id;
                project_Researcher_Table.ProjectId = (int)projectID;
                project_Researcher_Table.Effort = model.Effort;
                project_Researcher_Table.Role = model.Role;
                project_Researcher_Table.Personnel = model.Personnel;
                _db.project_Researcher_Tables.Add(project_Researcher_Table);
                _db.SaveChanges();
                return View("AddSuccess");
            }
            else if (researcher_details_add.Count() == 0 && model.UID == null && model.Email == null & model.FirstName != null)
            {
                ViewBag.ProjectID = projectID;
                ViewBag.error = "Please enter the email and UID for researcher not found";
                return RedirectToAction("Collaborators");
            }
            else
            {
                return View("AddSuccess");
            }





        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult C_N_P_NA()
        {
            return View("Search_C_N_P");
        }



        [HttpPost]
        [AllowAnonymous]
        public IActionResult C_N_P_NA(Search_Details _Search)
        {
            List<Current_Pending_Nasa> currentandpending = new List<Current_Pending_Nasa>();

            if (ModelState.IsValid)
            {
                var researcher_Details = _db.researcher_Table.Where(a => a.FirstName.Equals(_Search.FirstName) && a.LastName.Equals(_Search.LastName)).ToList();
                var researcher_id = researcher_Details.FirstOrDefault().ResearcherId;
                if (researcher_Details.Count() > 0)
                {
                     ViewBag.FirstName = _Search.FirstName;
                     ViewBag.LastName = _Search.LastName;
                    

                    var project_IDs = _db.project_Researcher_Tables.Where(a => a.ResearcherId.Equals(researcher_id)).ToList();

                    if (project_IDs.Count() > 0)
                    {
                        decimal effort = 0;
                        foreach (var project in project_IDs)
                        {
                            
                            var project_List = _db.project_Details_Table.Where(a => a.ProjectId.Equals(project.ProjectId)).ToList();
                            var project_List_single = project_List.FirstOrDefault();

                            Current_Pending_Nasa current_Pending_Nasa_single = new Current_Pending_Nasa();

                            current_Pending_Nasa_single.FirstName = _Search.FirstName;
                            current_Pending_Nasa_single.LastName = _Search.LastName;
                            current_Pending_Nasa_single.PIFirstName = project_List_single.PIFirstName;
                            current_Pending_Nasa_single.PILastName = project_List_single.PILastName;
                            current_Pending_Nasa_single.Role = project.Role;
                            current_Pending_Nasa_single.Effort = (project.Effort * 12) / 100;
                            current_Pending_Nasa_single.Start_date = project_List_single.Start_date;
                            current_Pending_Nasa_single.End_Date = project_List_single.End_Date;
                            current_Pending_Nasa_single.sponsor = project_List_single.Sponsor;
                            current_Pending_Nasa_single.Project_Name = project_List_single.ProjectName;
                            current_Pending_Nasa_single.Status = project_List_single.ProjectStatus;
                            current_Pending_Nasa_single.GrantNo = project_List_single.GrantNo;
                            current_Pending_Nasa_single.funds = project_List_single.Funds;

                            if (current_Pending_Nasa_single.Status.Contains("Current")){
                                effort = effort+ current_Pending_Nasa_single.Effort;
                            }

                            currentandpending.Add(current_Pending_Nasa_single);
                        }
                        if(effort > 12)
                        {
                            ViewBag.EffortError = "Please check before Proceeding as The current Effort is > 100 months";
                        }



                        //Sorting
                        var current = currentandpending.Where(a => a.Status.Contains("Current")).ToList();

                        var pending = currentandpending.Where(a => a.Status.Contains("Pending")).ToList();

                        //For current
                        var PI_Projects = current.Where(a => a.Role.Contains("Principal Investigator")).ToList();
                        var Non_PI = current.Where(a => a.Role.Trim() != "Principal Investigator").ToList();

                        if (PI_Projects.Count() > 0)
                            PI_Projects = PI_Projects.OrderBy(a => a.End_Date).ToList();

                        if (Non_PI.Count() > 0)
                            Non_PI = Non_PI.OrderBy(a => a.End_Date).ToList();


                        //For Pending

                        var PI_Projects_pend = pending.Where(a => a.Role.Contains("Principal Investigator")).ToList();
                        var Non_PI_pend = pending.Where(a => a.Role.Trim() != "Principal Investigator").ToList();

                        if (PI_Projects_pend.Count() > 0)
                            PI_Projects_pend = PI_Projects_pend.OrderBy(a => a.End_Date).ToList();

                        if (Non_PI_pend.Count() > 0)
                            Non_PI_pend = Non_PI_pend.OrderBy(a => a.End_Date).ToList();


                        var all = PI_Projects.Concat(Non_PI);

                        all = all.Concat(PI_Projects_pend);
                        all = all.Concat(Non_PI_pend);





                        ViewBag.project_Details = all;
                        return View("SearchSuccess");
                    }
                    else
                    {
                        ViewBag.error = "No records Found";
                        return View("Search_C_N_P");
                    }

                }
                else
                {
                    ViewBag.error = "No records Found";
                    return View("Search_C_N_P");
                }
            }
            else {

                ViewBag.error = "Enter Right Researcher Information";
                return View("Search_C_N_P");
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult C_N_P_NS()
        {
            return View("Search_C_N_P_NS");
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult C_N_P_NS(Search_Details _Search)
        {
            String lastname = "";
            PDF_Generator pDF_Generator = new PDF_Generator();  
            List<Current_Pending_Nasa> currentandpending = new List<Current_Pending_Nasa>();
            List<Current_Pending_Nasa> currentandpending_sorted = new List<Current_Pending_Nasa>();

            if (ModelState.IsValid)
            {
                var researcher_Details = _db.researcher_Table.Where(a => a.FirstName.Equals(_Search.FirstName) && a.LastName.Equals(_Search.LastName)).ToList();
                var researcher_id = researcher_Details.FirstOrDefault().ResearcherId;
                if (researcher_Details.Count() > 0)
                {
                    ViewBag.FirstName = _Search.FirstName;
                    ViewBag.LastName = _Search.LastName;


                    var project_IDs = _db.project_Researcher_Tables.Where(a => a.ResearcherId.Equals(researcher_id)).ToList();

                    if (project_IDs.Count() > 0)
                    {
                        



                        decimal effort = 0;
                        foreach (var project in project_IDs)
                        {

                            var project_List = _db.project_Details_Table.Where(a => a.ProjectId.Equals(project.ProjectId)).ToList();
                            var project_List_single = project_List.FirstOrDefault();

                            Current_Pending_Nasa current_Pending_Nasa_single = new Current_Pending_Nasa();

                            current_Pending_Nasa_single.FirstName = _Search.FirstName;
                            current_Pending_Nasa_single.LastName = _Search.LastName;
                            lastname = _Search.LastName; ;
                            current_Pending_Nasa_single.PIFirstName = project_List_single.PIFirstName;
                            current_Pending_Nasa_single.PILastName = project_List_single.PILastName;
                            current_Pending_Nasa_single.Role = project.Role;
                            current_Pending_Nasa_single.Effort = (project.Effort*12)/100;
                            current_Pending_Nasa_single.Start_date = project_List_single.Start_date;
                            current_Pending_Nasa_single.End_Date = project_List_single.End_Date;
                            current_Pending_Nasa_single.sponsor = project_List_single.Sponsor;
                            current_Pending_Nasa_single.Project_Name = project_List_single.ProjectName;
                            current_Pending_Nasa_single.Status = project_List_single.ProjectStatus;
                            current_Pending_Nasa_single.GrantNo = project_List_single.GrantNo;
                            current_Pending_Nasa_single.primary_place_performance = project_List_single.PlaceOfPerformance;
                            current_Pending_Nasa_single.funds = project_List_single.Funds;

                            var jsonData = System.IO.File.ReadAllText("Misc/Project_String.json");

                            var objResponse1 = JsonConvert.DeserializeObject<List<Write_file>>(jsonData);

                            Write_file result = objResponse1.Find(x => x.project_id.Equals(project.ProjectId));

                            if (result != null)
                            {
                                current_Pending_Nasa_single.overallObj = result.project_Summary;
                                current_Pending_Nasa_single.Statement_of_Overlap = result.over_Lap;
                            }

                            if (current_Pending_Nasa_single.Status.Contains("Current"))
                            {
                                effort = effort + current_Pending_Nasa_single.Effort;
                            }

                            currentandpending.Add(current_Pending_Nasa_single);
                        }
                        if (effort > 12)
                        {
                            ViewBag.EffortError = "Please check before Proceeding as The current Effort is > 100 months";
                        }

                        //Sorting
                        var current = currentandpending.Where(a => a.Status.Contains("Current")).ToList();
                        
                        var pending = currentandpending.Where(a => a.Status.Contains("Pending")).ToList();

                        //For current
                        var PI_Projects = current.Where(a => a.Role.Contains("Principal Investigator")).ToList();
                        var Non_PI = current.Where(a => a.Role.Trim() != "Principal Investigator").ToList();

                        if(PI_Projects.Count()>0)
                        PI_Projects= PI_Projects.OrderBy(a => a.End_Date).ToList();

                        if (Non_PI.Count() > 0)
                            Non_PI = Non_PI.OrderBy(a => a.End_Date).ToList();


                        //For Pending

                        var PI_Projects_pend = pending.Where(a => a.Role.Contains("Principal Investigator")).ToList();
                        var Non_PI_pend = pending.Where(a => a.Role.Trim() != "Principal Investigator").ToList();

                        if (PI_Projects_pend.Count() > 0)
                            PI_Projects_pend = PI_Projects_pend.OrderBy(a => a.End_Date).ToList();

                        if (Non_PI_pend.Count() > 0)
                            Non_PI_pend = Non_PI_pend.OrderBy(a => a.End_Date).ToList();


                        var all = PI_Projects.Concat(Non_PI);

                        all = all.Concat(PI_Projects_pend);
                        all = all.Concat(Non_PI_pend);



                        //currentandpending = (List<Current_Pending_Nasa>)all;
                        //Generate NSF
                        //pDF_Generator.pdfCheck(all);
                        ViewBag.project_Details = all;
                        ViewBag.lastname_save = _Search.LastName;
                        return View("SearchSuccess_NS");
                    }
                    else
                    {
                        ViewBag.error = "No records Found";
                        return View("Search_C_N_P");
                    }

                }
                else
                {
                    ViewBag.error = "No records Found";
                    return View("Search_C_N_P");
                }
            }
            else
            {

                ViewBag.error = "Enter Right Researcher Information";
                return View("Search_C_N_P");
            }
        }


        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Archive(int id)
        { 
            List<Project_Details_Table> project_details = new List<Project_Details_Table>();
             //  var researcher_id = ViewBag.Research_ID;
            try { 
                    var project_IDs = _db.project_Researcher_Tables.Where(a => a.ResearcherId.Equals(id)).ToList();

                    if (project_IDs.Count() > 0)
                    {
                        foreach (var project in project_IDs)
                        {
                            var project_List = _db.project_Details_Table.Where(a => a.ProjectId.Equals(project.ProjectId) && (!a.ProjectStatus.Contains("Pending") && !a.ProjectStatus.Contains("Current"))).ToList();

                            if (project_List.Count > 0)
                            {
                            project_details.Add(project_List.FirstOrDefault());
                            
                            }

                        }
                      //  ViewBag.project_Details = projects_complete_list;
                    ViewBag.Researcher_Id = id;
                    ViewBag.Project_other = project_details;
                    return View("Search_View_Projects_Archive");
                    }
                    else
                    {
                        ViewBag.error = "No records Found";
                        return View();
                    }

            }
            catch(NullReferenceException e)
            {
                ViewBag.error = e.Message;
            }



                    /*
                    var data = _db.ProjectDetails.Where(a => a.FirstName.Equals(_Search.FirstName) && a.LastName.Equals(_Search.LastName)).ToList();
                    var data1 = _db.CollaboratorDetails.Where(s=>s.FirstName.Equals(_Search.FirstName) && s.LastName.Equals(_Search.LastName)).ToList();
                    foreach (var item in data1)
                    {
                        var projectId = item.Projectid;
                        var internal_val = _db.ProjectDetails.Where(a => a.Project_Id.Equals(projectId)).ToList();
                        var add_val = internal_val.FirstOrDefault();
                        add_val.Effort = item.Effort;
                        add_val.Role = item.Role;
                        add_val.FirstName = item.FirstName;
                        add_val.LastName = item.LastName;
                        data.Add(add_val);


                    }*/





            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult C_N_P_NIH()
        {
            return View("Search_NIH");
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult C_N_P_NIH(Search_Details _Search)
        {
            
            List<Current_Pending_NIH> currentandpending = new List<Current_Pending_NIH>();
            List<Current_Pending_NIH> currentandpending_sorted = new List<Current_Pending_NIH>();

            if (ModelState.IsValid)
            {
                var researcher_Details = _db.researcher_Table.Where(a => a.FirstName.Equals(_Search.FirstName) && a.LastName.Equals(_Search.LastName)).ToList();
                var researcher_id = researcher_Details.FirstOrDefault().ResearcherId;
                if (researcher_Details.Count() > 0)
                {
                    ViewBag.FirstName = _Search.FirstName;
                    ViewBag.LastName = _Search.LastName;


                    var project_IDs = _db.project_Researcher_Tables.Where(a => a.ResearcherId.Equals(researcher_id)).ToList();

                    if (project_IDs.Count() > 0)
                    {




                        decimal effort = 0;
                        foreach (var project in project_IDs)
                        {

                            var project_List = _db.project_Details_Table.Where(a => a.ProjectId.Equals(project.ProjectId)).ToList();
                            var project_List_single = project_List.FirstOrDefault();

                            Current_Pending_NIH current_Pending_Nasa_single = new Current_Pending_NIH();

                            current_Pending_Nasa_single.FirstName = _Search.FirstName;
                            current_Pending_Nasa_single.LastName = _Search.LastName;
                            current_Pending_Nasa_single.PIFirstName = project_List_single.PIFirstName;
                            current_Pending_Nasa_single.PILastName = project_List_single.PILastName;
                            current_Pending_Nasa_single.Role = project.Role;
                            current_Pending_Nasa_single.Effort = (project.Effort * 12) / 100;
                            current_Pending_Nasa_single.Start_date = project_List_single.Start_date;
                            current_Pending_Nasa_single.End_Date = project_List_single.End_Date;
                            current_Pending_Nasa_single.sponsor = project_List_single.Sponsor;
                            current_Pending_Nasa_single.Project_Name = project_List_single.ProjectName;
                            current_Pending_Nasa_single.Status = project_List_single.ProjectStatus;
                            current_Pending_Nasa_single.primary_place_performance = project_List_single.PlaceOfPerformance;
                            current_Pending_Nasa_single.funds = project_List_single.Funds;
                            current_Pending_Nasa_single.GrantNo = project_List_single.GrantNo;
                            current_Pending_Nasa_single.start_year = project_List_single.Start_date.Year;
                            current_Pending_Nasa_single.end_year = project_List_single.End_Date.Year;
                            current_Pending_Nasa_single.inkind = project_List_single.IsInkind;
                            var jsonData = System.IO.File.ReadAllText("Misc/Project_String.json");

                            var objResponse1 = JsonConvert.DeserializeObject<List<Write_file>>(jsonData);

                            Write_file result = objResponse1.Find(x => x.project_id.Equals(project.ProjectId));

                            if (result != null)
                            {
                                current_Pending_Nasa_single.overallObj = result.project_Summary;
                                current_Pending_Nasa_single.Statement_of_Overlap = result.over_Lap;
                            }

                            if (current_Pending_Nasa_single.Status.Contains("Current"))
                            {
                                effort = effort + current_Pending_Nasa_single.Effort;
                            }

                            currentandpending.Add(current_Pending_Nasa_single);
                        }
                        if (effort > 12)
                        {
                            ViewBag.EffortError = "Please check before Proceeding as The current Effort is > 100 months";
                        }
                        //Sorting
                        var current = currentandpending.Where(a => a.Status.Contains("Current")).ToList();

                        var pending = currentandpending.Where(a => a.Status.Contains("Pending")).ToList();

                        //For current
                        var PI_Projects = current.Where(a => a.Role.Contains("Principal Investigator")).ToList();
                        var Non_PI = current.Where(a => a.Role.Trim() != "Principal Investigator").ToList();

                        if (PI_Projects.Count() > 0)
                            PI_Projects = PI_Projects.OrderBy(a => a.End_Date).ToList();

                        if (Non_PI.Count() > 0)
                            Non_PI = Non_PI.OrderBy(a => a.End_Date).ToList();


                        //For Pending

                        var PI_Projects_pend = pending.Where(a => a.Role.Contains("Principal Investigator")).ToList();
                        var Non_PI_pend = pending.Where(a => a.Role.Trim() != "Principal Investigator").ToList();

                        if (PI_Projects_pend.Count() > 0)
                            PI_Projects_pend = PI_Projects_pend.OrderBy(a => a.End_Date).ToList();

                        if (Non_PI_pend.Count() > 0)
                            Non_PI_pend = Non_PI_pend.OrderBy(a => a.End_Date).ToList();


                        var all = PI_Projects.Concat(Non_PI);

                        all = all.Concat(PI_Projects_pend);
                        all = all.Concat(Non_PI_pend);

                        ViewBag.project_Details = all;
                        return View("SearchSuccess_NIH");
                    }
                    else
                    {
                        ViewBag.error = "No records Found";
                        return View("Search_NIH");
                    }

                }
                else
                {
                    ViewBag.error = "No records Found";
                    return View("Search_NIH");
                }
            }
            else
            {

                ViewBag.error = "Enter Right Researcher Information";
                return View("Search_NIH");
            }
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult C_N_P_DOD()
        {
            return View("Search_DOD");
        }


        public JsonResult CheckResearcher(string Fname, string Lname)
        {
            bool isValid = _db.researcher_Table.Where(a => a.FirstName.Contains(Fname) && a.LastName.Contains(Lname)).Any();
            return new JsonResult(isValid);
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}