﻿using FAQPhone.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FAQPhone.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivatePage : ContentPage
    {
        public ActivatePage()
        {
            InitializeComponent();
            BindingContext = new ActivateViewModel(Navigation);
        }
    }
}
