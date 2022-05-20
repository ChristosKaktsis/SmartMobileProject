using SmartMobileProject.Models;
using SmartMobileProject.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
       

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }



        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
       

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public async void GoBack(object obj)
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Θέλετε να αποχωρήσετε ;",
              "Πιθανόν αλλαγές δεν θα αποθηκευτούν", "Ναί", "Όχι");
            if (answer)
            {
                await Shell.Current.Navigation.PopAsync();
            }
        }
        public async void Cancel(object obj)
        {
            await Shell.Current.Navigation.PopAsync();
        }
        public bool OnlineMode
        {
            get => Preferences.Get(nameof(OnlineMode), false);
            set
            {
                if (!Preferences.Get(nameof(OnlineMode), false))
                {
                    OnlineWarning();
                    
                }
                    
                Preferences.Set(nameof(OnlineMode), value);
                OnPropertyChanged(nameof(OnlineMode));
            }
        }
        public bool Lock
        {
            get => Preferences.Get(nameof(Lock), false);
            set
            {
                OnPropertyChanged(nameof(Lock));
            }
        }
        public bool IsTrialOn
        {
            get 
            {
                if(Lock)
                    TrialMessage();
                return !Lock; 
            }
        }
        public bool Active
        {
            get => Preferences.Get(nameof(Active), false);
            set
            {
                Preferences.Set(nameof(Active), value);
                OnPropertyChanged(nameof(Active));
            }
        }
        public bool LoadAllCustomers
        {
            get => Preferences.Get(nameof(LoadAllCustomers), false);
            set
            {
                Preferences.Set(nameof(LoadAllCustomers), value);
                OnPropertyChanged(nameof(LoadAllCustomers));
            }
        }
        public bool LACFlag
        {
            get => Preferences.Get(nameof(LACFlag), false);
            set
            {
                Preferences.Set(nameof(LACFlag), value);
                OnPropertyChanged(nameof(LACFlag));
            }
        }
        public async void TrialMessage()
        {
            await Application.Current.MainPage.DisplayAlert("Free Trial",
                     "Έχει λήξει ή δωρεάν δοκιμή 30 ημερών. Για να ενεργοποιήσετε την εφαρμογή " +
                     "πατήστε Activate Product απο το menu Activation", "Εντάξει");
        }
        public async void OnlineWarning()
        {
            await Application.Current.MainPage.DisplayAlert("Προσοχή",
                     "Μόνο τα δεδομένα που υπάρχουν στο Smart μπορούν να επικοινωνήσουν με το mobile. " +
                     "Οποιαδήποτε ενέργεια κάνετε στο Stand Alone Θα διαγραφεί ", "Εντάξει");
        }
    }
}
