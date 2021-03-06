using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class BarCodeListViewModel :BaseViewModel
    {
        private string itemId;

        public Command BarCodeDetailCommand { get; set; }
        public Command LoadBarCodeItemsCommand { get; set; }
        public ObservableCollection<BarCodeΕίδους> BarCodeList { get; set; } 
        public BarCodeListViewModel()
        {
            BarCodeDetailCommand = new Command(async () => await AppShell.Current.GoToAsync(nameof(BarCodeDetailViewPage)));
            LoadBarCodeItemsCommand = new Command(async () => await LoadBarCodeItems());
            BarCodeList = new ObservableCollection<BarCodeΕίδους>();
        }
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;             
            }
        }

        

        private  Task LoadBarCodeItems()
        {
            try
            {
                BarCodeList.Clear();
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var barcodeitems = uow.Query<BarCodeΕίδους>().Where(x=> x.ΕίδοςOid.ToString()==ItemId);
                    foreach(var item in barcodeitems)
                    {
                        BarCodeList.Add(item);
                    }
                }
                return Task.FromResult(true);
            }
            catch(Exception e)
            {
                Debug.WriteLine("Wrong :"+e);
                return Task.FromResult(false);
            }
        }
        public void OnAppearing()
        {
            LoadBarCodeItemsCommand.Execute(true);
        }
    }
}
