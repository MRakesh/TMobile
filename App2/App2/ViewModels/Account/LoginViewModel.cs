using App2.Helpers;
using App2.Models;
using App2.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App2.ViewModels
{
    public class LoginViewModel : BaseViewModel
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
            try
            {
                IsBusy = true;
                //  System.Threading.Thread.Sleep(2500);
                if (!ValidationHelper.IsFormValid(LoginModel, _page)) { IsBusy = false; return; }
                var response = await _accountService.GetTokenForLoginAsync(LoginModel);
                //if (!string.IsNullOrEmpty(responce))
                IsBusy = false;
                if (response.result.Message.ToLower().Contains("bad"))
                    await _page.DisplayAlert("Status", "Wrong Login and Email...", "OK");
                else
                 await  NavigatetoHomePage(response);

            }
            catch (Exception ex)
            {
                //await _page.DisplayAlert("Status", ex.Message + " " + ex.StackTrace, "OK");
                await _page.DisplayAlert("Status", "Wrong Login and Email...", "OK");
            }
        }
    }
}
