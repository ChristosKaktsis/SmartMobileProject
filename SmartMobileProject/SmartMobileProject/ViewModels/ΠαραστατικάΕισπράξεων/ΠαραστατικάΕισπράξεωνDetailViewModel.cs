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
    class ΠαραστατικάΕισπράξεωνDetailViewModel : BaseViewModel
    {
        UnitOfWork uow = new UnitOfWork();
        ΠαραστατικάΕισπράξεων parastatiko;
        Πελάτης customer;
        //errormessage
        public bool stopNavigating = true;
        string pelatiserrormessage;
        string seiraerrormessage;
        public bool hasError = false;
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
        public ΠαραστατικάΕισπράξεων Parastatiko
        {
            get
            {
                return parastatiko;
            }
            set
            {
                parastatiko = value;
                SetProperty(ref parastatiko, value);
                OnPropertyChanged("parastatiko");
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
                Parastatiko.Πελάτης = value;
                Parastatiko.ΔιεύθυνσηΕίσπραξης = null;
                SelectedCustomerName = value == null ? "Επιλογή Πελάτη" : value.DisplayName;
                if (value == null)
                    return;
                Parastatiko.ΔιεύθυνσηΕίσπραξης = value.ΚεντρικήΔιευθυνση;
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
        { get; } = new ObservableCollection<Πελάτης>();
        public ObservableCollection<ΣειρέςΠαραστατικώνΕισπράξεων> ΣειρέςΠαραστατικώνΕισπράξεων
        { get; } = new ObservableCollection<ΣειρέςΠαραστατικώνΕισπράξεων>();
        public ΠαραστατικάΕισπράξεωνDetailViewModel()
        {
            DocCollectHelper.uow = uow;
            SetDoc();
            ΓραμμεςΠΕ = new Command(GoToLines);
            Πίσω = new Command(GoBack);
            OpenPopUp = new Command(() => PopUpIsOpen = !PopUpIsOpen);
        }
        public void OnAppearing()
        {
            LoadCustomers();
            LoadSeires();
        }

        private async void LoadCustomers()
        {
            try
            {
                var currentP = App.Πωλητής;
                if (currentP == null)
                {
                    Debug.WriteLine("LoadCustomers Politis Is Null !!!!");
                    return;
                }
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
            catch (Exception ex) { Debug.WriteLine(ex); }
        }
        private async void LoadSeires()
        {
            try
            {
                var currentP = App.Πωλητής;
                if (currentP == null)
                {
                    Console.WriteLine("LoadSeires Politis Is Null !!!!");
                    return;
                }
                var items = uow.Query<ΣειρέςΠαραστατικώνΕισπράξεων>();
                ΣειρέςΠαραστατικώνΕισπράξεων.Clear();
                await Task.Run(() =>
                {
                    foreach (var item in items)
                    {
                        ΣειρέςΠαραστατικώνΕισπράξεων.Add(item);
                    }
                });
            }
            catch (Exception ex) { Debug.WriteLine(ex);}
        }
        private async void SetDoc()
        {
            if (DocCollectHelper.ParastatikoEispr == null)
            {
                Parastatiko = new ΠαραστατικάΕισπράξεων(uow);
                Parastatiko.Ημνία = DateTime.Now;
                Parastatiko.SmartOid = Guid.NewGuid();
                Parastatiko.ΗμνίαΔημ = DateTime.Now;
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
            else
            {
                Parastatiko = await uow.GetObjectByKeyAsync<ΠαραστατικάΕισπράξεων>(DocCollectHelper.ParastatikoEispr.Oid);
                Customer = Parastatiko.Πελάτης;
                Title = Parastatiko.Παραστατικό;
                SeiraIsReadOnly = true;
            }
            DocCollectHelper.ParastatikoEispr = Parastatiko;
        }
        private async void GoToLines(object obj)
        {
            if (ChechError())
            {
                return;
            }
            await Shell.Current.GoToAsync(nameof(ΓραμμέςΠαραστατικώνΕισπράξεωνListViewPage));
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
                if (Parastatiko.Σειρά == null)
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
            set => SetProperty(ref _CustomerFieldColor, value);
        }
        void CheckSeira()
        {
            if (Parastatiko.Σειρά == null)
            {
                SeiraErrorMessage = "Η Σειρά Δεν πρέπει να είναι κενή";
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
            set
            {
                SetProperty(ref popUpIsOpen, value);
            }
        }
        public Command OpenPopUp { get; set; }
        public ICommand ΓραμμεςΠΕ { get; set; }
        public ICommand Πίσω { get; set; }
    }
}
