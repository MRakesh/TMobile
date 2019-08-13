using App2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views.Contact
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactHome : ContentPage
    {
        public ContactHome()
        {
            InitializeComponent();
            BindingContext = new RecruiterViewModel(this);
        }
    }
}