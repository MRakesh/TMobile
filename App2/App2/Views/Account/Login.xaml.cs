using App2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel(this);
        }

        private async void BtnContavtNavigate_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("contactessential?param1=05");
        }
    }
}



//async void SetTEdotor()
//{
//    TEditorResponse response = await CrossTEditor.Current.ShowTEditor("<pre>XAM consulting</pre>");
//    if (!string.IsNullOrEmpty(response.HTML))
//        _displayWebView.Source = new HtmlWebViewSource() { Html = response.HTML };            

//}

//void webviewNavigating(object sender, WebNavigatingEventArgs e)
//{
//    labelLoading.IsVisible = true;
//}

//void webviewNavigated(object sender, WebNavigatedEventArgs e)
//{
//    labelLoading.IsVisible = false;
//}