using System;
using System.Collections.Generic;
using System.Text;

namespace DailyLoan.Model.Resoinse.Pay
{
    public class UploadFileResponse
    {
        public string error { get; set; }
        public string[] filename { get; set; }
        public string message { get; set; }
        public string path { get; set; }
    }
}
