using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using DevExpress.Xpo;
using System.Windows.Input;
using Xamarin.Essentials;

namespace SmartMobileProject.ViewModels
{
    class ΔιευθύνσειςΠελάτηViewModel : BaseViewModel
    {
        public UnitOfWork uow = ((App)Application.Current).uow;
        AppShell appShell = (AppShell)Application.Current.MainPage;
        XPCollection<ΔιευθύνσειςΠελάτη> addressCollection = null;
        public XPCollection<ΔιευθύνσειςΠελάτη> AddressCollection
        {
            get { return addressCollection; }
            set { SetProperty(ref addressCollection, value); }
        }

        public ΔιευθύνσειςΠελάτηViewModel()
        {
            AddressCollection = appShell.customer1.ΔιευθύνσειςΠελάτη;
            AddressCollection.DeleteObjectOnRemove = true;
            Title = appShell.customer1.DisplayName;
            ΝέαΔιεύθυνση = new Command(createAddress);
            ΕπεξεργασίαΔιεύθυνσης = new Command(EditAddress);
            Κλήση = new Command(CallCustomer);
            ΔιαγραφήΔιεύθυνσης = new Command(deleteAddress);
            Ακύρωση = new Command(Cancel);
            if (!uow.InTransaction)
            {
                AddressCollection.Reload();
            }
        }

       
        private async void createAddress(object obj)
        {
            ΔιευθύνσειςΠελάτηDetailViewModel.editAddress = null;
           await Shell.Current.GoToAsync($"{nameof(ΔιευθύνσειςΠελάτηDetailViewPage)}?{nameof(ΔιευθύνσειςΠελάτηDetailViewModel.IsFromEdit)}={false}");
           // AddressCollection.Reload();
        }
        private async void EditAddress(object obj)
        {
            ΔιευθύνσειςΠελάτηDetailViewModel.editAddress = (ΔιευθύνσειςΠελάτη)obj;
            await Shell.Current.GoToAsync($"{nameof(ΔιευθύνσειςΠελάτηDetailViewPage)}?{nameof(ΔιευθύνσειςΠελάτηDetailViewModel.IsFromEdit)}={true}");
        }
        private async void CallCustomer(object obj)
        {
            try
            {
                PhoneDialer.Open(((ΔιευθύνσειςΠελάτη)obj).Τηλέφωνο);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
                Console.WriteLine(anEx);
                await Application.Current.MainPage.DisplayAlert("Alert", "Number was null or white space", "OK");
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
                Console.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Alert", "Phone Dialer is not supported on this device.", "OK");
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                Console.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Alert", "Other error has occurred.", "OK");
            }
        }
        private async void deleteAddress(object obj)
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", "Θέλετε να γίνει η διαγραφή ", "Ναί", "Όχι");

            if (answer)
            {
                ΔιευθύνσειςΠελάτη address = (ΔιευθύνσειςΠελάτη)obj;
                address.Delete();              
            }
        }

        public ICommand ΝέαΔιεύθυνση { get; set; }
        public ICommand ΕπεξεργασίαΔιεύθυνσης { get; set; }
        public ICommand Κλήση { set; get; }
        public ICommand ΔιαγραφήΔιεύθυνσης { get; set; }
        public ICommand Ακύρωση { get; set; }
    }
}
