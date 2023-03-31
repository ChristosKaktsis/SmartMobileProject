using SmartMobileProject.Views;
using SmartMobileProject.Models;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Services;
using System.Linq;

namespace SmartMobileProject.ViewModels
{
    class ΠελάτηςViewModel : BaseViewModel
    {
        public UnitOfWork uow = ((App)Application.Current).uow;
        //UnitOfWork uow;
        AppShell app = (AppShell)Application.Current.MainPage;
        XPCollection<Πελάτης> customerCollection = null;
        public XPCollection<Πελάτης> CustomerCollection
        {
            get { return customerCollection; }
            set { SetProperty(ref customerCollection, value); }
        }
        public ΠελάτηςViewModel()
        {
            Title = "Πελάτες";
            ΝέοςΠελάτης = new Command(createCustomer);
            ΔιαγραφήΠελάτη = new Command(deleteCustomer);
            ΤροποποίησηΠελάτη = new Command(editcustomer);
            ΒρεςΜεΑΦΜ = new Command(GetCustomerWithAFM);
            ΝέοςΠελάτηςΜεΑφμΗΧωρίς = new Command(createNewCustomerWithAFMorWithout);
            Ανανέωση = new Command(ReloadList);
            Κλήση = new Command(CallCustomer);
            Κινήσεις = new Command(GoToKiniseis);
            ΝέοΠαραστατικό = new Command(newOrder);

        }
        public void OnAppearing()
        {
            //uow = new UnitOfWork();

            if (LoadAllCustomers)
                CustomerCollection = new XPCollection<Πελάτης>(uow);
            else
                CustomerCollection = App.Πωλητής.Πελάτες;

            CustomerCollection.DeleteObjectOnRemove = true;
            ReloadList(null);
        }

        private async void GoToKiniseis(object obj)
        {
            try
            {
                if (obj == null)
                    return;

                var CustomerId = ((Πελάτης)obj).SmartOid;
                await Shell.Current.GoToAsync($"{nameof(ΚινήσειςΠελατώνViewPage)}?{nameof(ΚινήσειςΠελατώνViewModel.CustomerID)}={CustomerId}");
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                Console.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Alert", "Other error has occurred.", "OK");
            }
        }

