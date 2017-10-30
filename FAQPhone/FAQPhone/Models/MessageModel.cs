using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Models
{
    public class MessageModel
    {
        public string _id { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string owner { get; set; }
        public DateTime createDate { get; set; }
        public DateTime issueDate { get; set; }
        public DateTime expireDate { get; set; }
}
}
