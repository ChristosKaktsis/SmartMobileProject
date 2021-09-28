using SmartMobileProject.Models;
using SmartMobileProject.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
    }
}
