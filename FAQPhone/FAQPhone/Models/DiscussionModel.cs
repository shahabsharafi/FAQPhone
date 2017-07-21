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
        public string display { get; set; }
        public int state { get; set; }
        public DepartmentModel department { get; set; }
        public DateTime createDate { get; set; }
        public DateTime expDate { get; set; }
        public DiscussionDetailModel[] items { get; set; }
        public string[] tags { get; set; }
        public string CreateDate
        {
            get
            {
                return this.createDate.ToString("HH:mm");
            }
        }
        public string Operator
        {
            get
            {
                return this.to?.profile?.firstName + " " + this.to?.profile?.lastName;
            }
        }
        public string Department
        {
            get
            {
                return this.department.caption;
            }
        }
    }
}
