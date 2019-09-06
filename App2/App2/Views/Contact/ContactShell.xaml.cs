using App2.Views.General;
using App2.Views.Routes;

using Xamarin.Forms;

namespace App2.Views.Contact
{
    public partial class ContactShell : Xamarin.Forms.Shell
    {
        public ContactShell()
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
            Routing.RegisterRoute("contactessential", typeof(ContactEssentials));
            Routing.RegisterRoute("tabContent", typeof(TabContent));


            //Routing for Recruiters
            Routing.RegisterRoute("ContactHome", typeof(Views.Contact.ContactHome));
        }
    }
}
