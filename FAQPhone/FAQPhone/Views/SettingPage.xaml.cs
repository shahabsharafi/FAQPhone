using FAQPhone.Helpers;
using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
using FAQPhone.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FAQPhone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();
            var factory = App.Resolve<SettingViewModelFactory>();
            BindingContext = factory.Create(this);
        }
    }

    public class SettingViewModelFactory
    {
        public SettingViewModelFactory()
        {
        }
        public SettingViewModel Create(ContentPage page)
        {
            return new SettingViewModel(page);
        }
    }

    public class SettingViewModel : BaseViewModel
    {
        public SettingViewModel(ContentPage page) : base(page)
        {
            this.SaveCommand = new Command(async () => await saveCommand());
            var languageList = new string[] { "Fa", "Ar" };
            this.LanguageList = new ObservableCollection<AttributeModel>();
            foreach (var item in languageList)
            {
                string key = string.Format("setting_language_{0}", item.ToLower());
                string val = ResourceManagerHelper.GetValue(key);
                this.LanguageList.Add(new AttributeModel() { caption = val, _id = key });
            }
        }

        private ObservableCollection<AttributeModel> _LanguageList;
        public ObservableCollection<AttributeModel> LanguageList
        {
            get { return _LanguageList; }
            set { _LanguageList = value; }
        }

        private AttributeModel _SelectedLanguage;
        public AttributeModel SelectedLanguage
        {
            get { return _SelectedLanguage; }
            set
            {
                _SelectedLanguage = value;
                OnPropertyChanged();
            }
        }
        public ICommand SaveCommand { protected set; get; }

        public async Task saveCommand()
        {
            /////
            if (!string.IsNullOrEmpty(this.SelectedLanguage?._id))
            {
                Settings.Language = this.SelectedLanguage._id;
            }
            await this.Navigation.PopAsync();
        }
    }
}
