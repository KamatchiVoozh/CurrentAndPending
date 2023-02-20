using Document_Manager_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Document_Manager_Project.Controllers
{
    public class AccountController : Controller
    {
        private DataBase_Connetion _db = new DataBase_Connetion();

        private readonly ILogger<AccountController> _logger;

        public AccountController()
        {
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
          //  _logger.LogInformation("Login Page");
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
           // _logger.LogInformation("Register Page");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User_Details _user)
        {
            Approval_Details _approval = new Approval_Details();

            if (ModelState.IsValid)
            {
                var check = _db.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    
                   

                    _approval.FirstName = _user.FirstName;
                    _approval.LastName = _user.LastName;
                    _approval.Email = _user.Email;
                    _approval.Approval_Type = _user.Approval_Type;
                    _approval.Approval_status = _user.Approval_status;
                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.Approval_Details.Add(_approval);

                    _db.Users.Add(_user);
                    _db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Invalid Data Entered please recheck the fields";
                    return View();
                }
            }
            

                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = _db.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password) && s.Approval_status.Equals("Approved")).ToList();
                if (data.Count() > 0)
                {
                    string authId = GenerateAuthId();
                    // Set the login values to check the authenticity : From Stack Overflow
                    // Store the value in both our Session and a Cookie.
                    HttpContext.Session.SetString("AuthId_Token", authId);
                    CookieOptions options = new CookieOptions()
                    {
                        Path = "/",
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    };
                    Response.Cookies.Append("AuthCookie_Token", authId, options);

                    //add session
                    var FullName = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                      byte[] FullName_bytes = Encoding.ASCII.GetBytes(FullName);
                    var Access_Type = data.FirstOrDefault().Approval_Type;
                    if (Access_Type.Contains("Researcher")){
                        HttpContext.Session.SetString("Researcher", "Researcher");
                    }
                    HttpContext.Session.SetString("FirstName", data.FirstOrDefault().FirstName);
                    HttpContext.Session.SetString("LastName", data.FirstOrDefault().LastName);
                    HttpContext.Session.SetString("FullName", FullName);

                    var access = data.FirstOrDefault().Approval_Type;

                    if (access.Contains("Admin"))
                    {
                        HttpContext.Session.SetString("Admin", "True");
                        return View("Welcome_Admin");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Admin", "False");
                        return View("Welcome");
                    }

                    //Session["idUser"] = data.FirstOrDefault().idUser;
                    //  var idUser = data.FirstOrDefault().Email;
                    //  byte[] idUser_bytes = Encoding.ASCII.GetBytes(idUser);
                    //  HttpContext.Session.Set("FullName", idUser_bytes);
                    //  return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return View();
                }
            }
          //  _logger.LogInformation("Login Page");
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();//remove session
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Admin");
            HttpContext.Session.Remove("AuthId_Token");



            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            foreach (var cookie in Request.Cookies.Keys) { Response.Cookies.Delete(cookie); }

            return RedirectToAction("Login");
        }
        public ActionResult Back()
        {
           
            if (HttpContext.Session.GetString("Admin") == "True"){
                return View("Welcome_Admin");
            }
            else
            {
                return View("Welcome");
            }
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
        private string GenerateAuthId()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }

        public ActionResult Projects()

        {
            if (HttpContext.Session.GetString("Admin") == "True")
            {
                return View("Welcome_Admin");
            }
            else
            {

                return View("Welcome");

            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}