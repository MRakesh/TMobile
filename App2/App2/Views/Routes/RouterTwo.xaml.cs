using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views.Routes
{
      
    [QueryProperty("Name", "name")]
    [QueryProperty("IdParam", "id")]
    public partial class RouterTwo : ContentPage
    {
        public RouterTwo()
        {
            InitializeComponent();
          //  CallData(name);
        }

        private string name;
        private string id;
        //public string Name
        //{
        //    //get => name;
        //    set=> name = Uri.UnescapeDataString(value);
        //}

        public string Name
        {
            get => name; set
            {                
                name = Uri.UnescapeDataString(value);
                CallData1(name + " " + id);
            }
        }
        public string IdParam
        {
            get => id; set
            {
                id = Uri.UnescapeDataString(value);
                CallData(name + " " + id);
            }
        }

        public void CallData(string tname)
        {
            lblName.Text = "0- " + tname;   
        }
         public void CallData1(string tname)
        {
            lblName1.Text = tname;   
        }
         public void CallData2(string tname)
        {
            lblName1.Text = "2 -" +  tname;   
        }

        private void BtnGet_Clicked(object sender, EventArgs e)
        {
            CallData2(name + " " + id);
            // DisplayAlert("QueryParam", name, "Ok");
        }
    }
}