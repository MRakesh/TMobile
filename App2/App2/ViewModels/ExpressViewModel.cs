using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using App2.Models;
using Xamarin.Forms;

namespace App2.ViewModels
{
    public class ExpressViewModel : BaseViewModel
    {
        public ObservableCollection<State> List { get; set; }

        public ICommand InvokeViewModelPageContentCommand { get; }
        public ICommand DeleteViewModelPageContentCommand { get; }
        public ExpressViewModel()
        {
            List = new ObservableCollection<State>
            {
                new State{ Id= 1,  Name = "AS",Location = "Location" },
                new State{ Id= 2,  Name = "BS",Location = "Location" },
                new State{ Id= 3,  Name = "CS",Location = "Location" },
                new State{ Id= 4,  Name = "DS",Location = "Location" },
                new State{ Id= 5,  Name = "ES",Location = "Location" }
            };

            InvokeViewModelPageContentCommand = new Command(InvokeViewModelPageContent);
            DeleteViewModelPageContentCommand = new Command(DeleteViewModelPageContent);
        }

        public void InvokeViewModelPageContent(object obj)
        {
            List.Add(new State { Id = RandomNumber(), Name = "Viewmodel-Data-AS", Location = "Location" });
            //List = new ObservableCollection<State>
            //{
            //    new State{ Name = "Viewmodel-Data-AS",Location = "Location" },
            //    new State{ Name = "Viewmodel-Data-BS",Location = "Location" },
            //    new State{ Name = "Viewmodel-Data-CS",Location = "Location" },
            //    new State{ Name = "Viewmodel-Data-DS",Location = "Location" },
            //    new State{ Name = "Viewmodel-Data-ES",Location = "Location" }
            //};
        }

        public void DeleteViewModelPageContent(object obj)
        {
            int random = RandomNumber();
            var state = List.FirstOrDefault(e => e.Id == random);
            if (state != null)
                List.Remove(state);
        }

        public int RandomNumber()
        {
            Random random = new Random();
            return random.Next(5);
        }
    }
}
