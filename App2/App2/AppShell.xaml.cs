using App2.Views;
using App2.Views.Associate;
using App2.Views.Contact;
using App2.Views.Routes;

using Xamarin.Forms;

namespace App2
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {

            Routing.RegisterRoute("routerOne", typeof(RouterOne));
            Routing.RegisterRoute("routerTwo", typeof(RouterTwo));
            Routing.RegisterRoute("routerThree", typeof(RouterThree));
            Routing.RegisterRoute("loginview", typeof(LogInPage));
            Routing.RegisterRoute("fileload", typeof(Fileload));

            //Routing for Associates
            Routing.RegisterRoute("AssociateHome", typeof(AssociateHome));
            //Routing for Recruiters
            Routing.RegisterRoute("ContactHome", typeof(ContactHome));
        }
    }
}
