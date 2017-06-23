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

[assembly: ExportRenderer(typeof(MessageViewCell), typeof(MessageRenderer))]

namespace FAQPhone.Droid.CustomRenderers
{
    public class MessageRenderer : ViewCellRenderer
    {
        NativeAndroidCell cell;

        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            var nativeCell = (MessageViewCell)item;
            //Console.WriteLine("\t\t" + nativeCell.Text);

            cell = convertView as NativeAndroidCell;
            if (cell == null)
            {
                cell = new NativeAndroidCell(context, nativeCell);
            }
            else
            {
                cell.MessageViewCell.PropertyChanged -= OnNativeCellPropertyChanged;
            }

            nativeCell.PropertyChanged += OnNativeCellPropertyChanged;

            cell.UpdateCell(nativeCell);
            return cell;
        }
        void OnNativeCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var nativeCell = (MessageViewCell)sender;
            if (e.PropertyName == MessageViewCell.BodyProperty.PropertyName)
            {
                cell.MessageTextView.Text = nativeCell.Body;
            }
        }
        /*
        protected override View GetCellCore(Cell item, View convertView, ViewGroup parent, Context context)
        {
            var inflatorservice = (LayoutInflater)Forms.Context.GetSystemService(Android.Content.Context.LayoutInflaterService);
            var textMsgVm = item.BindingContext as DiscussionDetailModel;
            if (textMsgVm != null)
            {
                bool isMine = textMsgVm.owner.username == "user";// App.Bag.username;
                var template = (LinearLayout)inflatorservice.Inflate(isMine ? Resource.Layout.message_item_owner : Resource.Layout.message_item_opponent, null, false);
                //template.FindViewById<TextView>(Resource.Id.timestamp).Text = textMsgVm.Timestamp.ToString("HH:mm");
                template.FindViewById<TextView>(Resource.Id.nick).Text = isMine ? "Me:" : textMsgVm.owner.username + ":";
                template.FindViewById<TextView>(Resource.Id.message).Text = textMsgVm.text;
                //return template;
            }

            return base.GetCellCore(item, convertView, parent, context);
        }


        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnCellPropertyChanged(sender, e);
        }
        */
    }

    internal class NativeAndroidCell : LinearLayout, INativeElementView
    {
        public TextView MessageTextView { get; set; }

        public MessageViewCell MessageViewCell { get; private set; }
        public Element Element => MessageViewCell;

        public NativeAndroidCell(Context context, MessageViewCell cell) : base(context)
        {
            MessageViewCell = cell;

            bool isMine = cell.Owner == App.Bag.username;
            var view = (context as Activity).LayoutInflater.Inflate(isMine ? Resource.Layout.message_item_owner : Resource.Layout.message_item_opponent, null);
            MessageTextView = view.FindViewById<TextView>(Resource.Id.message);

            AddView(view);
        }

        public void UpdateCell(MessageViewCell cell)
        {
            MessageTextView.Text = cell.Body;
        }
    }
}