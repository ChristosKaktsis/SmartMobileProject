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
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodeViewModel : BaseViewModel
    {
        private string _searchText;
        private bool _isFocused;
        private BarCodeΕίδους _selectedBarCode;
        private bool _isSelected;
        private float _ποσότητα = 1;

        public ObservableCollection<BarCodeΕίδους> BarCodeList { get; set; }
        public Command LoadBarCodeItemsCommand { get; set; }
        public Command Προσθήκη { get; set; }
        public ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodeViewModel()
        {
            LoadBarCodeItemsCommand = new Command(async () => await LoadBarCodeitems());
            Προσθήκη = new Command(OnAddLinePressed);
            BarCodeList = new ObservableCollection<BarCodeΕίδους>();
        }

        private async void OnAddLinePressed(object obj)
        {
            try
            {
                UnitOfWork uow = ΝέοΠαραστατικόViewModel.uow;
                ΓραμμέςΠαραστατικώνΠωλήσεων νεαΓραμμή = new ΓραμμέςΠαραστατικώνΠωλήσεων(uow);
                νεαΓραμμή.SmartOid = Guid.NewGuid();
                νεαΓραμμή.Ποσότητα = Ποσότητα;

                νεαΓραμμή.Είδος = uow.Query<Είδος>().Where(x => x.SmartOid == SelectedBarCode.ΕίδοςOid).FirstOrDefault();
                νεαΓραμμή.Τιμή = SelectedBarCode.ΑκολουθείΤήνΤιμήΕίδους ? νεαΓραμμή.Είδος.ΤιμήΧονδρικής : SelectedBarCode.ΤιμήΧονδρικής;
                νεαΓραμμή.ΠοσοστόΦπα = SelectedBarCode.ΦΠΑ != null ? (SelectedBarCode.ΦΠΑ.Φπακανονικό / 100) : 0;
                νεαΓραμμή.Εκπτωση = 0;
                νεαΓραμμή.ΑξίαΕκπτωσης = (decimal)(νεαΓραμμή.Ποσότητα * νεαΓραμμή.Τιμή * νεαΓραμμή.Εκπτωση);
                νεαΓραμμή.ΚαθαρήΑξία = (decimal)(νεαΓραμμή.Ποσότητα * νεαΓραμμή.Τιμή) - νεαΓραμμή.ΑξίαΕκπτωσης;
                νεαΓραμμή.Φπα = νεαΓραμμή.ΚαθαρήΑξία * (decimal)νεαΓραμμή.ΠοσοστόΦπα;
                νεαΓραμμή.ΑξίαΓραμμής = νεαΓραμμή.ΚαθαρήΑξία + νεαΓραμμή.Φπα;
                νεαΓραμμή.BarCodeΕίδους = uow.Query<BarCodeΕίδους>().Where(x => x.Oid == SelectedBarCode.Oid).FirstOrDefault();
                νεαΓραμμή.ΠαραστατικάΠωλήσεων = ΝέοΠαραστατικόViewModel.Order;
                await Shell.Current.GoToAsync("..");
                await Shell.Current.GoToAsync(nameof(ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodePage));
            }
            catch(Exception e)
            {
                Console.WriteLine("BarCode Add Line Error "+e);
                await Application.Current.MainPage.DisplayAlert("Σφάλμα",
                     "Κάτι πήγε λάθος στην προσθήκη Γραμμής με BarCode", "Εντάξει");
            }
           
        }

        private async Task<bool> LoadBarCodeitems()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                    return await Task.FromResult(false); 

                IsBusy = true;
                BarCodeList.Clear();
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var barcodeitems = uow.Query<BarCodeΕίδους>().Where(x=>x.Κωδικός == SearchText);
                    foreach (var item in barcodeitems)
                        BarCodeList.Add(item);
                }
                if(!BarCodeList.Any())
                    await Application.Current.MainPage.DisplayAlert("Search",
                     "Το BarCode Δεν βρέθηκε!", "Εντάξει");
                SelectedBarCode = BarCodeList.FirstOrDefault();

                return await Task.FromResult(true);
            }
            catch(Exception e)
            {
                Debug.WriteLine("Κάτι πήγε λάθος στο φόρτομα barcode "+e);
                return await Task.FromResult(false);
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
            set 
            { 
                SetProperty(ref _selectedBarCode, value);
                if (value != null)
                    IsItemSelected = true;
                else
                    IsItemSelected = false;
            }
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
        public bool IsItemSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }
        public float Ποσότητα
        {
            get { return _ποσότητα; }
            set { SetProperty(ref _ποσότητα, value); }
        }
    }
}
