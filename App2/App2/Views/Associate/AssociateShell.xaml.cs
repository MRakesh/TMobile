using App2.Views.General;
using App2.Views.Routes;

using Xamarin.Forms;

namespace App2.Views.Associate
{
    public partial class AssociateShell : Xamarin.Forms.Shell
    {
        public AssociateShell()
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
        
            //Routing for Associates
            Routing.RegisterRoute("AssociateHome", typeof(AssociateHome));
            Routing.RegisterRoute("AssociateProfileInfo", typeof(AssociateProfileInfo));
        }
    }
}
