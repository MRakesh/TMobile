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
    public partial class LogInPage : ContentPage
    {
        public LogInPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Clicked(object sender, EventArgs e)
        {
            indicator.IsRunning = true;
            System.Threading.Thread.Sleep(3000);
            string user = txtUser.Text;
            string password = txtPsw.Text;
            indicator.IsRunning = false;
            DisplayAlert("User Details", user + " " + password, "ok");
        }
    }
}