using App2.Services;
using App2.Views.Account;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App2.ViewModels
{
    public class DefaultViewModel
    {
        public IAccountService _accountService => DependencyService.Get<IAccountService>() ?? new AccountService();

        public ICommand LoginInCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand CompanyCommand { get; }
        private Page _page;
        public INavigation Navigation { get; set; }
        public DefaultViewModel(Page page, INavigation navigation)
        {
            _page = page;
            this.Navigation = navigation;

            LoginInCommand = new Command(async () => await LoginAsync());
            RegisterCommand = new Command(async () => await RegisterAsync());
            CompanyCommand = new Command(async () => await CompanyAsync());
        }

        private async Task LoginAsync()
        {
            //_page.DisplayAlert("Statuus", "Thsi sthe message", "ok");
           // await Navigation.PushAsync( new NavigationPage(new Login()));
            //_page.Navigation.PushAsync();
            await Shell.Current.GoToAsync(new ShellNavigationState("App2.Views.Account/Login"), true);
        }
        private async Task RegisterAsync()
        {
            await Shell.Current.GoToAsync(new ShellNavigationState("Register"), true);
        }
        private async Task CompanyAsync()
        {
            await Shell.Current.GoToAsync(new ShellNavigationState("CompanyRegister"), true);
        }
    }
}
