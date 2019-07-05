using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpressPage : ContentPage
    {
        public ExpressPage()
        {
            InitializeComponent();
        }

        private void BtnContentPage_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync(new ShellNavigationState("loginview"), true);
            // DisplayAlert("Content Page", "This is the Conten page Event", "ok");
        }

    }
}