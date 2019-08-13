using App2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views.Associate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssociateHome : ContentPage
    {
        public AssociateHome()
        {
            InitializeComponent();
            BindingContext = new AssociateViewModel(this);
        }
    }
}