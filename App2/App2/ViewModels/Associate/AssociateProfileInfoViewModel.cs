using App2.Dtos;
using App2.Models;
using App2.Services;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App2.ViewModels
{
    public class AssociateProfileInfoViewModel : BaseViewModel
    {
        private Page _page;
        public ICommand SetTextCommand { get; }
        public IAccountService _accountService => DependencyService.Get<IAccountService>() ?? new AccountService();
        public AssociateProfileInfoViewModel(Page page)
        {
            _page = page;
            this.ProfileInfo = new AssociateViewProfileDto() { Email = "testogemai" };

            SetTextCommand = new Command(() => SetText());
        }


        void SetText()
        {
            ProfileInfo = new AssociateViewProfileDto() { Email = "new EMailtest" };
            Email = "33 age";
        }


        private string email;
        public string Email { get { return email; } set { if (email != value) { email = value; OnPropertyChanged(); } } }


        private AssociateViewProfileDto profileInfo;
        public AssociateViewProfileDto ProfileInfo
        {
            get { return profileInfo; }
            set
            {
                profileInfo = value;
                SetProperty(ref profileInfo, value);
                //OnPropertyChanged("ProfileInfo");                
            }
        }

        void setTestData()
        {
            ProfileInfo = new AssociateViewProfileDto {  Email = "new email content" };
            Email = "New age 66";

        }

        public async Task FillProfileInfo()
        {
            try
            {
                string response = await _accountService.GetCustomResponseFromAPIAsync("api/candidate/getAssociateViewProfile/14");

                var model = JsonConvert.DeserializeObject<AssociateViewProfileDtoBase>(response);
                //ProfileInfo = model.result;
                //Label txtMesage = _page.FindByName<Label>("lblMessage");
                //txtMesage.Text = model.result.Email;
                
                Email = model.result.Email + " " + model.result.Name;
                await _page.DisplayAlert("Status", model.result.Email, "OK");
            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Error Status", ex.Message + " " + ex.StackTrace, "OK");
            }

        }
    }
}
