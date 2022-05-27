using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΠαραστατικόDetailViewModel : BaseViewModel
    {
        //UnitOfWork uow = new UnitOfWork();
        UnitOfWork uow ;
        ΠαραστατικάΠωλήσεων order;
        Πελάτης customer;
        //errormessage
        string pelatiserrormessage;
        string seiraerrormessage;
        public  bool hasError = false;
        public bool HasError
        {
            get { return hasError; }
            set
            {
                SetProperty(ref hasError, value);
            }
        }
        public string PelatisErrorMessage
        {
            get
            {
                return pelatiserrormessage;
            }
            set
            {
                SetProperty(ref pelatiserrormessage, value);
            }
        }
        public string SeiraErrorMessage
        {
            get
            {
                return seiraerrormessage;
            }
            set
            {
                SetProperty(ref seiraerrormessage, value);
            }
        }
        //
        public ΠαραστατικάΠωλήσεων Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
                SetProperty(ref order, value);
                OnPropertyChanged("Order");
            }
        }
        public Πελάτης Customer
        {
            get
            {
                return customer;
            }
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
        public XPCollection<Πελάτης> CustomerCollection { get; set; }
        //public XPCollection<ΣειρέςΠαραστατικώνΠωλήσεων> ΣειρέςΠαραστατικώνΠωλήσεων { get; set; }
        public ObservableCollection<ΣειρέςΠαραστατικώνΠωλήσεων> ΣειρέςΠαραστατικώνΠωλήσεων { get; }
        public XPCollection<ΤρόποςΠληρωμής> ΤρόποςΠληρωμής { get; set; }
        public XPCollection<ΤρόποςΑποστολής> ΤρόποςΑποστολής { get; set; }
        public ΠαραστατικόDetailViewModel()
        {
            uow = ΝέοΠαραστατικόViewModel.uow;
            SetOrder();
            ΝέοΠαραστατικόViewModel.Order = Order;
            ΝέοΠαραστατικόViewModel.politis.Πελάτες.Reload();//

            if(LoadAllCustomers)
                CustomerCollection = new XPCollection<Πελάτης>(uow);
            else
                CustomerCollection = ΝέοΠαραστατικόViewModel.politis.Πελάτες;

            //ΣειρέςΠαραστατικώνΠωλήσεων = new XPCollection<ΣειρέςΠαραστατικώνΠωλήσεων>(uow);
            ΣειρέςΠαραστατικώνΠωλήσεων = new ObservableCollection<ΣειρέςΠαραστατικώνΠωλήσεων>();
            ΤρόποςΠληρωμής = new XPCollection<ΤρόποςΠληρωμής>(uow);
            ΤρόποςΑποστολής = new XPCollection<ΤρόποςΑποστολής>(uow);
            ΓραμμεςΠΠ = new Command(GoToLines);
            Πίσω = new Command(GoBack);
            ΕπιλογήΠροηγούμενης = new Command(GoToChooseOlderOrder);
            OpenPopUp = new Command(() => PopUpIsOpen = !PopUpIsOpen);
        }
        public async void OnAppearing()
        {
            await LoadSeires();
        }
        private async Task LoadSeires()
        {
            try
            {
                var currentP = ((AppShell)Application.Current.MainPage).πωλητής;
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
        private void SetOrder()
        {
            if (ΝέοΠαραστατικόViewModel.Order == null)
            {
                Order = new ΠαραστατικάΠωλήσεων(uow);
                Order.Ημνία = DateTime.Now;
                Order.SmartOid = Guid.NewGuid();
                Order.ΗμνίαΔημ = DateTime.Now;
                //Selected Πελατης με Oid 
                SetCustomer();
                SetCustomerFromNewCustomer(ΝέοΠαραστατικόViewModel.πελατης);
                Title = "Βασικά Στοιχεία";
            }
            else
            {
                Order = ΝέοΠαραστατικόViewModel.Order;
                Customer = Order.Πελάτης;//uow in transaction γιατι βαζουμε στοιχεια στο Order 
                Title = Order.Παραστατικό;
            }

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
        //popup
        private bool popUpIsOpen;
        public bool PopUpIsOpen
        {
            get => popUpIsOpen;
            set
            {
                SetProperty(ref popUpIsOpen, value);
            }
        }
        public Command ΕπιλογήΠροηγούμενης { get; set; }
        public Command OpenPopUp { get; set; }
        public ICommand ΓραμμεςΠΠ { get; set; }

        public ICommand Πίσω { get; set; }
    }
}
