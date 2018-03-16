using Awesome;
using CustomRenderer;
using FAQPhone.Helpers;
using FAQPhone.Infarstructure;
using FAQPhone.Models;
using ScnSideMenu.Forms;
using Singleselect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FAQPhone.Views
{
    public class MenuPage : SideBarPage
    {
        public class MenuItemViewModel : SingleSelectViewModel<MenuItemModel> { }
        public class WrappedMenuItemTemplate : ViewCell
        {
            public WrappedMenuItemTemplate() : base()
            {
                Label lblIcone = new Label() {
                    FontFamily = "fontawesome",
                    FontSize = 20,
                    TextColor = Color.FromHex("#4e5758"),
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(5,10,5,0),
                    
                };
                lblIcone.SetBinding(Label.TextProperty, new Binding("Icon"));

                Label lbl = new Label()
                {
                    FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromHex("#4e5758"),
                    Margin = new Thickness(0, 10, 0, 0),

                };
                lbl.SetBinding(Label.TextProperty, new Binding("DisplayName"));

                BadgeLabel lblBadge = new BadgeLabel()
                {
                    Margin = new Thickness(5, 5, 5, 5)
                };
                lblBadge.SetBinding(Label.TextProperty, new Binding("Badge"));
                lblBadge.SetBinding(Label.IsVisibleProperty, new Binding("ShowBadge"));

                Grid grid = new Grid() {
                    RowDefinitions = new RowDefinitionCollection()
                    {
                        new RowDefinition() { Height = GridLength.Auto }
                    },
                    ColumnDefinitions = new ColumnDefinitionCollection()
                    {
                        new ColumnDefinition() { Width = GridLength.Auto },
                        new ColumnDefinition() { Width = GridLength.Star },
                        new ColumnDefinition() { Width = GridLength.Auto }
                    }
                };

                grid.Children.Add(lblIcone, 2, 0);
                grid.Children.Add(lbl, 1, 0);
                grid.Children.Add(lblBadge, 0, 0);

                View = grid;
            }
        }
        public MenuPage() : base(PanelSetEnum.psRight)
        {
            ViewModel = new MenuItemViewModel();

            var list = new List<MenuItemModel>();
            list.Add(new MenuItemModel() { CommandName = Constants.OPERATOR_RECEIVE_FAQ, Icon = FontAwesome.FADownload });
            list.Add(new MenuItemModel() { CommandName = Constants.OPERATOR_INPROGRESS_FAQ, Icon = FontAwesome.FATasks, Badge = "4" });
            WrappedItems = list;

            ToolbarItems.Add(new Xamarin.Forms.ToolbarItem()
            {
                Icon = Utility.GetImage("rule40"),
                Priority = 0,
                Order = Xamarin.Forms.ToolbarItemOrder.Primary,
                Command = new Command(async () => await showMenu())
            });

            RightPanelWidth = 150;

            RightPanel.AddToContext(new StackLayout
            {
                Padding = new Thickness(32),
                Children =
                {
                    new Label
                    {
                        Text = "right menu",
                        TextColor = Color.Red,
                    }
                }
            });

            RightPanel.BackgroundColor = Color.Blue;

            ContentLayout.Children.Add(new AbsoluteLayout()
            {
                Children = {
                    new ListView() {
                        ItemTemplate = new DataTemplate(() => { return new WrappedMenuItemTemplate(); })
                    }
                }
            });

            ListView mainList = new ListView()
            {
                ItemTemplate = new DataTemplate(() => { return new WrappedMenuItemTemplate(); }),
            };
            mainList.SetBinding(ListView.ItemsSourceProperty, new Binding("WrappedItems"));
            mainList.ItemSelected += (sender, e) => {
                if (e.SelectedItem == null) return;
                //this.SelectedItem = (T)e.SelectedItem;
                //Select?.Invoke(this, new EventArgs());
            };
            StackLayout stack = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
            };
            stack.Children.Add(mainList);
            ContentLayout.Children.Add(stack);

            Content.BindingContext = ViewModel;
        }
        public ICommand ShowMenu { protected set; get; }
        public async Task showMenu()
        {
            IsShowRightPanel = !IsShowRightPanel;
        }
        public MenuItemViewModel ViewModel;
        public IList<MenuItemModel> WrappedItems
        {
            get
            {
                return ViewModel.WrappedItems;
            }
            set
            {
                ViewModel.WrappedItems.Clear();
                foreach (var item in value)
                {
                    ViewModel.WrappedItems.Add(item);
                }
            }
        }
    }
}
