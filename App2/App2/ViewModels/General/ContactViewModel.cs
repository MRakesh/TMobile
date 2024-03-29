﻿using App2.Associates.Dtos;
using App2.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App2.ViewModels
{
    public class ContactViewModel: BaseViewModel
    {
        private Page _page;
        public ICommand GetChatsListCommand { get; }
        //public ICommand TextChangedCommand { get; }
        public IAccountService _accountService => DependencyService.Get<IAccountService>() ?? new AccountService();
        public ContactViewModel(Page page, string queryparam1= null, string queryParam2 = null)
        {
            _page = page;
            GetChatsListCommand = new Command(async () => await GetChatsList());
            //TextChangedCommand = new Command(TextChangedList());
            if (!string.IsNullOrEmpty(queryparam1))
                Age = queryparam1;

            Task.Run(() => GetChatsList());

           // this.ProfileInfo = new AssociateViewProfileDto() { Email = "32323" };
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

        private string email;
        public string Email { get { return email; } set { if (email != value) { email = value; OnPropertyChanged(); } } }


        private string age_;
        public string Age { get { return age_; } set { if (age_ != value) { age_ = ProcessAge(value); OnPropertyChanged(); } } }

        private string ProcessAge(string age)
        {
            Email = "Sopme nte Emai";
            if (string.IsNullOrEmpty(age))
                return age;

            if (age.Length > 3)
                age = age.Substring(0, 3);

            if (age.StartsWith("0"))
                age = age.Remove(0, 1);

            return age.Replace(".", "").Replace("-", "");
        }

        //void TextChangedList()
        //{
        //    _page.DisplayAlert("Error Status",  ,"OK");
        //}
        public async Task GetChatsList()
        {
            try
            {
                string response = await _accountService.GetResponseFromAPIAsync("api/candidate/getAssociateTopicPopUpChats/222");
                this.ProfileInfo = new AssociateViewProfileDto { Email = response };

                //Label txtMesage = _page.FindByName<Label>("lblError");
                //txtMesage.Text = "some error on the code";

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
