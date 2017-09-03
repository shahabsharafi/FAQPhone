using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Models
{
    public class DiscussionDetailModel : INotifyPropertyChanged
    {
        public string _id { get; set; }
        public AccountModel owner { get; set; }
        public DateTime createDate { get; set; }
        public string attachment { get; set; }
        public string text { get; set; }
        string _Icon;
        public string Icon
        {
            get { return _Icon; }
            set { _Icon = value; OnPropertyChanged(); }
        }//0: has not attachment, 1: has attachment, 2: downloading, 3: downloaded

        int _Mode;
        public int Mode
        {
            get { return _Mode; }
            set
            {
                _Mode = value;
                switch (value)
                {
                    case 0:
                        Icon = string.Empty;
                        break;
                    case 1:
                        Icon = Awesome.FontAwesome.FADownload;
                        break;
                    case 2:
                        Icon = Awesome.FontAwesome.FASpinner;
                        break;
                    case 3:
                        Icon = Awesome.FontAwesome.FAFile;
                        break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public class DiscussionModel
    {
        public string _id { get; set; }
        public AccountModel from { get; set; }
        public AccountModel to { get; set; }
        public string title { get; set; }
        public string display { get; set; }
        public int state { get; set; }//0: created, 1: recived, 2:finished, 3: report
        public int cancelation { get; set; }//1: CANCELATION_UNCLEAR, 2:CANCELATION_UNRELATED, 3:CANCELATION_ANNOYING, 4:CANCELATION_OFFENSIV
        public long price { get; set; }
        public decimal payment { get; set; }
        public decimal wage { get; set; }
        public bool userRead { get; set; }
        public bool operatorRead { get; set; }
        public DepartmentModel department { get; set; }
        public DateTime createDate { get; set; }
        public DateTime answerDate { get; set; }
        public DateTime expDate { get; set; }
        public DiscussionDetailModel[] items { get; set; }
        public string[] tags { get; set; }
        public string CreateDate
        {
            get
            {
                return string.Format("{0}: {1}", ResourceManagerHelper.GetValue("discussion_question"), this.createDate.ToString("HH:mm"));
            }
        }
        public string AnswerDate
        {
            get
            {
                return string.Format("{0}: {1}", ResourceManagerHelper.GetValue("discussion_answer"), this.answerDate.ToString("HH:mm"));
            }
        }
        public string Operator
        {
            get
            {
                return string.Format("{0}: {1} {2}", ResourceManagerHelper.GetValue("discussion_operator"), this.to?.profile?.firstName, this.to?.profile?.lastName);
            }
        }
        public string Department
        {
            get
            {
                return this.department.caption;
            }
        }
        public string Caption { get; set; }
        public string Mode { get; set; }
    }
}
