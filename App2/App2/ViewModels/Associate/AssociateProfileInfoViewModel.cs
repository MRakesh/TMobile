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

        //private string workAuth;
        //public string WorkAuth
        //{
        //    get { return workAuth; }
        //    set { workAuth = value; OnPropertyChanged(); }
        //}

        public AssociateProfileInfoViewModel(Page page)
        {
            _page = page;
            IsBusy = true;
            Task.Run(() => FillProfileInfo());
        }

        private AssociateViewProfileDto profileInfo;
        public AssociateViewProfileDto ProfileInfo
        {
            get { return profileInfo; }
            set
            {
                profileInfo = value;
                OnPropertyChanged();                
            }
        }
      
        public async Task FillProfileInfo()
        {
            try
            {
                string response = await _accountService.GetResponseFromAPIAsync("api/candidate/getAssociateViewProfile/14");

                var model = JsonConvert.DeserializeObject<AssociateViewProfileDtoBase>(response);
                ProfileInfo = model.result;
               
                IsBusy = false;
                //Label txtMesage = _page.FindByName<Label>("lblMessage");
                //txtMesage.Text = model.result.Email;

                //await _page.DisplayAlert("Status", model.result.Email, "OK");
            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Error Status", ex.Message + " " + ex.StackTrace, "OK");
            }

        }
    }
}
