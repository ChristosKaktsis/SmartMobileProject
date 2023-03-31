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
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodeViewModel : BaseViewModel
    {
        private string _searchText;
        private bool _isFocused;
        private BarCodeΕίδους _selectedBarCode;
        private bool _isSelected;
        private float _MainQuantity = 1;
        UnitOfWork uow = DocHelperViewModel.uow;
        public ObservableCollection<BarCodeΕίδους> OtherBarCodeList { get; set; }
        public ObservableCollection<BarCodeΕίδους> SelectedBarCodeList { get; set; }
        public Command Προσθήκη { get; set; }
        public Command LoadBarCodeCommand { get; }
        public ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodeViewModel()
        {
            Προσθήκη = new Command(Complete);
            //LoadBarCodeCommand = new Command(LoadOtherBarCodeItems);
            OtherBarCodeList = new ObservableCollection<BarCodeΕίδους>();
            SelectedBarCodeList = new ObservableCollection<BarCodeΕίδους>();
            SelectedBarCodeList.CollectionChanged += SelectedBarCodeList_CollectionChanged;
        }

        private async void Complete()
        {
            AddLines();
            await Shell.Current.GoToAsync("..");
        }

        private void SelectedBarCodeList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var add_action = e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add;
            if (add_action)
            {
                var item = e.NewItems[0] as BarCodeΕίδους;
                AddQuantity(item);
            }
        }
        private void AddQuantity(BarCodeΕίδους item)
        {
            if (item == null)
                return;
            item.Ποσότητα = Ποσότητα;
        }
        private async void AddLines()
        {
            try
            {
                AddLineOfOrders(SelectedBarCode);
                foreach (var item in SelectedBarCodeList)
                    AddLineOfOrders(item);
            }
            catch(Exception e)
            {
                Console.WriteLine("BarCode Add Line Error "+e);
                await Application.Current.MainPage.DisplayAlert("Σφάλμα",
                     "Κάτι πήγε λάθος στην προσθήκη Γραμμής με BarCode", "Εντάξει");
            }
           
        }
        private void AddLineOfOrders(BarCodeΕίδους item)
        {
            if (item == null)
                return;
            var currentseira = DocHelperViewModel.Order.Σειρά;
            ΓραμμέςΠαραστατικώνΠωλήσεων νεαΓραμμή = new ΓραμμέςΠαραστατικώνΠωλήσεων(uow);
            νεαΓραμμή.SmartOid = Guid.NewGuid();
            νεαΓραμμή.Ποσότητα = item.Ποσότητα;
            νεαΓραμμή.Είδος = uow.Query<Είδος>().Where(x => x.SmartOid == item.ΕίδοςOid).FirstOrDefault();
            νεαΓραμμή.Τιμή = item.ΑκολουθείΤήνΤιμήΕίδους ? νεαΓραμμή.Είδος.getPrice(currentseira.Λιανική) : item.getPrice(currentseira.Λιανική);
            νεαΓραμμή.ΠοσοστόΦπα = item.ΦΠΑ != null ? (item.ΦΠΑ.Φπακανονικό / 100) : 0;
            νεαΓραμμή.Εκπτωση = 0;
            double clearvalue = CalculateClearValue(νεαΓραμμή.Τιμή, νεαΓραμμή.ΠοσοστόΦπα);
            νεαΓραμμή.ΑξίαΕκπτωσης = (decimal)(νεαΓραμμή.Ποσότητα * clearvalue * νεαΓραμμή.Εκπτωση);
            νεαΓραμμή.ΚαθαρήΑξία = (decimal)(νεαΓραμμή.Ποσότητα * clearvalue) - νεαΓραμμή.ΑξίαΕκπτωσης;
            νεαΓραμμή.Φπα = νεαΓραμμή.ΚαθαρήΑξία * (decimal)νεαΓραμμή.ΠοσοστόΦπα;
            νεαΓραμμή.ΑξίαΓραμμής = νεαΓραμμή.ΚαθαρήΑξία + νεαΓραμμή.Φπα;
            νεαΓραμμή.BarCodeΕίδους = uow.Query<BarCodeΕίδους>().Where(x => x.Κωδικός == item.Κωδικός).FirstOrDefault();
            νεαΓραμμή.ΠαραστατικάΠωλήσεων = DocHelperViewModel.Order;
        }
        /// <summary>
        /// Υπολογισμός καθαρής αξίας χωρίς καμια εκπτωση
        /// </summary>
        /// <param name="τιμή"></param>
        /// <param name="ποσοστόΦπα"></param>
        /// <returns></returns>
        private double CalculateClearValue(double τιμή, float ποσοστόΦπα)
        {
            var currentparastatiko = DocHelperViewModel.Order;
            if (currentparastatiko == null)
                return 0;
            if (currentparastatiko.Σειρά == null)
                return 0;
            if (currentparastatiko.Σειρά.Λιανική)
                return τιμή / (1 + ποσοστόΦπα);
            return τιμή;
        }
        private async void LoadBarCode()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                    return ; 
                IsBusy = true;
              
                SelectedBarCodeList.Clear();//if change SearchText
                 //get the item
                var barcodeitem = await uow.GetObjectByKeyAsync<BarCodeΕίδους>(SearchText);
                //notify user if nothing came up
                if (barcodeitem == null)
                    await Application.Current.MainPage.DisplayAlert("Search",
                     "Το BarCode Δεν βρέθηκε!", "Εντάξει");
                //select the item
                SelectedBarCode = barcodeitem;
                //load other barcode 
                LoadOtherBarCodeItems();
            }
            catch(Exception e)
            {
                Debug.WriteLine("Κάτι πήγε λάθος στο φόρτομα barcode "+e);
            }
            finally
            {
                IsBusy = false;
            }
            
        }
        private void LoadOtherBarCodeItems()
        {
            try
            {
                IsBusy = true;
                OtherBarCodeList.Clear();
                if (SelectedBarCode == null)
                    return;
                //get the other bar code items from the same item
                var restbarcodeitems = uow.Query<BarCodeΕίδους>()
                    .Where(x => x.ΕίδοςOid == SelectedBarCode.ΕίδοςOid);
                foreach (var item in restbarcodeitems)
                    if (item.Κωδικός != SelectedBarCode.Κωδικός)
                        OtherBarCodeList.Add(item);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Κάτι πήγε λάθος στο φόρτομα barcode " + e);
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
                if(_selectedBarCode == null) return;
                _selectedBarCode.Ποσότητα = MainItemQuantity;
                AddScanned();
            }
        }

        private void AddScanned()
        {
            if (!OneOne) return;
            try
            {
                AddLineOfOrders(SelectedBarCode);
                SearchText = string.Empty; // it will clear values 
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e.Message}");
            }
        }

        private void ClearValues()
        {
            SelectedBarCode = null;
            OtherBarCodeList.Clear();
        }
        public string SearchText
        {
            get { return _searchText; }
            set 
            {
                SetProperty(ref _searchText, value);
                if (string.IsNullOrWhiteSpace(value))
                    ClearValues();
            }
        }
        public bool IsFocused
        {
            get { return _isFocused; }
            set 
            { 
                _isFocused = value;
                if (!value)
                    LoadBarCode();
            }
        }
        public bool IsItemSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }
        public float MainItemQuantity 
        {
            get => _MainQuantity; 
            set 
            {
                if (SelectedBarCode == null) return;
                SelectedBarCode.Ποσότητα = value;
            } 
        }
        public float Ποσότητα { get; set; } = 1;
        public bool OneOne
        {
            get => Preferences.Get(nameof(OneOne), false);
            set
            {
                Preferences.Set(nameof(OneOne), value);
                OnPropertyChanged(nameof(OneOne));
            }
        }
    }
}
