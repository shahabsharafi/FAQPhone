using System.ComponentModel;
using System.Net;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;
using CustomRenderer;
using FAQPhone.Droid.CustomRenderers;
using FAQPhone.Views;
using FAQPhone.Models;
using Android.App;

[assembly: ExportRenderer(typeof(DiscussionViewCell), typeof(DiscussionRenderer))]

namespace FAQPhone.Droid.CustomRenderers
{
    public class DiscussionRenderer : ViewCellRenderer
    {
        internal class NativeAndroidCell : LinearLayout, INativeElementView
        {
            public TextView DepartmentTextView { get; set; }
            public TextView CreateDateTextView { get; set; }
            public TextView OperatorTextView { get; set; }
            public TextView LastAnswerTextView { get; set; }
            public TextView TitleTextView { get; set; }

            public DiscussionViewCell DiscussionViewCell { get; private set; }
            public Element Element => DiscussionViewCell;

            public NativeAndroidCell(Context context, DiscussionViewCell cell) : base(context)
            {
                DiscussionViewCell = cell;

                var view = (context as Activity).LayoutInflater.Inflate(Resource.Layout.discustion_item, null);

                DepartmentTextView = view.FindViewById<TextView>(Resource.Id.discussion_department);
                CreateDateTextView = view.FindViewById<TextView>(Resource.Id.discussion_createdate);
                OperatorTextView = view.FindViewById<TextView>(Resource.Id.discussion_operator);
                LastAnswerTextView = view.FindViewById<TextView>(Resource.Id.discussion_lastanswer);
                TitleTextView = view.FindViewById<TextView>(Resource.Id.discussion_title);

                var font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, "Irsans.ttf");

                DepartmentTextView.Typeface = font;
                CreateDateTextView.Typeface = font;
                OperatorTextView.Typeface = font;
                LastAnswerTextView.Typeface = font;
                TitleTextView.Typeface = font;

                AddView(view);
            }

            public void UpdateCell(DiscussionViewCell cell)
            {
                DepartmentTextView.Text = cell.Department;
                CreateDateTextView.Text = cell.CreateDate;
                OperatorTextView.Text = cell.Operator;
                LastAnswerTextView.Text = cell.AnswerDate;
                TitleTextView.Text = cell.Title;
            }

        }

        NativeAndroidCell cell;

        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            var nativeCell = (DiscussionViewCell)item;

            cell = convertView as NativeAndroidCell;
            if (cell == null)
            {
                cell = new NativeAndroidCell(context, nativeCell);
            }
            else
            {
                cell.DiscussionViewCell.PropertyChanged -= OnNativeCellPropertyChanged;
            }

            nativeCell.PropertyChanged += OnNativeCellPropertyChanged;

            cell.UpdateCell(nativeCell);
            return cell;
        }
        void OnNativeCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var nativeCell = (DiscussionViewCell)sender;

            if (e.PropertyName == DiscussionViewCell.DepartmentProperty.PropertyName)
            {
                cell.DepartmentTextView.Text = nativeCell.Department;
            }
            if (e.PropertyName == DiscussionViewCell.CreateDateProperty.PropertyName)
            {
                cell.CreateDateTextView.Text = nativeCell.CreateDate;
            }
            if (e.PropertyName == DiscussionViewCell.OperatorProperty.PropertyName)
            {
                cell.OperatorTextView.Text = nativeCell.Operator;
            }
            if (e.PropertyName == DiscussionViewCell.AnswerDateProperty.PropertyName)
            {
                cell.LastAnswerTextView.Text = nativeCell.AnswerDate;
            }
            if (e.PropertyName == DiscussionViewCell.TitleProperty.PropertyName)
            {
                cell.TitleTextView.Text = nativeCell.Title;
            }
        }        
    }    
}