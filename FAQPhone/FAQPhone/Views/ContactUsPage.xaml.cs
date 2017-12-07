using FAQPhone.Infarstructure;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FAQPhone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactUsPage : ContentPage
    {
        public ContactUsPage()
        {
            InitializeComponent();
            var factory = App.Resolve<ContactUsFactory>();
            BindingContext = factory.Create(this);
        }
    }
    
    public class ContactUsFactory
    {
        IEmailService emailService;
        public ContactUsFactory(IEmailService emailService)
        {
            this.emailService = emailService;
        }
        public ContactUsViewModel Create(ContentPage page)
        {
            return new ContactUsViewModel(this.emailService, page);
        }
    }

    public class ContactUsViewModel : BaseViewModel
    {

        public ContactUsViewModel(IEmailService emailService, ContentPage page) : base(page)
        {
            this.emailService = emailService;
            this.SendCommand = new Command(async () => await sendCommand());
        }
        private IEmailService emailService { get; set; }

        string _subject;
        public string subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                OnPropertyChanged();
            }
        }

        string _text;
        public string text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public ICommand SendCommand { protected set; get; }

        public async Task sendCommand()
        {
            await this.emailService.Send(new Models.EmailModel()
            {
                subject = this.subject,
                text = this.text
            });
            await this.Navigation.PopAsync();
        }
    }
    
}