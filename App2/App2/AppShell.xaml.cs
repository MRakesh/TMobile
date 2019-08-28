using App2.Views;
using App2.Views.Account;
using App2.Views.Associate;
using App2.Views.General;
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

            ShellItem item = new ShellItem();
           
            ShellSection shell_section = new ShellSection
            {
                Title = "Shell Register Page",
            };
            ShellSection shell_section1 = new ShellSection
            {
                Title = "Shell About",
            };

            shell_section.Items.Add(new ShellContent() { Content = new Register() });
            shell_section1.Items.Add(new ShellContent() { Content = new AboutPage() });

            item.Items.Add(shell_section);
            item.Items.Add(shell_section1);

            item.Title = "Shell Parent Dynamic Page!";
            item.FlyoutIcon = "bars";

            ParentShell.Items.Add(item);
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

            //Default Page
            Routing.RegisterRoute("Login", typeof(Login));
            Routing.RegisterRoute("Register", typeof(Register));
            Routing.RegisterRoute("CompanyRegister", typeof(Company));
            Routing.RegisterRoute("EmployerRegister", typeof(Employer));

            //Routing for Associates
            Routing.RegisterRoute("AssociateHome", typeof(AssociateHome));
            Routing.RegisterRoute("AssociateProfileInfo", typeof(AssociateProfileInfo));

            //Routing for Recruiters
            Routing.RegisterRoute("ContactHome", typeof(Views.Contact.ContactHome));
        }
    }
}
