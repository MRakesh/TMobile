using App2.Helpers;
using App2.Models;
using App2.Services;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App2.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public IAccountService _accountService => DependencyService.Get<IAccountService>() ?? new AccountService();
        public RegisterModel Model { get; set; } = new RegisterModel();
        public ICommand RegisterCommand { get; }
        private Page _page;

        public RegisterViewModel(Page page)
        {
            //_page = page;
            //RegisterCommand = new Command(async () => await RegisterAsync());
            
            //if (_accountService.IsCoockiExists())
            //    Shell.Current.GoToAsync(new ShellNavigationState("AssociateHome"), true);
        }

        ////private async Task RegisterAsync()
        ////{
        ////    try
        ////    {
        ////        IsBusy = true;
        ////        if (!ValidationHelper.IsFormValid(Model, _page)) { IsBusy = false; return; }
        ////        var response = await _accountService.PostDataToAPIAsync(Model);
        ////        //if (!string.IsNullOrEmpty(responce))
        ////        IsBusy = false;
        ////        if(response.Contains("automatic"))
        ////        {
        ////            var regModel = JsonConvert.DeserializeObject<RegisterAutomateModelBase>(response);
        ////            if(regModel.result.automatic)
        ////            {
        ////                //calling login method.
        ////            }
        ////        }
        ////        else if(response.Contains(""))
        ////        if (response.result.Message.ToLower().Contains("bad"))
        ////            await _page.DisplayAlert("Status", "Wrong Login and Email...", "OK");
        ////        else
        ////        {
        ////            // await _page.DisplayAlert("Status", response.result.Type, "OK");
        ////            if (response.result.Type.ToLower() == "associate")
        ////                await Shell.Current.GoToAsync(new ShellNavigationState("AssociateHome"), true);
        ////            if (response.result.Type.ToLower() == "recruiter")
        ////                await Shell.Current.GoToAsync(new ShellNavigationState("ContactHome"), true);
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        //await _page.DisplayAlert("Status", ex.Message + " " + ex.StackTrace, "OK");
        ////        await _page.DisplayAlert("Status", "Wrong Login and Email...", "OK");
        ////    }
        ////}

    }
}
