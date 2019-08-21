using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using App2.Models;
using Xamarin.Essentials;
using App2.Services;
using System.Windows.Input;

namespace App2.ViewModels
{
    public class ContactViewModel
    {
        private Page _page;
        public ICommand GetChatsListCommand { get; }
        public IAccountService _accountService => DependencyService.Get<IAccountService>() ?? new AccountService();
        public ContactViewModel(Page page)
        {
            _page = page;
            GetChatsListCommand = new Command(async () => await GetChatsList());
        }

        async Task GetChatsList()
        {
            try
            {
                string response = await _accountService.GetCustomResponseFromAPIAsync("api/recruiter/getRecruiterTopicPopUpChats/194");
                await _page.DisplayAlert("Status", response, "OK");
            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Error Status", ex.Message + " " + ex.StackTrace, "OK");
            }
        }

        //public Command SmsCommand { get; }
        //public Command EmailCommand { get; }
        //public Command PhoneCommand { get; }
        //public Command NavigateCommand { get; }

        //public ContactEssential Contact { get; }

        //public ContactViewModel(ContactEssential contact)
        //{
        //    Contact = contact;

        //}

        //public ContactViewModel(Page page)
        //{

        //    Contact = new ContactEssential
        //    {
        //        Name = "James Montemagno",
        //        Address = "Microsoft Building 18",
        //        City = "Redmond",
        //        State = "WA",
        //        ZipCode = "98052",
        //        Email = "rakesh.maggidi@itghar.com",
        //        PhoneNumber = "555-555-5555"
        //    };

        //    SmsCommand = new Command(async () => await ExecuteSmsCommand());
        //    EmailCommand = new Command(async () => await ExecuteEmailCommand());
        //    PhoneCommand = new Command(ExecutePhoneCommand);
        //    NavigateCommand = new Command(async () => await ExecuteNavigateCommand());


        // }

        //async Task ExecuteSmsCommand()
        //{
        //    try
        //    {
        //        await Sms.ComposeAsync(new SmsMessage(string.Empty, Contact.PhoneNumber));
        //    }
        //    catch (Exception ex)
        //    {
        //        ProcessException(ex);
        //    }
        //}

        //async Task ExecuteEmailCommand()
        //{
        //    try
        //    {
        //        await Email.ComposeAsync(string.Empty, string.Empty, Contact.Email);
        //    }
        //    catch (Exception ex)
        //    {
        //        ProcessException(ex);
        //    }
        //}

        //void ExecutePhoneCommand()
        //{
        //    try
        //    {
        //        PhoneDialer.Open(Contact.PhoneNumber);
        //    }
        //    catch (Exception ex)
        //    {
        //        ProcessException(ex);
        //    }
        //}

        //async Task ExecuteNavigateCommand()
        //{
        //    try
        //    {
        //        await Map.OpenAsync(new Placemark
        //        {
        //            Thoroughfare = Contact.Address,
        //            Locality = Contact.City,
        //            AdminArea = Contact.State,
        //            PostalCode = Contact.ZipCode
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        ProcessException(ex);
        //    }
        //}

        //void ProcessException(Exception ex)
        //{
        //    if (ex != null)
        //        Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        //}
    }
}
