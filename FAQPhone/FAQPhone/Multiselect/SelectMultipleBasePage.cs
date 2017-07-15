using System;

using Xamarin.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using FAQPhone.Infrastructure;

namespace Multiselect
{
	/* 
	* based on
	* https://gist.github.com/glennstephens/76e7e347ca6c19d4ef15
	*/

	public class SelectMultipleBasePage<T> : ContentPage
	{
		public class WrappedSelection<T> : INotifyPropertyChanged
		{
			public T Item { get; set; }
			bool isSelected = false;
			public bool IsSelected { 
				get {
					return isSelected;
				}
				set
				{
					if (isSelected != value) {
						isSelected = value;
						PropertyChanged (this, new PropertyChangedEventArgs ("IsSelected"));
//						PropertyChanged (this, new PropertyChangedEventArgs (nameof (IsSelected))); // C# 6
					}
				}
			}
			public event PropertyChangedEventHandler PropertyChanged = delegate {};
		}
		public class WrappedItemSelectionTemplate : ViewCell
		{
			public WrappedItemSelectionTemplate() : base ()
			{
				Label name = new Label();
				name.SetBinding(Label.TextProperty, new Binding("Item.Name"));
				Switch mainSwitch = new Switch();
				mainSwitch.SetBinding(Switch.IsToggledProperty, new Binding("IsSelected"));
				RelativeLayout layout = new RelativeLayout();
				layout.Children.Add (name, 
					Constraint.Constant (5), 
					Constraint.Constant (5),
					Constraint.RelativeToParent (p => p.Width - 60),
					Constraint.RelativeToParent (p => p.Height - 10)
				);
				layout.Children.Add (mainSwitch, 
					Constraint.RelativeToParent (p => p.Width - 55), 
					Constraint.Constant (5),
					Constraint.Constant (50),
					Constraint.RelativeToParent (p => p.Height - 10)
				);
				View = layout;
			}
		}
        public event EventHandler Select;
        public List<WrappedSelection<T>> WrappedItems = new List<WrappedSelection<T>>();
		public SelectMultipleBasePage(List<T> items)
		{
			WrappedItems = items.Select (item => new WrappedSelection<T> () { Item = item, IsSelected = false }).ToList ();
			ListView mainList = new ListView () {
				ItemsSource = WrappedItems,
				ItemTemplate = new DataTemplate (typeof(WrappedItemSelectionTemplate)),
			};
            
			mainList.ItemSelected += (sender, e) => {
				if (e.SelectedItem == null) return;
				var o = (WrappedSelection<T>)e.SelectedItem;
				o.IsSelected = !o.IsSelected;
				((ListView)sender).SelectedItem = null; //de-select
			};
            Button btn = new Button() {
                Text = ResourceManagerHelper.GetValue("discussion_select_tags"),
                StyleClass = new List<string>() { "Info" },                
            };
            btn.Clicked += (sender, e) => {
                Select?.Invoke(this, new EventArgs());
                Navigation.PopAsync();
            }; 
            StackLayout stack = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
            };
            stack.Children.Add(mainList);
            stack.Children.Add(btn);
			Content = stack;
            string selectAllCaption = ResourceManagerHelper.GetValue("command_select_all");
            string selectNoneCaption = ResourceManagerHelper.GetValue("command_select_none");
            if (Device.OS == TargetPlatform.Windows)
            {   // fix issue where rows are badly sized (as tall as the screen) on WinPhone8.1
                mainList.RowHeight = 40;
                // also need icons for Windows app bar (other platforms can just use text)
                ToolbarItems.Add(new ToolbarItem(selectAllCaption, "check.png", SelectAll, ToolbarItemOrder.Primary));
                ToolbarItems.Add(new ToolbarItem(selectNoneCaption, "cancel.png", SelectNone, ToolbarItemOrder.Primary));
            }
            else
            {
                ToolbarItems.Add(new ToolbarItem(selectAllCaption, null, SelectAll, ToolbarItemOrder.Primary));
                ToolbarItems.Add(new ToolbarItem(selectNoneCaption, null, SelectNone, ToolbarItemOrder.Primary));
            }
		}

        void SelectAll ()
		{
			foreach (var wi in WrappedItems) {
				wi.IsSelected = true;
			}
		}
		void SelectNone ()
		{
			foreach (var wi in WrappedItems) {
				wi.IsSelected = false;
			}
		}
		public List<T> GetSelection() 
		{
			return WrappedItems.Where (item => item.IsSelected).Select (wrappedItem => wrappedItem.Item).ToList ();	
		}
	}
}


