using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Inferstructure
{
    public class Constants
    {
        public static string RestUrl { get { return "http://192.168.1.104:4000/api/{0}"; } }
        //public static string RestUrl { get { return "http://94.182.227.163:4000/api/{0}"; } }

        public const string OPERATOR_RECEIVE_FAQ = "operator_receive_faq";
        public const string OPERATOR_INPROGRESS_FAQ = "operator_inprogress_faq";
        public const string OPERATOR_ARCHIVED_FAQ = "operator_archived_faq";
        public const string OPERATOR_FAQ = "operator_faq";
        public const string USER_CREATE_FAQ = "user_create_faq";
        public const string USER_INPROGRESS_FAQ = "user_inprogress_faq";
        public const string USER_ARCHIVED_FAQ = "user_archived_faq";
        public const string USER_FAQ = "user_faq";
        public const string SIGNOUT = "signout";
    }
}
