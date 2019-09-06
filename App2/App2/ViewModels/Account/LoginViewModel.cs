using App2.Helpers;
using App2.Models;
using App2.Services;
using App2.Views.Associate;
using App2.Views.Contact;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace App2.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public IAccountService _accountService => DependencyService.Get<IAccountService>() ?? new AccountService();
        public string Message { get; set; }
        public LoginModel LoginModel { get; set; } = new LoginModel();
        public ICommand LoginInCommand { get; }
        private Page _page;
        public INavigation Navigation { get; set; }
        public LoginViewModel(Page page, INavigation navigation)
        {
            this.Navigation = navigation;
            _page = page;
            LoginInCommand = new Command(async () => await LoginAsync());

            // string cookie = _accountService.GetPrivateCookie();
            // if(cookie.Length > 0)
            //// if (_accountService.IsCoockiExists())
            //     Shell.Current.GoToAsync(new ShellNavigationState("AssociateHome"), true);
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
                {

                    //Label lblMessage = (Label)_page.FindByName<("lblMesage");
                    //Entry txtMesage = _page.FindByName<Entry>("txtMesage");
                    //txtMesage.Text = response.targetUrl;

                    //string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    //string filename = Path.Combine(path, "coockiefile.txt");

                    //using (var streamWriter = new StreamWriter(filename, true))
                    //{
                    //    streamWriter.WriteLine(response.targetUrl);
                    //}                    

                    //string pathForTheFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DateTime.UtcNow.ToString() +"_" + "coockiefile.txt");
                    //File.WriteAllText(pathForTheFile, response.targetUrl);
                    Entry txtMesage = _page.FindByName<Entry>("txtMesage");
                    txtMesage.Text = response.targetUrl;



                    //  await _page.DisplayAlert("Status", response.targetUrl, "OK");


                    // await NavigatetoHomePage(response);

                    if (response.result.Type.ToLower() == "associate")
                    {
                        Application.Current.MainPage = new AssociateShell();
                        //await Navigation.PushAsync(new NavigationPage(new AssociateShell()));
                    }
                    //   await Shell.Current.GoToAsync(new ShellNavigationState("AssociateHome"), true);
                    else if (response.result.Type.ToLower() == "recruiter")
                    {
                        Application.Current.MainPage = new ContactShell();
                        //await Navigation.PushAsync(new NavigationPage(new ContactShell()));
                    }
                    // await Shell.Current.GoToAsync(new ShellNavigationState("ContactHome"), true);



                }

            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Excvepton Status", ex.Message + " " + ex.StackTrace, "OK");
                //await _page.DisplayAlert("Status", "Wrong Login and Email...", "OK");
            }
        }
    }
}
