using App2.Views.Associate;
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
    public partial class FlypoutContentHeader : ContentView
    {
        public FlypoutContentHeader()
        {
            InitializeComponent();

            //lblUserName.Text = "User Exists Give Unique Name";
        }

        private void BtnHomePage_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new AppShell();
        }

        private void BtnDashboard_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new AssociateShell();
        }
    }
}