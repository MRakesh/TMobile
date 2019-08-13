using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Models
{
    public class BaseAPIResponseModel
    {
        public string targetUrl { get; set; }
        public bool success { get; set; }
        public string error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }
}
