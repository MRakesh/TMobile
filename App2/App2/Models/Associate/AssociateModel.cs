using System;
using System.Collections.Generic;

namespace App2.Models
{
    public class RecruiterTopicPopUpDto
    {
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public List<string> Name { get; set; }
        public List<string> Name1 { get; set; }
    }

    public class RecruiterTopicPopUpBaseDto : BaseAPIResponseModel
    {
        public List<RecruiterTopicPopUpDto> result { get; set; }
    }

}
