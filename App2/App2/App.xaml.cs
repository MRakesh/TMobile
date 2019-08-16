using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App2.Services;
using App2.Views;

namespace App2
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
//#if DEBUG
//            HotReloader.Current.Run(this);
//#endif

            //services registrations
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<AccountService>();

            
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
