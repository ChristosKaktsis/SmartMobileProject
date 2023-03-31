using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΓραμμέςΠαραστατικώνΕισπράξεωνDetailViewModel : BaseViewModel
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
        public XPCollection<ΛογαριασμοίΧρηματικώνΔιαθέσιμων> Λογαριασμοί { get; set; }
        // error messages
        string logarisasmoserrormessage;
        public bool hasError = false;
        public bool HasError
        {
            get { return hasError; }
            set
            {
                SetProperty(ref hasError, value);
            }
        }
        public string LogariasmosErrorMessage
        {
            get
            {
                return logarisasmoserrormessage;
            }
            set
            {
                SetProperty(ref logarisasmoserrormessage, value);
            }
        }
        public ΓραμμέςΠαραστατικώνΕισπράξεωνDetailViewModel()
        {
            uow = DocCollectHelper.uow;
            if (DocCollectHelper.editline == null)
            {
                ΓραμμέςΠΕ = new ΓραμμέςΠαραστατικώνΕισπράξεων(uow);
                ΓραμμέςΠΕ.SmartOid = Guid.NewGuid();
            }
            else
            {
                ΓραμμέςΠΕ = DocCollectHelper.editline;
            }
            
            Λογαριασμοί = new XPCollection<ΛογαριασμοίΧρηματικώνΔιαθέσιμων>(uow);
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
                ΓραμμέςΠΕ.ΠαραστατικάΕισπράξεων = DocCollectHelper.ParastatikoEispr;
            }

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        
        bool logariasmosIsFocused;
        public bool LogariasmosIsFocused
        {
            set
            {
                SetProperty(ref logariasmosIsFocused, value);
                OnLogariasmosFocusedChanged(value);
            }
        }
        private void OnLogariasmosFocusedChanged(bool value)
        {
            if (!value)
            {
                if (ΓραμμέςΠΕ.Λογαριασμός == null)
                {
                    //PelatisErrorMessage = "Το ΑΦΜ Δεν Πρέπει να είναι κενό";
                }
                else
                {
                    LogariasmosErrorMessage = string.Empty;
                }
            }
        }
        bool ChechError()
        {

            CheckLogariasmos();
            if (!string.IsNullOrEmpty(LogariasmosErrorMessage))
            {
                HasError = true;
            }
            else
            {
                HasError = false;
            }
            return HasError;
        }
        void CheckLogariasmos()
        {
            if (ΓραμμέςΠΕ.Λογαριασμός == null)
            {
                LogariasmosErrorMessage = "Ο Λογαριασμός δεν πρέπει να είναι κενός";
            }
            else
            {
                LogariasmosErrorMessage = string.Empty;
            }
        }
        public ICommand Αποθήκευση { set; get; }
        public ICommand Ακύρωση { set; get; }
    }
}
