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
        internal class NativeAndroidCell : LinearLayout, INativeElementView
        {
            public TextView MessageTextView { get; set; }
            public TextView MessageIconView { get; set; }

            public MessageViewCell MessageViewCell { get; private set; }
            public Element Element => MessageViewCell;

            public NativeAndroidCell(Context context, MessageViewCell cell) : base(context)
            {
                MessageViewCell = cell;

                bool isMine = cell.Owner == App.Username;
                var view = (context as Activity).LayoutInflater.Inflate(isMine ? Resource.Layout.message_item_owner : Resource.Layout.message_item_opponent, null);
                MessageIconView = view.FindViewById<TextView>(Resource.Id.message_icon);
                MessageTextView = view.FindViewById<TextView>(Resource.Id.message);
                if (string.IsNullOrEmpty(cell.Icon))
                {
                    MessageTextView.Visibility = ViewStates.Visible;
                    MessageIconView.Visibility = ViewStates.Invisible;
                }
                else
                {
                    MessageTextView.Visibility = ViewStates.Invisible;
                    MessageIconView.Visibility = ViewStates.Visible;
                }
                
                AddView(view);
            }

            public void UpdateCell(MessageViewCell cell)
            {                
                MessageTextView.Text = cell.Body;
                MessageIconView.Text = cell.Icon;
            }
        }

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
            if (e.PropertyName == MessageViewCell.IconProperty.PropertyName)
            {
                cell.MessageIconView.Text = nativeCell.Icon;
            }
        }
    }

    
}