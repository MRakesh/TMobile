using App2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Default : ContentPage
    {
        public Default()
        {
            InitializeComponent();
            BindingContext = new DefaultViewModel(this, Navigation);

            //var tgr = new TapGestureRecognizer();
            //tgr.SetBinding<DefaultViewModel>(TapGestureRecognizer.CommandProperty, ViewModel => ViewModel.LoginInCommand);
        }


    }
}