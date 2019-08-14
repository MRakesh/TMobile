using App2.Services;
using App2.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        //public IAccountService _accountService => DependencyService.Get<IAccountService>() ?? new AccountService();

        //bool isBusy = false;
        //private MediaFile _mediaFile;
        public Register()
        {
            InitializeComponent();
            this.BindingContext = new RegisterViewModel(this);

            //btnResume.Clicked += async (sender, args) =>
            //{
            //    await CrossMedia.Current.Initialize();
            //    if (!CrossMedia.Current.IsPickPhotoSupported)
            //    {
            //        await DisplayAlert("Warning", ":( Permission not granted to photos.", "OK");
            //        return;
            //    }
            //    var _mediaFile = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();
            //    if (_mediaFile == null)
            //        return;

            //    //image.Source = ImageSource.FromStream(() =>
            //    //{
            //    //    var stream = _mediaFile.GetStream();
            //    //    //  file.Dispose();
            //    //    return stream;
            //    //});

            //    string fileEXtension = System.IO.Path.GetExtension(_mediaFile.Path);
            //    var fileFormats = UtilityService.GetFileFormats();
            //    if (fileFormats.Any(f => f != fileEXtension))
            //        await DisplayAlert("Warning", "Upload file in format " + string.Join(", ", fileFormats) , "Ok");
            //    else
            //        await _accountService.PostFileDataToAPIAsync(_mediaFile);
            //};
        }
    }
}