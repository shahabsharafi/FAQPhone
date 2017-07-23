using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomRenderer
{
    public class DiscussionViewCell : ViewCell
    {
        public static readonly BindableProperty DepartmentProperty = BindableProperty.Create("Department", typeof(string), typeof(DiscussionViewCell), "");

        public string Department
        {
            get { return (string)GetValue(DepartmentProperty); }
            set { SetValue(DepartmentProperty, value); }
        }

        public static readonly BindableProperty CreateDateProperty = BindableProperty.Create("CreateDate", typeof(string), typeof(DiscussionViewCell), "");

        public string CreateDate
        {
            get { return (string)GetValue(CreateDateProperty); }
            set { SetValue(CreateDateProperty, value); }
        }

        public static readonly BindableProperty OperatorProperty = BindableProperty.Create("Operator", typeof(string), typeof(DiscussionViewCell), "");

        public string Operator
        {
            get { return (string)GetValue(OperatorProperty); }
            set { SetValue(OperatorProperty, value); }
        }

        public static readonly BindableProperty AnswerDateProperty = BindableProperty.Create("LastAnswer", typeof(string), typeof(DiscussionViewCell), "");

        public string AnswerDate
        {
            get { return (string)GetValue(AnswerDateProperty); }
            set { SetValue(AnswerDateProperty, value); }
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(DiscussionViewCell), "");

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
    }
}
