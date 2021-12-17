using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodeViewModel : BaseViewModel
    {
        private string _searchText;
        private bool _isFocused;
        private BarCodeΕίδους _selectedBarCode;

        public ObservableCollection<BarCodeΕίδους> BarCodeList { get; set; }
        public Command LoadBarCodeItemsCommand { get; set; }

        public ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodeViewModel()
        {
            LoadBarCodeItemsCommand = new Command(async () => await LoadBarCodeitems());
            BarCodeList = new ObservableCollection<BarCodeΕίδους>();
        }

        private Task LoadBarCodeitems()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                    return Task.FromResult(false); 

                IsBusy = true;
                BarCodeList.Clear();
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var barcodeitems = uow.Query<BarCodeΕίδους>().Where(x=>x.Κωδικός == SearchText);
                    foreach (var item in barcodeitems)
                        BarCodeList.Add(item);
                }
                return Task.FromResult(true);
            }
            catch(Exception e)
            {
                Debug.WriteLine("Κάτι πήγε λάθος στο φόρτομα barcode "+e);
                return Task.FromResult(false);
            }
            finally
            {
                IsBusy = false;
            }
            
        }
        public void OnAppearing()
        {
            
        }
        public BarCodeΕίδους SelectedBarCode
        {
            get { return _selectedBarCode; }
            set { SetProperty(ref _selectedBarCode, value); }
        }
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        public bool IsFocused
        {
            get { return _isFocused; }
            set 
            { 
                _isFocused = value;
                if (!value)
                    LoadBarCodeItemsCommand.Execute(null);
            }
        }
    }
}
