using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΠαραστατικάΕισπράξεωνDetailViewModel : BaseViewModel
    {
        UnitOfWork uow;
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
                Parastatiko.ΔιεύθυνσηΕίσπραξης = value.ΚεντρικήΔιευθυνση;
            }
        }
        public XPCollection<Πελάτης> CustomerCollection { get; set; }
        public XPCollection<ΣειρέςΠαραστατικώνΕισπράξεων> ΣειρέςΠαραστατικώνΕισπράξεων { get; set; }
        public ΠαραστατικάΕισπράξεωνDetailViewModel()
        {
            uow = ΠαραστατικάΕισπράξεωνStaticViewModel.uow;

            if (ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr == null)
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
                Title = "Βασικά Στοιχεία";
            }
            else
            {
                Parastatiko = ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr;
                Customer = Parastatiko.Πελάτης;
                Title = Parastatiko.Παραστατικό;
            }
            ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr = Parastatiko;
            // CustomerCollection = new XPCollection<Πελάτης>(uow);
            ΠαραστατικάΕισπράξεωνStaticViewModel.politis.Πελάτες.Reload();
            CustomerCollection = ΠαραστατικάΕισπράξεωνStaticViewModel.politis.Πελάτες;
            ΣειρέςΠαραστατικώνΕισπράξεων = new XPCollection<ΣειρέςΠαραστατικώνΕισπράξεων>(uow);

            ΓραμμεςΠΕ = new Command(GoToLines);
            Πίσω = new Command(GoBack);

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
            }
            else
            {
                PelatisErrorMessage = string.Empty;
            }
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
        public ICommand ΓραμμεςΠΕ { get; set; }
        public ICommand Πίσω { get; set; }
    }
}
