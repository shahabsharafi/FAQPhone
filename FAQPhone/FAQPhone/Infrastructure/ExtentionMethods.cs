using FAQPhone.Infarstructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Infrastructure
{
    public static class ExtentionMethods
    {
        private static readonly string[] pn = { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
        private static readonly string[] en = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public static string ToEnglishNumber(this string strNum)
        {
            string chash = strNum;
            for (int i = 0; i < 10; i++)
                chash = chash.Replace(pn[i], en[i]);
            return chash;
        }
        public static string ToEnglishNumber(this int intNum)
        {
            string chash = intNum.ToString();
            for (int i = 0; i < 10; i++)
                chash = chash.Replace(pn[i], en[i]);
            return chash;
        }
        public static string GetMessage(this Exception e)
        {
            return string.IsNullOrWhiteSpace(e.Message) ? Constants.MESSAGE_UNKNOWN_ERROR : e.Message;
        }

        public static string FormatString(this string input, string format, string d)
        {
            return string.IsNullOrWhiteSpace(input) ? d : string.Format(format, input);
        }
    }
}
