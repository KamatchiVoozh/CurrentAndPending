
// using ceTe.DynamicPDF.Merger;
using Microsoft.AspNetCore.Mvc;
using IronPdf;
using PdfDocument = Syncfusion.Pdf.PdfDocument;
using Microsoft.AspNetCore.StaticFiles;

namespace Document_Manager_Project.Models
{
    public class PDF_Generator
    {
        public void pdfCheck(IEnumerable<Current_Pending_Nasa> currentandpending)
        {
            string save_last_name = "";
            IronPdf.License.LicenseKey = "IRONPDF.KAMATCHIVOOZHIAN.2993-BDE5E3B10F-CTJGJH747E4RTHV-2MJVLXL3XGBL-TGO4VTFUUD2Y-Y7PJJD4NQKLI-JQNS5F3EYXGH-DPMNM3-TAIK46ZW4L6JEA-DEPLOYMENT.TRIAL-6AJB3H.TRIAL.EXPIRES.21.MAR.2023";
            bool result = IronPdf.License.IsValidLicense("IRONPDF.KAMATCHIVOOZHIAN.2993-BDE5E3B10F-CTJGJH747E4RTHV-2MJVLXL3XGBL-TGO4VTFUUD2Y-Y7PJJD4NQKLI-JQNS5F3EYXGH-DPMNM3-TAIK46ZW4L6JEA-DEPLOYMENT.TRIAL-6AJB3H.TRIAL.EXPIRES.21.MAR.2023");
            bool is_licensed = IronPdf.License.IsLicensed;
            IronPdf.PdfDocument document = IronPdf.PdfDocument.FromFile("Misc/NSF_CP.pdf");
            //MergeDocument document = new MergeDocument( "Misc/NSF_CP.pdf");
            foreach (var items in currentandpending)
            {
                document.Form.Fields[0].Value = items.FirstName + " " + items.LastName; //Rseracher Name
                save_last_name = items.LastName; 
                document.Form.Fields[1].Value = "";
                document.Form.Fields[2].Value = "";
                document.Form.Fields[3].Value = "University of Maryland";
                document.Form.Fields[4].Value = "College Park Maryland";
                document.Form.Fields[5].Value = items.FirstName + " " + items.LastName;
                document.Form.Fields[6].Value = DateTime.Now.ToString("MM/dd/yyyy");
                break;
            }


            int i = 7;
            foreach (var item in currentandpending) {

                document.Form.Fields[i].Value = item.Project_Name; 
                i = i + 1;

                if (item.Status.Trim().Equals("Current"))
                {
                    document.Form.Fields[i].Value = "On";
                    i =  i+2;
                }
                else if (item.Status.Trim().Equals("Pending"))
                {
                    i = i + 1;
                    document.Form.Fields[i].Value = "On";
                    i = i + 1;
                }

                document.Form.Fields[i].Value = item.GrantNo;
                    i = i + 1;
                document.Form.Fields[i].Value = item.sponsor;
                i = i + 1;
                document.Form.Fields[i].Value = item.primary_place_performance;
                i = i + 1;
                document.Form.Fields[i].Value = item.Start_date.ToString("MM/yyyy");
                i = i + 1;
                document.Form.Fields[i].Value = item.End_Date.ToString("MM/yyyy");
                i = i + 1;

                document.Form.Fields[i].Value = item.funds;
                i = i + 1;

                var start_year = item.Start_date.ToString("yyyy");
                int start_year_val = Int32.Parse(start_year);

                int end_year_val = Int32.Parse(item.End_Date.ToString("yyyy"));

                if (start_year_val <= end_year_val) {
                    document.Form.Fields[i].Value = start_year_val.ToString(); //1
                    i = i + 1;
                    start_year_val = start_year_val + 1;
                    document.Form.Fields[i].Value = item.Effort.ToString(); //1
                    i = i + 1;
                }
                else
                {
                    document.Form.Fields[i].Value = ""; //1
                    i = i + 1;
                    document.Form.Fields[i].Value =""; //1
                    i = i + 1;
                }


                if (start_year_val <= end_year_val)
                {
                    document.Form.Fields[i].Value = start_year_val.ToString(); //2
                    i = i + 1;
                    start_year_val = start_year_val + 1;
                    document.Form.Fields[i].Value = item.Effort.ToString(); //2
                    i = i + 1;
                }
                else
                {
                    document.Form.Fields[i].Value = ""; //2
                    i = i + 1;
                    document.Form.Fields[i].Value = ""; //2
                    i = i + 1;
                }

                if (start_year_val <= end_year_val)
                {
                    document.Form.Fields[i].Value = start_year_val.ToString(); //3
                    i = i + 1;
                    start_year_val = start_year_val + 1;
                    document.Form.Fields[i].Value = item.Effort.ToString(); //3
                    i = i + 1;
                }
                else
                {
                    document.Form.Fields[i].Value = ""; //3
                    i = i + 1;
                    document.Form.Fields[i].Value = ""; //3
                    i = i + 1;
                }

                if (start_year_val <= end_year_val)
                {
                    document.Form.Fields[i].Value = start_year_val.ToString(); //4
                    i = i + 1;
                    start_year_val = start_year_val + 1;
                    document.Form.Fields[i].Value = item.Effort.ToString(); //4
                    i = i + 1;
                }
                else
                {
                    document.Form.Fields[i].Value = ""; //4
                    i = i + 1;
                    document.Form.Fields[i].Value = ""; //4
                    i = i + 1;
                }

                if (start_year_val <= end_year_val)
                {
                    document.Form.Fields[i].Value = start_year_val.ToString(); //5
                    i = i + 1;
                    start_year_val = start_year_val + 1;
                    document.Form.Fields[i].Value = item.Effort.ToString(); //5
                    i = i + 1;
                }
                else
                {
                    document.Form.Fields[i].Value = ""; //5
                    i = i + 1;
                    document.Form.Fields[i].Value = ""; //5
                    i = i + 1;
                }


                document.Form.Fields[i].Value = item.overallObj;
                i = i + 1;

                document.Form.Fields[i].Value = item.Statement_of_Overlap;
                i = i + 1;
            }
            //document.Draw("MISC/Output.pdf");
            document.SaveAs(save_last_name + "Output.pdf");
            //FileStream from_stream = System.IO.File.OpenRead("MISC/Output.pdf");
            //return File(from_stream, "application/pdf", "DownloadName.pdf");
        
        }

    }
}
