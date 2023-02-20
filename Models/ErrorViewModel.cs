using Microsoft.AspNetCore.Diagnostics;
    
namespace Document_Manager_Project.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public IExceptionHandlerPathFeature ExceptionInfo { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}