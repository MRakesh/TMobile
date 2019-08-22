using App2.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views.General
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("ParamOne", "param1")]
    public partial class ContactEssentials : ContentPage
    {
        private string paramOne;
        public ContactEssentials()
        {
            InitializeComponent();
        }

        public string ParamOne
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    BindingContext = new ContactViewModel(this, value);
                }
                else
                {
                    BindingContext = new ContactViewModel(this);
                }
            }
        }

    }
}