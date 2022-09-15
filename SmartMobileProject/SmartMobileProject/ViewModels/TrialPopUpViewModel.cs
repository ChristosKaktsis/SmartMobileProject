using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class TrialPopUpViewModel : BaseViewModel
    {
        string bottomtext = "Επιλέξτε την διαδικασία που θέλετε να ακολουθήσετε.";
        public string BottomText
        {
            get => bottomtext;
            set
            {
                SetProperty(ref bottomtext, value);
            }
        }
        bool isselected;
        public bool ActiveIsSelected
        {
            get => isselected;
            set
            {
                SetProperty(ref isselected, value);
            }
        }
        ΣτοιχείαΕταιρίας εταιρία;
        public ΣτοιχείαΕταιρίας Εταιρία
        {
            get
            {
                using(UnitOfWork uow = new UnitOfWork())
                {
                    var eta = uow.Query<ΣτοιχείαΕταιρίας>();
                    if (eta.Any())
                        εταιρία = eta.FirstOrDefault();

                }
                return εταιρία;
            }
            
        }
        public Πωλητής Πωλητής
        {
            get
            {
                return ((AppShell)Application.Current.MainPage).πωλητής;
            }
            
        }
        public void TrialButton()
        {
            BottomText = "Δοκιμάστε το για 30 μέρες. " +
                         "Μετα η εφαρμογη θα κλειδώσει και " +
                         "δεν θα μπορειτε να κάνετε παραπάνω ενέργειες." +
                         " Για ενεργοποιηση πατήστε το Start today και πατήστε continue";
            ActiveIsSelected = false;
        }
        public void ActivationButton()
        {
            BottomText = "Πατήστε continue για να μας στείλετε με email την αίτηση ενεργοποίησης. " +
                         "Με την ολοκλήρωση θα σας στείλουμε με email " +
                         "τον κωδικό ενεργοποίησης τον οποίο τον εισάγετε πατώντας Activate Product ";
            ActiveIsSelected = true;
        }
        public async void ContinuePressed()
        {
            try
            {
                if (!ActiveIsSelected)
                    return;
                
                //Get id
                string Id = GetId();
                Preferences.Set("ID", Id);
                //crypto id
                string crypto = CryptoId(Id);
                //get compncredetial
                //set email body
                var body = await BuildBody(crypto);
                //send mail
                await EmailSender.SendEmail(
                    "Αίτηση ενεργοποίησης",
                    body,
                    new List<string> { "license@exelixis-software.gr" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Προσοχή",
                $"Κάτι πήγε στραβά! \n {ex}", "Εντάξει");
            }
        }

        private async Task<string> BuildBody(string crypto)
        {
            if (Εταιρία == null)
            {
                await Application.Current.MainPage.DisplayAlert("Προσοχή",
                "Συμπληρώστε τα στοιχεία εταιρίας πριν κάνετε αίτηση ενεργοποίησης", "Εντάξει");
                return string.Empty;
            }
            string Εταιρία_ΔΟΥ_Περιγραφή = "";
            if (Εταιρία.ΔΟΥ != null)
                Εταιρία_ΔΟΥ_Περιγραφή = Εταιρία.ΔΟΥ.Περιγραφή;
            string Εταιρία_Πολη = "";
            if (Εταιρία.Πόλη != null)
                Εταιρία_Πολη = Εταιρία.Πόλη.ΟνομαΠόλης;

            string body = "";
            body += "  --Στοιχεία Εταιρίας--\n" +
                    "Επωνυμία :" + Εταιρία.Επωνυμία + "\n" +
                    "Οδός :" + Εταιρία.Οδός + "\n" +
                    "Αριθμός :" + Εταιρία.Αριθμός + "\n" +
                    "Περιοχή :" + Εταιρία.Περιοχή + "\n" +
                    "Πόλη :" + Εταιρία_Πολη + "\n" +
                    "Τηλ :" + Εταιρία.Τηλέφωνο + "\n" +
                    "Email :" + Πωλητής.Email + "\n" +
                    "ΑΦΜ :" + Εταιρία.ΑΦΜ + "\n" +
                    "ΔΟΥ :" + Εταιρία_ΔΟΥ_Περιγραφή + "\n \n \n" +
                    "  --Στοιχεία Πωλητή--\n" +
                    "Ονοματεπώνυμο :" + Πωλητής.Ονοματεπώνυμο + "\n" +
                    "Οδός :" + Πωλητής.Οδός + "\n" +
                    "Αριθμός :" + Πωλητής.Αριθμός + "\n" +
                    "Τηλ :" + Πωλητής.KίνΤηλέφωνο + "\n" +
                    "StandAlone :" + (OnlineMode ? "Οχι" : "Ναι") + "\n" +
                    "Activation :" + crypto + "\n";
            return body;
        }
        private string CryptoId(string id)
        {
            string crypto = AesOperation.EncryptString("b14ca5898a4e4133bbce2ea2315a1916", id);
            return crypto;
        }
        private string GetId()
        {
            return DependencyService.Get<IDevice>().GetIdentifier();
        }
    }
}
