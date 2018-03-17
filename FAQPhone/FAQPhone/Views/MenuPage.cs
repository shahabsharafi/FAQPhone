using Awesome;
using CustomRenderer;
using FAQPhone.Helpers;
using FAQPhone.Infarstructure;
using FAQPhone.Models;
using ScnSideMenu.Forms;
using Singleselect;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FAQPhone.Views
{
    public class MenuPage : SideBarPage
    {
        public class MenuPageViewModel
        {
            public MenuPageViewModel()
            {
                _MainMenu = new ObservableCollection<MenuItemModel>();
                _RightMenu = new ObservableCollection<MenuItemModel>();
            }
            private ObservableCollection<MenuItemModel> _MainMenu { get; set; }
            public IList<MenuItemModel> MainMenu
            {
                get
                {
                    return _MainMenu;
                }
                set
                {
                    _MainMenu.Clear();
                    foreach (var item in value)
                    {
                        _MainMenu.Add(item);
                    }
                }
            }

            public IList<MenuItemModel> RightMenu
            {
                get
                {
                    return _RightMenu;
                }
                set
                {
                    _RightMenu.Clear();
                    foreach (var item in value)
                    {
                        _RightMenu.Add(item);
                    }
                }
            }

            public ObservableCollection<MenuItemModel> _RightMenu { get; set; }
        }
        public class WrappedMenuItemTemplate : ViewCell
        {
            public WrappedMenuItemTemplate() : this(Color.FromHex("#4e5758")) { }
            public WrappedMenuItemTemplate(Color color) : base()
            {
                Label lblIcone = new Label() {
                    FontFamily = "fontawesome",
                    FontSize = 20,
                    TextColor = color,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(5,10,5,0),
                    
                };
                lblIcone.SetBinding(Label.TextProperty, new Binding("Icon"));

                Label lbl = new Label()
                {
                    FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                    FontAttributes = FontAttributes.Bold,
                    TextColor = color,
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
            FillContext();
            InitToolbar();
            InitRightMenu();
            InitMainMenu();
        }

        private void InitMainMenu()
        {
            ListView list = new ListView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                ItemTemplate = new DataTemplate(() => { return new WrappedMenuItemTemplate(); }),
            };
            list.SetBinding(ListView.ItemsSourceProperty, new Binding("MainMenu"));
            list.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem == null) return;
                //this.SelectedItem = (T)e.SelectedItem;
                //Select?.Invoke(this, new EventArgs());
            };
            AbsoluteLayout layout = new AbsoluteLayout();
            layout.Children.Add(list, new Rectangle(0, 1, 1, 1), AbsoluteLayoutFlags.All);
            ContentLayout.Children.Add(layout);
        }

        private void InitRightMenu()
        {
            ListView list = new ListView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                ItemTemplate = new DataTemplate(() => { return new WrappedMenuItemTemplate(Color.FromHex("#fff")); }),
            };
            list.SetBinding(ListView.ItemsSourceProperty, new Binding("RightMenu"));
            list.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem == null) return;
                //this.SelectedItem = (T)e.SelectedItem;
                //Select?.Invoke(this, new EventArgs());
            };
            list.BackgroundColor = Color.FromHex("#24292e");            
            AbsoluteLayout layout = new AbsoluteLayout();
            layout.Children.Add(list, new Rectangle(0, 1, 1, 1), AbsoluteLayoutFlags.All);
                       
            RightPanelWidth = 300;
            RightPanel.AddToContext(layout);            
        }

        private void InitToolbar()
        {
            ToolbarItems.Add(new Xamarin.Forms.ToolbarItem()
            {
                Icon = Utility.GetImage("rule40"),
                Priority = 0,
                Order = Xamarin.Forms.ToolbarItemOrder.Primary,
                Command = new Command(async () => await showMenu())
            });
        }

        private void FillContext()
        {
            var mainList = new List<MenuItemModel>();
            mainList.Add(new MenuItemModel() { CommandName = Constants.OPERATOR_RECEIVE_FAQ, Icon = FontAwesome.FADownload });
            mainList.Add(new MenuItemModel() { CommandName = Constants.OPERATOR_INPROGRESS_FAQ, Icon = FontAwesome.FATasks, Badge = "4" });

            var rightList = new List<MenuItemModel>();
            rightList.Add(new MenuItemModel() { CommandName = Constants.REPORT_QUICK, Icon = FontAwesome.FAList, Badge = "2" });
            rightList.Add(new MenuItemModel() { CommandName = Constants.REPORT_BALANCE, Icon = FontAwesome.FAListAlt });

            var ViewModel = new MenuPageViewModel()
            {
                MainMenu = mainList,
                RightMenu = rightList
            };
            Content.BindingContext = ViewModel;
        }

        public ICommand ShowMenu { protected set; get; }
        public async Task showMenu()
        {
            IsShowRightPanel = !IsShowRightPanel;
        }     
    }
}
