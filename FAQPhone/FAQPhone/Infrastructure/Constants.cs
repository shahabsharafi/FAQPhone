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
        public const string OPERATOR_FAQ = "operator_faq";
        public const string USER_CREATE_FAQ = "user_create_faq";
        public const string USER_INPROGRESS_FAQ = "user_inprogress_faq";
        public const string USER_FAQ = "user_faq";
        public const string SIGNOUT = "signout";

        public const string ACCESS_OPERATOR = "access_operator";
        public const string ACCESS_USER = "access_user";

        public const string CANCELATION_UNCLEAR = "cancelation_unclear";
        public const string CANCELATION_UNRELATED = "cancelation_unrelated";
        public const string CANCELATION_ANNOYING = "cancelation_annoying";
        public const string CANCELATION_OFFENSIV = "cancelation_offensiv";

        public const string CANCELATION_UNCLEAR_TEXT = "cancelation_unclear_text";
        public const string CANCELATION_UNRELATED_TEXT = "cancelation_unrelated_text";
        public const string CANCELATION_ANNOYING_TEXT = "cancelation_annoying_text";
        public const string CANCELATION_OFFENSIV_TEXT = "cancelation_offensiv_text";
    } 
}
