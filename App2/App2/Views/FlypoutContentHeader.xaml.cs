using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlypoutContentHeader : ContentView
    {
        public FlypoutContentHeader()
        {
            InitializeComponent();

            lblUserName.Text = "User Exists Give Unique Name";
        }
    }
}