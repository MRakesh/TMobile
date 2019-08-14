using App2.Helpers;
using App2.Models;
using App2.Services;
using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Linq;
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
        public ICommand FileUploadCommand { get; }
        private Page _page;

        public RegisterViewModel(Page page)
        {
            _page = page;
            RegisterCommand = new Command(async () => await RegisterAsync());
            FileUploadCommand = new Command(async () => await FileUploadAsync());

            if (_accountService.IsCoockiExists())
                Shell.Current.GoToAsync(new ShellNavigationState("AssociateHome"), true);
        }

        private async Task FileUploadAsync()
        {
            IsBusy = true;
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await _page.DisplayAlert("Warning", ":( Permission not granted to photos.", "OK");
                return;
            }
            var _mediaFile = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();
            if (_mediaFile == null)
                return;

            //image.Source = ImageSource.FromStream(() =>
            //{
            //    var stream = _mediaFile.GetStream();
            //    //  file.Dispose();
            //    return stream;
            //});

            string fileEXtension = System.IO.Path.GetExtension(_mediaFile.Path);

            var fileFormats = UtilityService.GetFileFormats();
            if (fileFormats.Any(f => f == fileEXtension))
            {
                await _accountService.PostFileDataToAPIAsync(_mediaFile);
                IsBusy = false;
            }
            else
            {
                await _page.DisplayAlert("Warning", "Upload file in format " + string.Join(", ", fileFormats), "Ok");
                IsBusy = false;
            }
        }
        private async Task RegisterAsync()
        {
            try
            {
                IsBusy = true;
                if (!ValidationHelper.IsFormValid(Model, _page)) { IsBusy = false; return; }
                if (Application.Current.Properties.ContainsKey("MsGUID")) //!string.IsNullOrEmpty(Application.Current.Properties["MsGUID"].ToString()) &&
                {
                    Model.GUID = Application.Current.Properties["MsGUID"].ToString();
                    Model.FileName = Application.Current.Properties["MsFileName"].ToString();
                }

                Model.PortalId = PortalId;
                Model.CompanyId = PortalId;

                var response = await _accountService.PostDataToAPIAsync(Model, "/api/candidateAccount/candidateregistration");
                //if (!string.IsNullOrEmpty(responce))
                IsBusy = false;
                if (response.Contains("automatic"))
                {
                    Application.Current.Properties.Remove("MsGUID");
                    Application.Current.Properties.Remove("MsFileName");
                    var regModel = JsonConvert.DeserializeObject<RegisterAutomateModelBase>(response);
                    if (regModel.result.automatic)
                    {
                        //calling login method.
                        var loginResponse = await _accountService.GetTokenForLoginAsync(new LoginModel { Username = Model.Email, Password = Model.Password, RememberMe = false });
                        IsBusy = false;
                        if (loginResponse.result.Message.ToLower().Contains("bad"))
                            await _page.DisplayAlert("Status", "Wrong Login and Email...", "OK");
                        else
                            await NavigatetoHomePage(loginResponse);
                    }
                }
                else
                {
                    await _page.DisplayAlert("Status", response, "OK");
                }

            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Status", ex.Message + " " + ex.StackTrace, "OK");
                //await _page.DisplayAlert("Status", "Wrong Login and Email...", "OK");
            }
        }

    }
}
