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
        private float _ποσότητα = 1;

        public ObservableCollection<BarCodeΕίδους> BarCodeList { get; set; }
        public ObservableCollection<BarCodeΕίδους> OtherBarCodeList { get; set; }
        public ObservableCollection<BarCodeΕίδους> SelectedBarCodeList { get; set; }
        public Command LoadBarCodeItemsCommand { get; set; }
        public Command Προσθήκη { get; set; }
        public ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodeViewModel()
        {
            LoadBarCodeItemsCommand = new Command(async () => await LoadBarCodeitems());
            Προσθήκη = new Command(OnAddLinePressed);
            BarCodeList = new ObservableCollection<BarCodeΕίδους>();
            OtherBarCodeList = new ObservableCollection<BarCodeΕίδους>();
            SelectedBarCodeList = new ObservableCollection<BarCodeΕίδους>();
            SelectedBarCodeList.CollectionChanged += SelectedBarCodeList_CollectionChanged;
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
        private async void OnAddLinePressed()
        {
            try
            {
                AddLineOfOrders(SelectedBarCode);
                foreach (var item in SelectedBarCodeList)
                    AddLineOfOrders(item);
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
        private void AddLineOfOrders(BarCodeΕίδους item)
        {
            if (item == null)
                return;
            UnitOfWork uow = ΝέοΠαραστατικόViewModel.uow;
            ΓραμμέςΠαραστατικώνΠωλήσεων νεαΓραμμή = new ΓραμμέςΠαραστατικώνΠωλήσεων(uow);
            νεαΓραμμή.SmartOid = Guid.NewGuid();
            νεαΓραμμή.Ποσότητα = item.Ποσότητα;
            νεαΓραμμή.Είδος = uow.Query<Είδος>().Where(x => x.SmartOid == item.ΕίδοςOid).FirstOrDefault();
            νεαΓραμμή.Τιμή = item.ΑκολουθείΤήνΤιμήΕίδους ? νεαΓραμμή.Είδος.ΤιμήΧονδρικής : item.ΤιμήΧονδρικής;
            νεαΓραμμή.ΠοσοστόΦπα = item.ΦΠΑ != null ? (item.ΦΠΑ.Φπακανονικό / 100) : 0;
            νεαΓραμμή.Εκπτωση = 0;
            νεαΓραμμή.ΑξίαΕκπτωσης = (decimal)(νεαΓραμμή.Ποσότητα * νεαΓραμμή.Τιμή * νεαΓραμμή.Εκπτωση);
            νεαΓραμμή.ΚαθαρήΑξία = (decimal)(νεαΓραμμή.Ποσότητα * νεαΓραμμή.Τιμή) - νεαΓραμμή.ΑξίαΕκπτωσης;
            νεαΓραμμή.Φπα = νεαΓραμμή.ΚαθαρήΑξία * (decimal)νεαΓραμμή.ΠοσοστόΦπα;
            νεαΓραμμή.ΑξίαΓραμμής = νεαΓραμμή.ΚαθαρήΑξία + νεαΓραμμή.Φπα;
            νεαΓραμμή.BarCodeΕίδους = uow.Query<BarCodeΕίδους>().Where(x => x.Κωδικός == item.Κωδικός).FirstOrDefault();
            νεαΓραμμή.ΠαραστατικάΠωλήσεων = ΝέοΠαραστατικόViewModel.Order;
        }
        private async Task LoadBarCodeitems()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                    return ; 

                IsBusy = true;
                //set the list
                BarCodeList.Clear();
                SelectedBarCodeList.Clear();//if change SearchText
                using (UnitOfWork uow = new UnitOfWork())
                {
                    //get the item
                    var barcodeitem = await uow.GetObjectByKeyAsync<BarCodeΕίδους>(SearchText);
                    if (barcodeitem != null)
                        BarCodeList.Add(barcodeitem);
                }
                //notify user if nothing came up
                if(!BarCodeList.Any())
                    await Application.Current.MainPage.DisplayAlert("Search",
                     "Το BarCode Δεν βρέθηκε!", "Εντάξει");
                //select the item
                SelectedBarCode = BarCodeList.FirstOrDefault();
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
        private  void LoadOtherBarCodeItems()
        {
            try
            {
                IsBusy = true;
                OtherBarCodeList.Clear();
                if (SelectedBarCode == null)
                    return;
                
                using (UnitOfWork uow = new UnitOfWork())
                {
                    //get the other bar code items from the same item
                    var restbarcodeitems = uow.Query<BarCodeΕίδους>().Where(x => x.ΕίδοςOid == SelectedBarCode.ΕίδοςOid);
                    foreach (var item in restbarcodeitems)
                        if(item.Κωδικός!=SelectedBarCode.Κωδικός)
                            OtherBarCodeList.Add(item);
                }
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
                AddQuantity(value);
                if (OneOne)
                    if (value != null)
                        OnAddLinePressed();
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
            set 
            { 
                SetProperty(ref _ποσότητα, value);
            }
        }
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
