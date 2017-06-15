using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Models
{
    public class DiscussionDetailModel
    {
        public string _id { get; set; }
        public AccountModel owner { get; set; }
        public DateTime createDate { get; set; }
        public string text { get; set; }
    }
    public class DiscussionModel
    {
        public string _id { get; set; }
        public AccountModel from { get; set; }
        public AccountModel to { get; set; }
        public string title { get; set; }
        public string state { get; set; }
        public DepartmentModel department { get; set; }
        public DateTime createDate { get; set; }
        public DateTime expDate { get; set; }
        public DiscussionDetailModel[] items { get; set; }
    }
}
