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
                Children = { /*authorLabel, */messageLabel }
            };

            this.View = stack;
        }
    }
}
