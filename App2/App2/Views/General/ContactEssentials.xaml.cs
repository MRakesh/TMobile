using App2.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views.General
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactEssentials : ContentPage
    {
        public ContactEssentials()
        {
            InitializeComponent();
            BindingContext = new ContactViewModel();
        }
    }
}