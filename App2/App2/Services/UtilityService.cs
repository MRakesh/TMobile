using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Services
{
    public class UtilityService
    {
        public static List<string> GetFileFormats()
        {
            return new List<string>
            {
                ".png",".jpeg",".doc",".docx",".pdf"
            };
        }
    }
}
