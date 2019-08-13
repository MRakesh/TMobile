using App2.Helpers;
using App2.Models;
using App2.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App2.ViewModels
{
    public class LoginViewModel
    {
        public IAccountService _accountService => DependencyService.Get<IAccountService>() ?? new AccountService();
        public LoginModel LoginModel { get; set; } = new LoginModel();
        public ICommand LoginInCommand { get; }
        private Page _page;

        public LoginViewModel(Page page)
        {
            _page = page;
            LoginInCommand = new Command(async () => await LoginAsync());

            if (_accountService.IsCoockiExists())
                Shell.Current.GoToAsync(new ShellNavigationState("AssociateHome"), true);
        }

        private async Task LoginAsync()
        {
            if (!ValidationHelper.IsFormValid(LoginModel, _page)) { return; }
            string responce = await _accountService.GetTokenForLogin(LoginModel);
            await _page.DisplayAlert("Status", responce, "OK");
            await Shell.Current.GoToAsync(new ShellNavigationState("AssociateHome"), true);
        }
    }
}
