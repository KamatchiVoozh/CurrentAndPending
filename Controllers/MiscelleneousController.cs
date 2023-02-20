using Document_Manager_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;

namespace Document_Manager_Project.Controllers
{
    public class MiscelleneousController : Controller
    {
        private DataBase_Connetion _db = new DataBase_Connetion();

        private readonly ILogger<MiscelleneousController> _logger;
     
        public MiscelleneousController(ILogger<MiscelleneousController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult DownloadDoc()
        {
            // var renderer = new HtmlToPdf();
            // string pdfName = HttpContext.Session.GetString("FirstName") + ".pdf";
            // renderer.RenderHtmlAsPdf("<h1>This is test file</h1>").SaveAs(pdfName);
            // return RedirectToAction("Search", "ProjectsInformation");

           
            return View();
        }

        [HttpPost]
        public FileResult Export(string GridHtml)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                String render_l = GridHtml.Replace("\r\n", "<br />");
                sr = new StringReader(render_l);
                Over_Lap model = new Over_Lap();
                String html_string = "<h2 align =\"center\">Current and Pending</h2>";
                html_string = html_string.Replace("\r\n", "<br />");
                StringReader sr_1 = new StringReader(html_string);
                Document pdfDoc = new Document(PageSize.A4);
                pdfDoc.SetMargins(60f, 50f, 10f,10f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
               // XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr_1);
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "Current_And_Pending.pdf");
            }
           
        }


        [HttpPost]
        public async Task<FileResult> Export_NSAsync()
        {
            var path = Path.Combine(
                          Directory.GetCurrentDirectory(),"Output.pdf");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/pdf", Path.GetFileName(path));

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}