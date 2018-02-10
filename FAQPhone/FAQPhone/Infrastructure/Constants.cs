using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Infarstructure
{
    public class Constants
    {
        public static string BaseUrl { get { return "http://94.182.227.163:4000"; } }
        //public static string BaseUrl { get { return "http://192.168.1.104:4000"; } }
        public static string RestUrl { get { return BaseUrl + "/api/{0}"; } }
        public static string UploadUrl { get { return BaseUrl + "/api/uploads"; } }
        public static string DownloadUrl { get { return BaseUrl + "/uploads"; } }

        public const string INFO_URL = "info_url";

        public const string OPERATOR_RECEIVE_FAQ = "operator_receive_faq";
        public const string OPERATOR_INPROGRESS_FAQ = "operator_inprogress_faq";
        public const string OPERATOR_FAQ = "operator_faq";
        public const string USER_CREATE_FAQ = "user_create_faq";
        public const string USER_INPROGRESS_FAQ = "user_inprogress_faq";
        public const string USER_FAQ = "user_faq";
        public const string ACCOUNT = "account";
        public const string MY_DISCOUNT = "discount_tab";
        public const string ALL_MESSAGES = "all_messages";
        public const string OPERATOR_LIST = "operator_list";
        public const string CONTACT_US = "contactus";
        public const string RULES = "rules";
        public const string INFO = "info";
        public const string ABOUT_US = "about_us";
        public const string SETTING = "setting";
        public const string REPORT = "report";
        public const string REPORT_BALANCE = "report_balance";
        public const string REPORT_QUICK = "report_quick";
        public const string INTERNAL_SETTING = "internal_setting";
        public const string APP_VERSION = "app_version";
        public const string CHECK_VERSION = "check_version";
        public const string CHANGE_PASSWORD = "change_password";
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

        public const int DISCUSSION_STATE_CREATE = 0;
        public const int DISCUSSION_STATE_RECIVED = 1;
        public const int DISCUSSION_STATE_FINISHED = 2;
        public const int DISCUSSION_STATE_REPORT = 3;

        public const string MESSAGE_TITLE_ALERT = "message_title_alert";
        public const string MESSAGE_UNKNOWN_ERROR = "message_unknown_error";
        public const string MESSAGE_ARE_YOU_SURE = "message_are_you_sure";

        public const string COMMAND_OK = "command_ok";
        public const string COMMAND_YES = "command_yes";
        public const string COMMAND_NO = "command_no";

        public const string COMMAND_TRY = "command_try";
        public const string COMMAND_CONNECT = "command_connect";
    } 
}
