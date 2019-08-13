using App2.Models;
using App2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.MultiSelectListView;
using Xamarin.Forms.Xaml;

namespace App2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogInPage : ContentPage
    {
        LoginViewModelTemp viewModel;
        public LogInPage()
        {
            InitializeComponent();
            int id = 2;
            viewModel = new LoginViewModelTemp();
            this.picker.SelectedIndex = viewModel.StateList.IndexOf(viewModel.StateList.First(e => e.Id == id));
            BindingContext = new LoginViewModelTemp();
        }

        private void BtnLogin_Clicked(object sender, EventArgs e)
        {
            string selectedList = string.Empty;
            var litems = lvwUsers.ItemsSource;
            //var litems1 = lvwUsers.SelectedItem;

            foreach (var item in litems)
            {
                var selectedItem = (SelectableItem<State>)item;
                if (selectedItem.IsSelected)
                {
                    var state = selectedItem.Data;
                    selectedList += state.Name + ", ";
                }
            }

            DisplayAlert("User Details", selectedList, "ok");
            //indicator.IsRunning = true;
            //foreach (var item in viewModel.Users.SelectedItems)
            //{
            //    if (item.Id > 0)
            //        selectedList += item.Id + ", ";
            //}

            ////foreach (var item in viewModel.idList)
            ////{
            ////    selectedList += item + ", ";
            ////}

            //System.Threading.Thread.Sleep(3000);
            //string user = txtUser.Text;
            //string password = txtPsw.Text;
            //indicator.IsRunning = false;
            //DisplayAlert("User Details", selectedList, "ok");            
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                var state = (State)picker.SelectedItem;
                DisplayAlert("User Details", state.Id + " " + state.Name + " " + state.Location, "ok");
                // monkeyNameLabel.Text = (string)picker.ItemsSource[selectedIndex];
            }

        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            lvwUsers.ItemsSource = viewModel.Users.Where(ep => ep.Data.Name.ToLower().Contains(searchBar.Text.ToLower()));
        }
    }
}