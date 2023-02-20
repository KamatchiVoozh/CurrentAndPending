using Document_Manager_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Document_Manager_Project.Controllers
{
    public class ApprovalsController : Controller
    {
        private DataBase_Connetion _db = new DataBase_Connetion();
        string email;
        private readonly ILogger<ApprovalsController> _logger;

        //    public ApprovalsController(ILogger<ApprovalsController> logger)
        //    {
        //        _logger = logger;
        //   }

        [HttpGet]
        public IActionResult Approval_home()
        {
            return View();
        }

         [HttpGet]
        public IActionResult Approve()
        {

            var approval_list = _db.Approval_Details.Where(a => a.Approval_status.Equals("Pending")).ToList(); 
            ViewBag.Approval_Details = approval_list;
            return View("ApprovalsView");
        }

        [HttpPost]
        public IActionResult Approve(string html_val)
        {
            _db.Configuration.ValidateOnSaveEnabled = false;
            Approval_Details _approval = new Approval_Details();
            var value = TempData;
            var values = Request.Form.First();
            var val = values.Key;
            var resultString = Regex.Match(val, @"\d+").Value;
            int number = Int32.Parse(resultString);
            string email = "";
            int count = 0;
            foreach (var item in value)
            {
                count = count + 1;
                if(count == number)
                {
                    email = item.Key;
                    email = email.Trim();
                    break;
                }
               
            }
            
            var approval_list = _db.Approval_Details.Where(a => a.Email.Equals(email)).ToList();
            var Users_List = _db.Users.Where(c => c.Email.Equals(email)).ToList();
            if (Users_List.Any())
            {
                try
                {
                    _db.Approval_Details.Remove(approval_list.First());
                    _db.SaveChanges();
                    _db.Users.Remove(Users_List.First());
                    _db.SaveChanges();
                    Users_List.First().Approval_status = "Approved";
                    _db.Users.Add(Users_List.First());
                    _db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                }
            }


            ViewBag.Type = "Approval";

            return View("Approval_Rejection_Sucess");

        }

        [HttpPost]
        public IActionResult Reject(string html_val)
        {
            _db.Configuration.ValidateOnSaveEnabled = false;
            Approval_Details _approval = new Approval_Details();
            var value = TempData;
            var values = Request.Form.First();
            var val = values.Key;
            var resultString = Regex.Match(val, @"\d+").Value;
            int number = Int32.Parse(resultString);
            string email = "";
            int count = 0;
            foreach (var item in value)
            {
                count = count + 1;
                if (count == number)
                {
                    email = item.Key;
                    email = email.Trim();
                    break;
                }

            }

            var approval_list = _db.Approval_Details.Where(a => a.Email.Equals(email)).ToList();
            var Users_List = _db.Users.Where(c => c.Email.Equals(email)).ToList();
            if (Users_List.Any() || approval_list.Any())
            {
                try
                {
                    _db.Approval_Details.Remove(approval_list.First());
                    _db.SaveChanges();
                    _db.Users.Remove(Users_List.First());
                    _db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                }
            }
            ViewBag.Type = "Rejection";

            return View("Approval_Rejection_Sucess");
        }

        [HttpGet]
        public IActionResult Remove()
        {
            var approval_list = _db.Users.Where(a => a.Approval_status.Equals("Approved") && a.Approval_Type != "Admin").ToList();
            ViewBag.Approval_Details = approval_list;
            return View("RemovalView");
        }



        [HttpPost]
        public IActionResult Remove(string html_val)
        {
            _db.Configuration.ValidateOnSaveEnabled = false;
            Approval_Details _approval = new Approval_Details();
            var value = TempData;
            var values = Request.Form.First();
            var val = values.Key;
            var resultString = Regex.Match(val, @"\d+").Value;
            int number = Int32.Parse(resultString);
            string email = "";
            int count = 0;
            foreach (var item in value)
            {
                count = count + 1;
                if (count == number)
                {
                    email = item.Key;
                    email = email.Trim();
                    break;
                }

            }
            var Users_List = _db.Users.Where(c => c.Email.Equals(email)).ToList();
            if (Users_List.Any())
            {
                try
                {
                    _db.Users.Remove(Users_List.First());
                    _db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                }
            }
            ViewBag.Type_Of = "Rejection";

            return View("Approval_Rejection_Sucess");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}