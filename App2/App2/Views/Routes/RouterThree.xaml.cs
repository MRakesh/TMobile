using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views.Routes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("Name", "name")]
    public partial class RouterThree : ContentPage
    {
        public RouterThree()
        {
            InitializeComponent();
           

        }


    }
}