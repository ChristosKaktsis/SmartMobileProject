using SmartMobileProject.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΚαθαρισμόςViewModel : BaseViewModel
    {
        bool allLoading = false;
        public ICommand Καθαρισμός { set; get; }
        public ICommand ΚαθαρισμόςΕίδους { set; get; }
        public ICommand ΚαθαρισμόςΠελάτη { set; get; }
        public ICommand ΚαθαρισμόςΣειράς { set; get; }
        public ΚαθαρισμόςViewModel()
        {
            Shell.Current.FlyoutIsPresented = false;
            Καθαρισμός = new Command(ClearAllDataPressed);
            ΚαθαρισμόςΕίδους = new Command(ClearEidosDataPressed);
            ΚαθαρισμόςΠελάτη = new Command(ClearPelarisDataPressed);
            ΚαθαρισμόςΣειράς = new Command(ClearSeiraDataPressed);
        }

        private async void ClearSeiraDataPressed(object obj)
        {
            IsBusy = true;
            var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", "Θέλετε να διαγράψετε τις σειρές ; ", "Ναί", "Όχι");
            if (answer)
                await XpoHelper.DeleteAllΣεράData();
            IsBusy = false;
        }

        private async void ClearPelarisDataPressed(object obj)
        {
            IsBusy = true;
            var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", "Θέλετε να διαγράψετε τους πελάτες ; ", "Ναί", "Όχι");
            if (answer)
                await XpoHelper.DeleteAllΠελάτηςData();
            IsBusy = false;
        }

        private async void ClearEidosDataPressed(object obj)
        {
            IsBusy = true;
            var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", "Θέλετε να διαγράψετε τα είδη ; ", "Ναί", "Όχι");
            if (answer)
                await XpoHelper.DeleteAllΕίδοςData();
            IsBusy = false;
        }

        private async void ClearAllDataPressed(object obj)
        {
            IsBusy = true;
            var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", "Θέλετε να διαγράψετε όλα τα δεδομένα ; ", "Ναί", "Όχι");
            if (answer)
                await XpoHelper.DeleteAllData();
            IsBusy = false;
        }

        public bool AllLoading
        {
            get { return allLoading; }
            set
            {
                SetProperty(ref allLoading, value);
            }
        }
    }
}
