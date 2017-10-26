using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Models
{
    public class MessageModel
    {
        string title { get; set; }
        string text { get; set; }
        AccountModel owner { get; set; }
        DateTime createDate { get; set; }
        DateTime issueDate { get; set; }
        DateTime expireDate { get; set; }
}
}
