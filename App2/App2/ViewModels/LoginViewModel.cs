using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using App2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xamarin.Forms.MultiSelectListView;

namespace App2.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private bool _isBusy;
        private const int PageSize = 15;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        public List<State> StateList { get; set; }
        public InfiniteScrollCollection<State> Items { get; }
        public MultiSelectObservableCollection<State> Users { get; }

        private string someIds = string.Empty;
        public string SomeIds { get => someIds; set { someIds = value; SetProperty(ref someIds, value); } }

        public List<string> idList = null;
        public List<string> IdList
        {
            get { return idList; }
            set
            {
                idList = value;
                SetProperty(ref idList, value);
            }
        }

        public string GetCountofIds => someIds.Count().ToString();

        public ICommand LoadMore
        {
            get;
            set;
        }

        public LoginViewModel()
        {
            Items = new InfiniteScrollCollection<State>
            {
                OnLoadMore = async () =>
                {
                    IsBusy = true;

                    // load the next page
                    var page = Items.Count / PageSize;

                    var items = await Task.Run(() => GetStatesPagination(page, PageSize));

                    IsBusy = false;

                    // return the items that need to be added
                    return items;
                }
                //,
                //OnCanLoadMore = () =>
                //{
                //    return Items.Count < 1000;
                //}
            };

            DownloadDataAsync();

            //if (idList is null)
            //    idList = new List<string>();
            if (IdList is null)
            {
                IdList = new List<string>();
            }

            Users = new MultiSelectObservableCollection<State>
            {
                new State{ Id= 1,  Name = "AS",Location = "Location" },
                new State{ Id= 2,  Name = "BS",Location = "Location" },
                new State{ Id= 3,  Name = "CS",Location = "Location" },
                new State{ Id= 4,  Name = "DS",Location = "Location" },
                new State{ Id= 5,  Name = "ES",Location = "Location" }
            };

            Users[0].IsSelected = true;
            Users[Users.Count - 1].IsSelected = true;

            StateList = GetStates();
            ////OnPropertyChanged("StateList");
            ////int page1= 2;
            ////this.LoadMore = new Command(() =>
            ////{
            ////    var newNews = GetStatesPagination(1);
            ////    page1 += 1;
            ////    foreach (var item in newNews)
            ////    {
            ////        StateList.Add(item);
            ////        OnPropertyChanged("StateList");
            ////    }
            ////});

        }

        private void DownloadDataAsync()
        {
            var items = GetStatesPagination(pageIndex: 0, pageSize: PageSize);

            Items.AddRange(items);
        }

        private List<State> GetStates()
        {
            var nList = new List<State>();

            for (int i = 1; i < 100; i++)
            {
                nList.Add(new State { Id = i, Name = i + "AS", Location = i + "Location" });
            }
            return nList;
            //{
            //    new State{ Id= 1,  Name = "1AS",Location = "Location" },
            //    new State{ Id= 2,  Name = "2BS",Location = "Location" },
            //    new State{ Id= 3,  Name = "3CS",Location = "Location" },
            //    new State{ Id= 4,  Name = "4DS",Location = "Location" },
            //    new State{ Id= 5,  Name = "5AS",Location = "Location" },
            //    new State{ Id= 6,  Name = "6BS",Location = "Location" },
            //    new State{ Id= 7,  Name = "7CS",Location = "Location" },
            //    new State{ Id= 8,  Name = "8DS",Location = "Location" },
            //    new State{ Id= 9,  Name = "9ES",Location = "Location" },
            //    new State{ Id= 9,  Name = "9ES",Location = "Location" },
            //    new State{ Id= 9,  Name = "9ES",Location = "Location" },
            //    new State{ Id= 9,  Name = "9ES",Location = "Location" },
            //    new State{ Id= 9,  Name = "9ES",Location = "Location" },
            //    new State{ Id= 9,  Name = "9ES",Location = "Location" },
            //};
        }
        public List<State> GetStatesPagination(int pageIndex, int pageSize)
        {
            Task.Delay(2000);
            return GetStates().OrderByDescending(e => e.Id).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public ICommand DisplayNameCommand => new Command<State>(user =>
        {

            //var uu = Users[0];

            //IdList.Add(user.Name);
            //SomeIds = user.Name;
            ////Users.Add(new State { Id = 6, Name = "6AS", Location = "Location" });
            ////Users[0].IsSelected = true;
            ////Users[Users.Count - 1].IsSelected = true;

            //if (Users.IsSelected(user))
            //{
            //    Users[Users.IndexOf(Users.First(e => e.Data.Id == user.Id))].IsSelected = true;
            //}
            //else
            //    Users[Users.IndexOf(Users.First(e => e.Data.Id == user.Id))].IsSelected = false;
            //// await Application.Current.MainPage.DisplayAlert("Selected Name", user.Name +  " " +  user.Id, "OK");
        });

        public ICommand DisplayListIdsCommand => new Command<State>(user =>
      {
          // Application.Current.MainPage.DisplayAlert("Selected Name", idList.Count.ToString(), "OK");
      });


    }
}
