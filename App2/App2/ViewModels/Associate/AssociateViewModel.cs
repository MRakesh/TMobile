using App2.Models;
using App2.Services;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App2.ViewModels
{
    public class AssociateViewModel
    {
        private Page _page;
        public IAccountService _accountService => DependencyService.Get<IAccountService>() ?? new AccountService();
        public AssociateViewModel(Page page)
        {
            _page = page;
            GetChatsListCommand = new Command(async () => await GetChatsList());
        }
        async Task GetChatsList()
        {
            string response = await _accountService.GetResponseFromAPIAsync("api/candidate/getAssociateTopicPopUpChats/171");
            string oldres = string.Empty;
            var model = JsonConvert.DeserializeObject<RecruiterTopicPopUpBaseDto>(response);
            foreach (var item in model.result)
            {
                if (item != null)
                    oldres += item.Message;
            }
            await _page.DisplayAlert("Status", oldres, "OK");
        }
        public ICommand GetChatsListCommand { get; }

    }
}
