using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomRenderer
{
    public class MessageViewCell : ViewCell
    {
        /*
        public MessageViewCell()
        {
            //var authorLabel = new Label();
            //authorLabel.SetBinding(Label.TextProperty, new Binding("AuthorName", stringFormat: "{0}: "));
            //authorLabel.TextColor = Device.OnPlatform(Color.Blue, Color.Yellow, Color.Yellow);
            //authorLabel.FontSize = 14;

            var messageLabel = new Label();
            messageLabel.SetBinding(Label.TextProperty, new Binding("text"));
            messageLabel.FontSize = 14;

            var stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                //Children = { authorLabel, messageLabel }
                Children = { messageLabel }

            };

            this.View = stack;
        }
        */
        
        public static readonly BindableProperty BodyProperty = BindableProperty.Create("Body", typeof(string), typeof(MessageViewCell), "");

        public string Body
        {
            get { return (string)GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }

        public static readonly BindableProperty OwnerProperty = BindableProperty.Create("Owner", typeof(string), typeof(MessageViewCell), "");

        public string Owner
        {
            get { return (string)GetValue(OwnerProperty); }
            set { SetValue(OwnerProperty, value); }
        }
    }
}
