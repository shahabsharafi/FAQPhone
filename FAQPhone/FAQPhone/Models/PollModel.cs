using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Models
{
    public class Answer
    {
        string text { get; set; }
        string value { get; set; }
    }
    public class Question
    {
        int no { get; set; }
        string title { get; set; }
        Answer[] answers { get; set; }
    }
    public class Result
    {
        public AccountModel account { get; set; }
        public int no { get; set; }
        public string answer { get; set; }
    }
    public class PollModel
    {
        string _id { get; set; }
        string name { get; set; }
        int type { get; set; }
        Question[] questions { get; set; }
        Result[] results { get; set; }
    }
}