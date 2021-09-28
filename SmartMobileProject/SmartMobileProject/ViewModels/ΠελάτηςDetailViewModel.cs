using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;
using SmartMobileProject.Views;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMobileProject.ViewModels
{
    [QueryProperty(nameof(IsFromEdit), nameof(IsFromEdit))]
    class ΠελάτηςDetailViewModel : BaseViewModel
    {
        App app = (App)Application.Current; //
   
        private Πελάτης icustomer;
       
       //flags   
        public bool saveButtonPressed = false;
        public bool isFromEdit = false;
        public bool stopNavigating = true;
        public bool afmIsFocused;
        public bool eponimiaIsFocused;
        public static bool hasError = false;
        //
        //Error Messages
        string errorMessage;
        string afmErrorMessage;
        string eponimiaErrorMessage;
        string dieuthinsiErrorMessage;
        //

        public Πελάτης customer
        {
            get
            {
                return icustomer;
            }
            set
            {
                //icustomer = value;
                SetProperty(ref icustomer, value);
                OnPropertyChanged("customer");
            }
        }
        public XPCollection<ΔΟΥ> DOYCollection { get; set; }
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                SetProperty(ref errorMessage, value);
     

            }
        }
        public string AfmErrorMessage
        {
            get
            {
                return afmErrorMessage;
            }
            set
            {            
                SetProperty(ref afmErrorMessage, value);
                OnErrorMessageChanged(value);
                            
            }
        }
        public string EponimiaErrorMessage
        {
            get
            {
                return eponimiaErrorMessage;
            }
            set
            {
                SetProperty(ref eponimiaErrorMessage, value);
                OnErrorMessageChanged(value);
            }
        }
        public string DieuthinsiErrorMessage
        {
            get
            {
                return dieuthinsiErrorMessage;
            }
            set
            {
                SetProperty(ref dieuthinsiErrorMessage, value);
                OnErrorMessageChanged(value);
            }
        }
        public bool IsFromEdit
        {
            get
            {
                return isFromEdit;
            }
            set
            {
                isFromEdit = value;
                SetProperty(ref isFromEdit, value);
                SetTitle();
            }
        }
        public bool AfmIsFocused
        {
            get { return afmIsFocused; }
            set
            {            
                SetProperty(ref afmIsFocused, value);
                OnAfmFocusedChanged(value);
            }
        }
        public bool EponimiaIsFocused
        {
            get { return eponimiaIsFocused; }
            set
            {
                SetProperty(ref eponimiaIsFocused, value);
                OnEponimiaFocusedChanged(value);
            }
        }
        public bool HasError
        {
            get { return hasError; }
            set
            {             
                SetProperty(ref hasError, value);
            }
        }

        public ΠελάτηςDetailViewModel()
        {
           
            //
            //Πάρε τον πελάτη απο το App και βαλ τον στο icustomer
            //
            AppShell appShell = (AppShell)Application.Current.MainPage;
            customer = appShell.customer1;
                       
            if (customer == null)
            {
                App app1 = (App)Application.Current;      
                appShell.customer1 = new Πελάτης(app.uow);
                appShell.customer1.SmartOid = Guid.NewGuid();
                appShell.customer1.ΗμνίαΔημ = DateTime.Now;
                customer = appShell.customer1;
                appShell.πωλητής.Πελάτες.Add(customer);
            }            
            //set the DOY Collection
            DOYCollection = new XPCollection<ΔΟΥ>(app.uow);

            //commands
            Δημιουργία = new Command(Create);
            Αποθήκευση = new Command(Save);
            Διευθύνσεις = new Command(OpenAddress);
            Πίσω = new Command(GoBack);
        }

        private  void SetTitle()
        {
            if (IsFromEdit)
            {
                Title = "Επεξεργασια";
            }
            else
            {
                Title = "Νεος Πελάτης"; 
            }
        }
        private void Create(object obj)
        {
            //
            //Πάρε τον πελάτη απο το App και βαλ τον στο icustomer
            //
            AppShell appShell = (AppShell)Application.Current.MainPage;
            this.icustomer = appShell.customer1;

            if (customer == null)
            {             
                customer = new Πελάτης(app.uow);
            }

        }
        private async void Save(object obj)
        {

            if (ChechError())
            {
                await Application.Current.MainPage.DisplayAlert("Alert", ErrorMessage, "OK");
                return;
            }
            customer.ΚεντρικήΔιευθυνση = customer.ΔιευθύνσειςΠελάτη.FirstOrDefault();

            if (app.uow.InTransaction)
            {
                try
                {
                    if (CheckPelatis())
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "Υπάρχει ίδη πελάτης με ΑΦΜ :" + customer.ΑΦΜ, "OK");
                        return;
                    }
                    customer.CanUpload = true;
                    app.uow.CommitChanges();
                }
                catch(Exception e)
                {
                    //Console.WriteLine(e);
                    return;
                }
            }

            //tell the app that the save button is pressed
            saveButtonPressed = true;
            
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");

        }
        private async void OpenAddress(object obj)
        {
            await Shell.Current.GoToAsync(nameof(ΔιευθύνσειςΠελάτηListViewPage));
        }
        void OnAfmFocusedChanged(bool hasFocus)
        {
            if (!hasFocus)
            {
               
                if (string.IsNullOrEmpty(customer.ΑΦΜ))
                {
                    AfmErrorMessage = "Shoud Not Be Empty";
                    HasError = true;
                }
                else
                {
                    AfmErrorMessage = string.Empty;
                    HasError = false;
                }
            }
        }
        void OnEponimiaFocusedChanged(bool hasFocus)
        {
            if (!hasFocus)
            {
                if (string.IsNullOrEmpty(customer.Επωνυμία))
                {
                    EponimiaErrorMessage = "Shoud Not Be Empty";
                    HasError = true;
                }
                else
                {
                    EponimiaErrorMessage = string.Empty;
                    HasError = false;
                }
            }
        }
        //
        //error Checks
        //
        bool ChechError()
        {
            CheckAfm();
            CheckEponimia();
            CheckDieuthinsi();
            if (!string.IsNullOrEmpty(AfmErrorMessage) || !string.IsNullOrEmpty(EponimiaErrorMessage) || !string.IsNullOrEmpty(DieuthinsiErrorMessage))
            {
                HasError = true;
                ErrorMessage = AfmErrorMessage +"\n"+ EponimiaErrorMessage+"\n"+ DieuthinsiErrorMessage;
            }
            else
            {
                HasError = false;
            }
            return HasError;
        }
        void CheckAfm()
        {
            if (string.IsNullOrEmpty(customer.ΑΦΜ))
            {
                AfmErrorMessage = "Το ΑΦΜ Δεν Πρέπει να είναι κενό";
            }
            else
            {
                AfmErrorMessage = string.Empty;
            }
        }
        void CheckEponimia()
        {
            if (string.IsNullOrEmpty(customer.Επωνυμία))
            {
                EponimiaErrorMessage = "Η Επωνυμία Δεν Πρέπει να είναι κενή";
            }
            else
            {
                EponimiaErrorMessage = string.Empty;
            }
        }
        void CheckDieuthinsi()
        {
            if (customer.ΔιευθύνσειςΠελάτη.Count == 0)
            {  
                DieuthinsiErrorMessage = "Δεν Υπαρχει καμία Διεύθυνση";           
            }
            else
            {
                DieuthinsiErrorMessage = string.Empty;
            }
        }
        bool CheckPelatis()
        {
           
            var p = app.uow.Query<Πελάτης>().Where(x => x.Πωλητής == customer.Πωλητής 
            && x.ΑΦΜ == customer.ΑΦΜ);  
            
            if (p.Any() && p.FirstOrDefault().Oid!= customer.Oid)
                return true;

            return false;
        }
        void OnErrorMessageChanged(string newMessage)
        {
            if (newMessage == null)
            {
                return;
            }
           
            
        }
      //
        /// <summary>
        /// Γινεται commit στην βαση
        /// </summary>
        public ICommand Αποθήκευση { set; get; }
        public ICommand Δημιουργία { set; get; }
        public ICommand Διευθύνσεις { set; get; }
        public ICommand Πίσω { get; set; }

    }
}
