using System;

using Xamarin.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using FAQPhone.Infrastructure;
using System.Collections.ObjectModel;

namespace Singleselect
{
	/* 
	* based on
	* https://gist.github.com/glennstephens/76e7e347ca6c19d4ef15
	*/

	public class SelectSingleBasePage<T> : ContentPage
    {
		public class WrappedItemSelectionTemplate : ViewCell
		{
			public WrappedItemSelectionTemplate(bool hasIcon, bool rtl) : base ()
			{
                RelativeLayout layout = new RelativeLayout();
                int rm = 5;
                if (hasIcon)
                {
                    Label icon = new Label();
                    icon.SetBinding(Label.TextProperty, new Binding("Icon"));
                    icon.FontFamily = "fontawesome";
                    icon.FontSize = 20;

                    layout.Children.Add(icon,
                        rtl ? Constraint.RelativeToParent(p => p.Width - 30) : Constraint.Constant(5),
                        Constraint.Constant(5),
                        Constraint.RelativeToParent(p => 25),
                        Constraint.RelativeToParent(p => p.Height - 10)
                    );
                    rm += 30;
                }
				
                Label name = new Label();
                name.HorizontalTextAlignment = rtl ? TextAlignment.End : TextAlignment.Start;
                name.SetBinding(Label.TextProperty, new Binding("Name"));               
				
				layout.Children.Add (name,
                    rtl ? Constraint.Constant(5) : Constraint.Constant(rm),
                    Constraint.Constant (5),
                    Constraint.RelativeToParent(p => p.Width - (hasIcon ? 35 : 10)),
					Constraint.RelativeToParent (p => p.Height - 10)
				);
				View = layout;
			}
		}
        public event EventHandler Select;
        public SingleSelectViewModel<T> ViewModel;
        public IList<T> WrappedItems
        {
            get
            {
                return ViewModel.WrappedItems;
            }
            set
            {
                ViewModel.WrappedItems.Clear();
                foreach(var item in value)
                {
                    ViewModel.WrappedItems.Add(item);
                }
            }
        }
		public SelectSingleBasePage(IList<T> items, bool hasIcon, bool rtl)
		{
            ViewModel = new SingleSelectViewModel<T>();
            
            WrappedItems = items;
            ListView mainList = new ListView () { 
				ItemTemplate = new DataTemplate (() => { return new WrappedItemSelectionTemplate(hasIcon, rtl); }),
			};
            mainList.SetBinding(ListView.ItemsSourceProperty, new Binding("WrappedItems"));
			mainList.ItemSelected += (sender, e) => {
				if (e.SelectedItem == null) return;
                this.SelectedItem = (T)e.SelectedItem;
                Select?.Invoke(this, new EventArgs());
            };
            StackLayout stack = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
            };
            stack.Children.Add(mainList);
			Content = stack;
            
            Content.BindingContext = ViewModel;

        }

        public T SelectedItem { get; private set; }
	}
}


