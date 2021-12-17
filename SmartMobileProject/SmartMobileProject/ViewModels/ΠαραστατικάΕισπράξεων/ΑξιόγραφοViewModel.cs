using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΑξιόγραφοViewModel : BaseViewModel
    {
        UnitOfWork uow;
        ΓραμμέςΠαραστατικώνΕισπράξεων γραμμέςΠΕ;
        public ΓραμμέςΠαραστατικώνΕισπράξεων ΓραμμέςΠΕ
        {
            get
            {
                return γραμμέςΠΕ;
            }
            set
            {

                SetProperty(ref γραμμέςΠΕ, value);
                OnPropertyChanged("ΓραμμέςΠΕ");
            }
        }

        Αξιόγραφα αξιόγραφο;
        public Αξιόγραφα Αξιόγραφο
        {
            get
            {
                return αξιόγραφο;
            }
            set
            {

                SetProperty(ref αξιόγραφο, value);
                OnPropertyChanged("Αξιόγραφο");
            }
        }
        public XPCollection<Τράπεζα> Τράπεζα { get; set; }

        //errormessage
        string arithmoserrormessage;
        string axiaerrormessage;
        string hmerlixiserrormessage;
        public bool hasError = false;
        public bool HasError
        {
            get { return hasError; }
            set
            {
                SetProperty(ref hasError, value);
            }
        }
        public string ArithmosErrorMessage
        {
            get
            {
                return arithmoserrormessage;
            }
            set
            {
                SetProperty(ref arithmoserrormessage, value);
            }
        }
        public string AxiaErrorMessage
        {
            get
            {
                return axiaerrormessage;
            }
            set
            {
                SetProperty(ref axiaerrormessage, value);
            }
        }
        public string HmerlixisErrorMessage
        {
            get
            {
                return hmerlixiserrormessage;
            }
            set
            {
                SetProperty(ref hmerlixiserrormessage, value);
            }
        }
        public ΑξιόγραφοViewModel()
        {
            uow = ΠαραστατικάΕισπράξεωνStaticViewModel.uow;
            if (ΠαραστατικάΕισπράξεωνStaticViewModel.editline == null)
            {
                ΓραμμέςΠΕ = new ΓραμμέςΠαραστατικώνΕισπράξεων(uow);
                Αξιόγραφο = new Αξιόγραφα(uow);
                Αξιόγραφο.ΗμνίαΠαραλαβής = DateTime.Now;
                Αξιόγραφο.ΗμνίαΕκδοσης = DateTime.Now;
                ΓραμμέςΠΕ.Αξιόγραφα = Αξιόγραφο;
                ΓραμμέςΠΕ.SmartOid = Guid.NewGuid();
                Αξιόγραφο.SmartOid = Guid.NewGuid();
                Αξιόγραφο.Εκδότης = "Ο ίδιος";
            }
            else
            {
                ΓραμμέςΠΕ = ΠαραστατικάΕισπράξεωνStaticViewModel.editline;
                Αξιόγραφο = ΠαραστατικάΕισπράξεωνStaticViewModel.editline.Αξιόγραφα;
                
            }
           
           
            Τράπεζα = new XPCollection<Τράπεζα>(uow);
            Αποθήκευση = new Command(Save);
            Ακύρωση = new Command(Cancel);
        }
        private async void Save(object obj)
        {
            if (ChechError())
            {
                return;
            }
            if (uow.InTransaction)
            {
                
                ΓραμμέςΠΕ.Ποσόν = Αξιόγραφο.Αξία;
                
                ΓραμμέςΠΕ.ΠαραστατικάΕισπράξεων = ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr;
            }

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        
        bool arithmosIsFocused;
        public bool ArithmosIsFocused
        {
            set
            {
                SetProperty(ref arithmosIsFocused, value);
                OnArithmosFocusedChanged(value);
            }
        }
        private void OnArithmosFocusedChanged(bool value)
        {
            if (!value)
            {
                if (string.IsNullOrEmpty(Αξιόγραφο.ΑριθμόςΑξιογράφου))
                {
                    //PelatisErrorMessage = "Το ΑΦΜ Δεν Πρέπει να είναι κενό";
                }
                else
                {
                    ArithmosErrorMessage = string.Empty;
                }
            }
        }
        bool axiaIsFocused;
        public bool AxiaIsFocused
        {
            set
            {
                SetProperty(ref axiaIsFocused, value);
                OnAxiaFocusedChanged(value);
            }
        }
        private void OnAxiaFocusedChanged(bool value)
        {
            if (!value)
            {
                if (Αξιόγραφο.Αξία <= 0)
                {
                    //PelatisErrorMessage = "Το ΑΦΜ Δεν Πρέπει να είναι κενό";
                }
                else
                {
                    AxiaErrorMessage = string.Empty;
                }
            }
        }
        bool hmerlixisIsFocused;
        public bool HmerlixisIsFocused
        {
            set
            {
                SetProperty(ref hmerlixisIsFocused, value);
                OnHmerlixisFocusedChanged(value);
            }
        }
        private void OnHmerlixisFocusedChanged(bool value)
        {
            if (!value)
            {
                if (Αξιόγραφο.ΗμνίαΛήξης == DateTime.MinValue)
                {
                    //PelatisErrorMessage = "Το ΑΦΜ Δεν Πρέπει να είναι κενό";
                }
                else
                {
                    HmerlixisErrorMessage = string.Empty;
                }
            }
        }
        bool ChechError()
        {
            CheckArithmos();
            CheckAxia();
            CheckHmerlixis();
            if (!string.IsNullOrEmpty(ArithmosErrorMessage) || !string.IsNullOrEmpty(AxiaErrorMessage) || !string.IsNullOrEmpty(HmerlixisErrorMessage))
            {
                HasError = true;
            }
            else
            {
                HasError = false;
            }
            return HasError;
        }
        void CheckArithmos()
        {
            if (string.IsNullOrEmpty(Αξιόγραφο.ΑριθμόςΑξιογράφου))
            {
                ArithmosErrorMessage = "Ο Αριθμός Αξιογράφου Δεν πρέπει να είναι κενός";
            }
            else
            {
                ArithmosErrorMessage = string.Empty;
            }
        }
        void CheckAxia()
        {
            if (Αξιόγραφο.Αξία <=0)
            {
                AxiaErrorMessage = "Η Αξία Δεν πρέπει να είναι μηδέν";
            }
            else
            {
                AxiaErrorMessage = string.Empty;
            }
        }
        void CheckHmerlixis()
        {
            if (Αξιόγραφο.ΗμνίαΛήξης == DateTime.MinValue)
            {
                HmerlixisErrorMessage = "Η Ημνία Λήξης Δεν πρέπει να είναι κενή";
            }
            else
            {
                HmerlixisErrorMessage = string.Empty;
            }
        }
        public ICommand Αποθήκευση { set; get; }
        public ICommand Ακύρωση { set; get; }
    }
}
