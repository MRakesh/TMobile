﻿using App2.ViewModels;
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
    public partial class AssociateProfileInfo : ContentPage
    {
        public AssociateProfileInfo()
        {
            InitializeComponent();
            BindingContext = GetViewModel();
        }

        protected override async void OnAppearing()
        {
            await GetViewModel().FillProfileInfo();
        }

        protected override async void OnBindingContextChanged()
        {
            await GetViewModel().FillProfileInfo();
        }
        AssociateProfileInfoViewModel GetViewModel()
        {
            return new AssociateProfileInfoViewModel(this);
        }
    }
}