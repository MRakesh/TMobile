﻿using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace App2.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
            TokenCommand = new Command(CalculateSquareRoot);
        }
        void CalculateSquareRoot()
        {
           
        }
        public ICommand OpenWebCommand { get; }
        public ICommand TokenCommand { get; }
    }
}