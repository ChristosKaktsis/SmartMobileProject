using DevExpress.Data.Filtering;
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
    class ΠαραστατικόDetailViewModel : BaseViewModel
    {
        UnitOfWork uow = new UnitOfWork();
        ΠαραστατικάΠωλήσεων order;
        Πελάτης customer;
        //errormessage
        string pelatiserrormessage;
        string seiraerrormessage;
        public  bool hasError = false;
        public bool HasError
        {
            get => hasError;  
            set=> SetProperty(ref hasError, value);
        }
        public string PelatisErrorMessage
        {
            get => pelatiserrormessage;
            set => SetProperty(ref pelatiserrormessage, value);
        }
        public string SeiraErrorMessage
        {
            get => seiraerrormessage; 
            set => SetProperty(ref seiraerrormessage, value);
        }
        //
        public ΠαραστατικάΠωλήσεων Order
        {
            get => order;
            set=> SetProperty(ref order, value);
        }
        public Πελάτης Customer
        {
            get=> customer;
            set
            {
                SetProperty(ref customer, value);
                Order.Πελάτης = value;
                Order.ΔιεύθυνσηΠαράδοσης = null;
                SelectedCustomerName =value == null ? "Επιλογή Πελάτη" : value.DisplayName;
                if (value == null)
                    return;
                Order.ΔιεύθυνσηΠαράδοσης = value.ΚεντρικήΔιευθυνση;
                PopUpIsOpen = false;
            }
        }
        private string selectedCustomerName = "Επιλογή Πελάτη";
        public string SelectedCustomerName
        {
            get => selectedCustomerName;
            set => SetProperty(ref selectedCustomerName, value);
        }
        public ObservableCollection<Πελάτης> CustomerCollection 
        { get; set; } = new ObservableCollection<Πελάτης>();
        //public XPCollection<ΣειρέςΠαραστατικώνΠωλήσεων> ΣειρέςΠαραστατικώνΠωλήσεων { get; set; }
        public ObservableCollection<ΣειρέςΠαραστατικώνΠωλήσεων> ΣειρέςΠαραστατικώνΠωλήσεων 
        { get; } = new ObservableCollection<ΣειρέςΠαραστατικώνΠωλήσεων>();
        public XPCollection<ΤρόποςΠληρωμής> ΤρόποςΠληρωμής { get; set; }
        public XPCollection<ΜεταφορικόΜέσο> ΜεταφορικόΜέσο { get; set; }
        public ΠαραστατικόDetailViewModel()
        {
            DocHelperViewModel.uow = uow;
            SetOrder();
            DocHelperViewModel.Order = Order;
            //ΝέοΠαραστατικόViewModel.politis.Πελάτες.Reload();//
            ΤρόποςΠληρωμής = new XPCollection<ΤρόποςΠληρωμής>(uow);
            ΜεταφορικόΜέσο = new XPCollection<ΜεταφορικόΜέσο>(uow);
            ΓραμμεςΠΠ = new Command(GoToLines);
            Πίσω = new Command(GoBack);
            ΕπιλογήΠροηγούμενης = new Command(GoToChooseOlderOrder);
            OpenPopUp = new Command(() => PopUpIsOpen = !PopUpIsOpen);
        }
        public async void OnAppearing()
        {
            await LoadSeires();
            await LoadCustomers();
        }
        private async Task LoadCustomers()
        {
            try
            {
                var currentP = App.Πωλητής;
                if (currentP == null)
                {
                    Debug.WriteLine("LoadSeires Politis Is Null !!!!");
                    return;
                }
                //if(LoadAllCustomers)
                //    CustomerCollection = new XPCollection<Πελάτης>(uow);
                //else
                //    CustomerCollection = ΝέοΠαραστατικόViewModel.politis.Πελάτες;
                List<Πελάτης> items;
                if (LoadAllCustomers)
                    items = await uow.Query<Πελάτης>().ToListAsync();
                else
                    items = await uow.Query<Πελάτης>().Where(
                        x => x.Πωλητής.SmartOid == currentP.SmartOid).ToListAsync();
                CustomerCollection.Clear();
                var sortedItems = items.OrderBy(i => i.DisplayName);
                await Task.Run(() =>
                {
                    foreach (var item in sortedItems)
                    {
                        CustomerCollection.Add(item);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private async Task LoadSeires()
        {
            try
            {
                var currentP = App.Πωλητής;
                if (currentP == null)
                {
                    Console.WriteLine("LoadSeires Politis Is Null !!!!");
                    return;
                }
                var items = uow.Query<ΣειρέςΠαραστατικώνΠωλήσεων>().Where(x => x.IDΠωλητή == currentP.SmartOid);
                ΣειρέςΠαραστατικώνΠωλήσεων.Clear();
                await Task.Run(() =>
                {
                    foreach (var item in items)
                    {
                        ΣειρέςΠαραστατικώνΠωλήσεων.Add(item);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private async void SetOrder()
        {
            try
            {
                if (DocHelperViewModel.Order == null)
                {
                    Order = new ΠαραστατικάΠωλήσεων(uow);
                    Order.Ημνία = DateTime.Now;
                    Order.SmartOid = Guid.NewGuid();
                    Order.ΗμνίαΔημ = DateTime.Now;
                    //Selected Πελατης με Oid 
                    SetCustomer();
                    SetCustomerFromNewCustomer(DocHelperViewModel.πελατης);
                    Title = "Βασικά Στοιχεία";
                }
                else
                {
                    //Order = ΝέοΠαραστατικόViewModel.Order;
                    Order = await uow.GetObjectByKeyAsync<ΠαραστατικάΠωλήσεων>(DocHelperViewModel.Order.Oid);
                    Customer = Order.Πελάτης;//uow in transaction γιατι βαζουμε στοιχεια στο Order 
                    Title = Order.Παραστατικό;
                    SeiraIsReadOnly = true;
                }
            }
            catch(Exception ex) { Debug.WriteLine(ex); }
        }
        private void SetCustomerFromNewCustomer(Πελάτης πελατης)
        {
            if (πελατης == null)
                return;
            Customer = uow.Query<Πελάτης>().Where(x => x.Oid== πελατης.Oid).FirstOrDefault();
        }
        private void SetCustomer()
        {
            if (Application.Current.Properties.ContainsKey("Πελάτης"))
            {
                if (Application.Current.Properties["Πελάτης"] != null)
                {
                    string pelatis = (string)Application.Current.Properties["Πελάτης"];
                    // do something with id
                    var find = uow.GetObjectByKey<Πελάτης>(int.Parse(pelatis));
                    Customer = (Πελάτης)find;  
                }
            }
        }
        private async void GoToLines(object obj)
        {
            if (ChechError())
            {
                return;
            }
            await Shell.Current.GoToAsync(nameof(ΓραμμέςΠαραστατικώνΠωλήσεωνListViewPage));
        }
        private async void GoToChooseOlderOrder(object obj)
        {
            if (ChechError())
            {
                return;
            }
            await Shell.Current.GoToAsync(nameof(ΕπιλογήΠροηγΠαρΠωλPage));
        }
        //errorChecks
        bool pelatisIsFocused;
        public bool PelatisIsFocused
        {
            set
            {
                SetProperty(ref pelatisIsFocused, value);
                OnPelatisFocusedChanged(value);
            }
        }
        private void OnPelatisFocusedChanged(bool value)
        {
            if (!value)
            {
                if (Customer == null)
                {
                    //PelatisErrorMessage = "Το ΑΦΜ Δεν Πρέπει να είναι κενό";
                }
                else
                {
                    PelatisErrorMessage = string.Empty;
                }
            }
        }
        bool seiraIsFocused;
        public bool SeiraIsFocused
        {
            set
            {
                SetProperty(ref seiraIsFocused, value);
                OnSeiraFocusedChanged(value);
            }
        }
        private void OnSeiraFocusedChanged(bool value)
        {
            if (!value)
            {
                if (Order.Σειρά == null)
                {
                    //PelatisErrorMessage = "Το ΑΦΜ Δεν Πρέπει να είναι κενό";
                }
                else
                {
                    SeiraErrorMessage = string.Empty;
                }
            }
        }
        bool ChechError()
        {
            CheckPelati();
            CheckSeira();
            if (!string.IsNullOrEmpty(PelatisErrorMessage) || !string.IsNullOrEmpty(SeiraErrorMessage))
            {
                HasError = true;
            }
            else
            {
                HasError = false;
            }
            return HasError;
        }
        void CheckPelati()
        {
            if (Customer == null)
            {
                PelatisErrorMessage = "Ο Πελάτης Δεν πρέπει να είναι κενός";
                CustomerFieldColor = Color.Red;
            }
            else
            {
                PelatisErrorMessage = string.Empty;
                CustomerFieldColor = Color.Gray;
            }
        }
        private Color _CustomerFieldColor = Color.Gray;
        public Color CustomerFieldColor
        {
            get => _CustomerFieldColor;
            set =>SetProperty(ref _CustomerFieldColor, value);
        }
        void CheckSeira()
        {
            if (Order.Σειρά == null)
            {
                SeiraErrorMessage = "Η σειρά δε πρέπει να είναι κενή";
            }
            else
            {
                SeiraErrorMessage = string.Empty;
            }
        }
        private bool _SeiraIsReadOnly;
        public bool SeiraIsReadOnly
        {
            get => _SeiraIsReadOnly;
            set => SetProperty(ref _SeiraIsReadOnly, value);
        }
        //popup
        private bool popUpIsOpen;
        public bool PopUpIsOpen
        {
            get => popUpIsOpen;
            set=> SetProperty(ref popUpIsOpen, value);
        }
        public Command ΕπιλογήΠροηγούμενης { get; set; }
        public Command OpenPopUp { get; set; }
        public ICommand ΓραμμεςΠΠ { get; set; }
        public ICommand Πίσω { get; set; }
    }
}
