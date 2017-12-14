using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Models
{
    public class ProfileModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fatherName { get; set; }
        public string no { get; set; }
        public string placeOfIssues { get; set; }
        public string nationalCode { get; set; }
        public DateTime? birthDay { get; set; }
        public string birthPlace { get; set; }
        public string sex { get; set; }
        public string status { get; set; }
        public string jobState { get; set; }
        public string religion { get; set; }
        public string sect { get; set; }
        public string reference { get; set; }
        public bool IsComplete()
        {
            return
                !string.IsNullOrWhiteSpace(this.firstName) &&
                !string.IsNullOrWhiteSpace(this.lastName) &&
                !string.IsNullOrWhiteSpace(this.fatherName) &&
                !string.IsNullOrWhiteSpace(this.no) &&
                !string.IsNullOrWhiteSpace(this.placeOfIssues) &&
                !string.IsNullOrWhiteSpace(this.nationalCode) &&
                this.birthDay != null &&
                !string.IsNullOrWhiteSpace(this.birthPlace) &&
                !string.IsNullOrWhiteSpace(this.sex) &&
                !string.IsNullOrWhiteSpace(this.status);
        }
    }
    public class ContactModel {
        public string house { get; set; }
        public string work { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string pcode { get; set; }
        public bool IsComplete()
        {
            return
                !string.IsNullOrWhiteSpace(this.house) &&
                !string.IsNullOrWhiteSpace(this.work) &&
                !string.IsNullOrWhiteSpace(this.country) &&
                !string.IsNullOrWhiteSpace(this.province) &&
                !string.IsNullOrWhiteSpace(this.city) &&
                !string.IsNullOrWhiteSpace(this.address) &&
                !string.IsNullOrWhiteSpace(this.pcode);
        }
    }

    public class EducationModel
    {
        public string grade { get; set; }
        public string major { get; set; }
        public string university { get; set; }
        public string level { get; set; }
        public bool IsComplete()
        {
            return
                !string.IsNullOrWhiteSpace(this.grade) &&
                !string.IsNullOrWhiteSpace(this.major) &&
                !string.IsNullOrWhiteSpace(this.university) &&
                !string.IsNullOrWhiteSpace(this.level);
        }
    }

    public class AccountComment
    {
        public DateTime createDate { get; set; }
        public string text { get; set; }
    }
    public class AccountModel
    {
        public string _id { get; set; }
        public string username { get; set; }
        public bool blocked { get; set; }
        public bool disabled { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public bool isOperator { get; set; }
        public bool isUser { get; set; }
        public bool isOrganization { get; set; }
        public bool isManager { get; set; }
        public bool sexPrevention { get; set; }
        public long? price { get; set; }
        public bool isAdmin { get; set; }
        public string orgCode { get; set; }
        public ProfileModel profile { get; set; }
        public ContactModel contact { get; set; }
        public EducationModel education { get; set; }
        public int state { get; set; }
        public decimal credit { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsOnline { get; set; }
        public AccountComment[] comments { get; set; }
        public bool IsComplete()
        {
            return
                this.profile.IsComplete() &&
                this.contact.IsComplete() &&
                this.education.IsComplete();
        }
    }
}
