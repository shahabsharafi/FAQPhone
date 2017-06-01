using FAQPhone.Infrastructure;
using FAQPhone.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FAQPhone.ViewModels
{
    public class SignupViewModel : INotifyPropertyChanged
    {
        public SignupViewModel()
        {
            this.SignupCommand = new Command((nothing) =>
            {
                // Add the key to the input string.
                //this.InputString += key;
            });
        }
        string _username;
        public string username {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
        string _password;
        public string password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }
        string _confirm;
        public string confirm
        {
            get { return _confirm; }
            set { _confirm = value; OnPropertyChanged(); }
        }
        string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        string _mobile;
        public string mobile
        {
            get { return _mobile; }
            set { _mobile = value; OnPropertyChanged(); }
        }

        public TextAlignment Direction { get { return ResourceManagerHelper.Direction; } }
        public ICommand SignupCommand { protected set; get; }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