        private async void CallCustomer(object obj)
        {
            try
            {
                var customer = obj as Πελάτης;
                if (customer?.ΚεντρικήΔιευθυνση == null) return;
                if (string.IsNullOrWhiteSpace(customer.ΚεντρικήΔιευθυνση.Τηλέφωνο)) return;
                PhoneDialer.Open(customer.ΚεντρικήΔιευθυνση.Τηλέφωνο);
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

        private async void createNewCustomerWithAFMorWithout(object obj)
        {
            if (!IsTrialOn)
                return;

            var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", "Θέλετε να γίνει συμπλήρωση στοιχείων του πελάτη με βάση το ΑΦΜ ; ", "Ναί", "Όχι");
            if (answer)
            {
                ΒρεςΜεΑΦΜ.Execute(null);
            }
            else
            {
                ΝέοςΠελάτης.Execute(null);
            }

        }

        async void editcustomer(object obj)
        {
            if (!IsTrialOn)
                return;
            if (uow.InTransaction)
            {
                uow.ReloadChangedObjects();
            }
            Πελάτης customer = (Πελάτης)obj;
            app.customer1 = customer;
            //Κανε set to IsFromEdit= true γιατι οταν παταμε πισω να μην διαγραφει τον πελατη
            await Shell.Current.GoToAsync($"{nameof(TestΠελάτηςDetailViewPage)}?{nameof(ΠελάτηςDetailViewModel.IsFromEdit)}={true}");
            //await Shell.Current.GoToAsync($"{nameof(ΠελάτηςDetailTabViewPage)}?{nameof(ΠελάτηςDetailViewModel.IsFromEdit)}={true}");   

        }
        async void newOrder(object obj)
        {
            if (!IsTrialOn)
                return;
            if (uow.InTransaction)
            {
                uow.ReloadChangedObjects();
            }
            Πελάτης customer = (Πελάτης)obj;
            
            if (!IsTrialOn)
                return;
            //setthe static class for new order
            DocHelperViewModel.Order = null;

            DocHelperViewModel.uow = new UnitOfWork();
            //ΝέοΠαραστατικόViewModel.uow = uow;
            //set politi
            //var p = ΝέοΠαραστατικόViewModel.uow.Query<Πωλητής>().Where(x => x.Oid == App.Πωλητής.Oid);
            //ΝέοΠαραστατικόViewModel.politis = p.FirstOrDefault();
            DocHelperViewModel.πελατης = customer;
            await Shell.Current.GoToAsync(nameof(ΠαραστατικόΒασικάΣτοιχείαPage));
        }
        async void deleteCustomer(Object sender)
        {        
            var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", 
                "Θέλετε να γίνει η διαγραφή ", "Ναί", "Όχι");
            
            if (answer)
            {
                try
                {
                    Πελάτης customer = (Πελάτης)sender;
                    await uow.DeleteAsync(customer);
                    if (uow.InTransaction)
                    {
                        uow.CommitChanges();
                    }
                    ReloadList(null);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Σφάλμα",
                    "Κάτι πήγε λάθος στην διαγραφή !", "ΟΚ");
                    Console.WriteLine(ex);
                }
            }                 
        }
        private async void createCustomer(object obj)
        {                          
            app.customer1 = null;
            await Shell.Current.GoToAsync($"{nameof(TestΠελάτηςDetailViewPage)}?{nameof(ΠελάτηςDetailViewModel.IsFromEdit)}={false}");
            //await Shell.Current.GoToAsync($"{nameof(ΠελάτηςDetailTabViewPage)}?{nameof(ΠελάτηςDetailViewModel.IsFromEdit)}={false}");
            CustomerCollection.Reload();
        }
        private async void GetCustomerWithAFM(object obj)
        {
            ΣτοιχείαΕταιρίας στοιχεία = await XpoHelper.GetSTOIXEIAETAIRIASData();
            string result = await Application.Current.MainPage.DisplayPromptAsync("Αναζήτηση στοιχείων με το ΑΦΜ", 
                "Γράψε το ΑΦΜ", 
                "Αποδοχή", "Ακύρωση");
            if (result == null)
            {
                return;
            }
            AFM = result;
            CallAfmInfo callAfmInfo = new CallAfmInfo(στοιχεία);
            Πελάτης customer = new Πελάτης(uow);
            customer.SmartOid = Guid.NewGuid();
            app.customer1 = customer;
            App.Πωλητής.Πελάτες.Add(customer);
            callAfmInfo.setCustomerWithAfm(AFM, customer);

            if (callAfmInfo.error != "")
            {
                customer.Delete();
                await Application.Current.MainPage.DisplayAlert("Alert", callAfmInfo.error, "OK");
            } else
            {
                await Shell.Current.GoToAsync($"{nameof(TestΠελάτηςDetailViewPage)}?{nameof(ΠελάτηςDetailViewModel.IsFromEdit)}={false}");
                //await Shell.Current.GoToAsync($"{nameof(ΠελάτηςDetailTabViewPage)}?{nameof(ΠελάτηςDetailViewModel.IsFromEdit)}={false}");
                CustomerCollection.Reload();
            }
        }
        private void ReloadList(object obj)
        {
            CustomerCollection.Reload();
            if (uow.InTransaction)
            {
                uow.ReloadChangedObjects();
            }
        }
        private string afm;
        public string AFM
        {
            get
            {
                return afm;
            }
            set
            {
                afm = value;
                SetProperty(ref afm, value);
            }
        }
        public ICommand ΝέοςΠελάτης { get; }
        public ICommand Ανανέωση { get; }
        public ICommand ΤροποποίησηΠελάτη { get; }
        public ICommand ΝέοΠαραστατικό { get; }
        public ICommand ΔιαγραφήΠελάτη { get; }
        /// <summary>
        /// Καλειται η setCustomerwithafm και γεμιζει τον customer
        /// αλλιως εμφανιζει το μηνυμα λαθους που επιστρεφει το service του Υπουργειου
        /// </summary>
        public ICommand ΒρεςΜεΑΦΜ { set; get; }
        public ICommand Κλήση { set; get; }
        public ICommand Κινήσεις { set; get; }
        public ICommand ΝέοςΠελάτηςΜεΑφμΗΧωρίς { set; get; }


    }
}
