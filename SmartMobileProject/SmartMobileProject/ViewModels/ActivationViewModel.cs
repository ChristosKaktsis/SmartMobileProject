using SmartMobileProject.Constants;
using SmartMobileProject.Core;
using SmartMobileProject.Network;
using SmartMobileProject.Repositories;
using SmartMobileProject.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ActivationViewModel : BaseViewModel
    {
        public Command Register_Device_Command { get; }
        public Command Delete_Device_Command { get; }
        private CompanyRepository repository = new CompanyRepository();
        private string vat;
        public ActivationViewModel()
        {
            Register_Device_Command = new Command(async () => await RegisterDevice());
            Delete_Device_Command = new Command(async () => await DeleteDevice());
        }
        private async Task GetVat()
        {
            try
            {
                vat = await repository.GetVAT();
            }
            catch
            {
                Debug.WriteLine("GetVat() exeption !!!");
            }
            finally
            {
                if (string.IsNullOrEmpty(vat)) {
                    await Shell.Current.DisplayAlert(
                        "Το ΑΦΜ απο τα στοιχεία εταιρίας είναι κενό.", 
                        "Πρέπει να είναι συμπληρωμένα τα στοιχεία εταιρίας για να ενεργοποιήσετε την εφαρμογή", 
                        "ΟΚ");
                }
            }
        }
        private async Task RegisterDevice()
        {
            IsBusy = true;
            try
            {
                if (await DeviceExists(vat))
                    return;
                var result = await LicenseService.AddLicense(vat);
                if (result == null)
                {
                    ToastResult(Response_Status.Not_Saved);
                    return;
                }
                ToastResult(result.Status);

                InfoStrings.Device_ID = result.ID;
                Device_Number = result.Device_Number;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally { IsBusy = false; }
        }
        private async Task DeleteDevice()
        {
            IsBusy = true;
            try
            {
                var result = await LicenseService.DeleteLicense(vat, InfoStrings.Device_ID);
                if (result == null)
                {
                    ToastResult(Response_Status.Not_Saved);
                    return;
                }
                if (result.Status == Response_Status.Success)
                {
                    InfoStrings.Device_ID = result.ID;
                    Device_Number = result.Device_Number;
                    await Shell.Current.DisplayAlert("Διαγραφή Συσκευής Επιτυχής.", "", "ΟΚ");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally { IsBusy = false; }
        }
        private async Task<bool> DeviceExists(string vat)
        {
            var exist_id = await LicenseService.GetLicense(vat, InfoStrings.Device_ID);
            if (exist_id == null) return false;
            if (exist_id.Status == Response_Status.Exist)
            {
                ToastResult(exist_id.Status);
                return true;
            }
            return false;
        }
        private async void ToastResult(Response_Status error)
        {
            if (error == Response_Status.Success)
                await Shell.Current.DisplayAlert("Εγγραφή Συσκευής Επιτυχής.", "", "ΟΚ");
            if (error == Response_Status.Exist)
                await Shell.Current.DisplayAlert("Η Συσκευή υπάρχει στο σύστημα", "", "ΟΚ");
            if (error == Response_Status.Limit_Reached)
                await Shell.Current.DisplayAlert("Εγγραφή Συσκευής δεν μπορεί να πραγματοποιηθεί.",
                    "Εγγραφή Συσκευής δεν μπορεί να πραγματοποιηθεί επειδή έχετε συμπληρώσει το όριο αδειών", "ΟΚ");
            if (error == Response_Status.Not_Found)
                await Shell.Current.DisplayAlert("Εγγραφή Συσκευής δεν μπορεί να πραγματοποιηθεί.",
                    "Εγγραφή Συσκευής δεν μπορεί να πραγματοποιηθεί επειδή δεν βρέθηκε αριθμός σειράς πελάτη με το ΑΦΜ σας", "ΟΚ");
            if (error == Response_Status.Not_Saved)
                await Shell.Current.DisplayAlert("Εγγραφή Συσκευής δεν πραγματοποιήθηκε",
                    "", "ΟΚ");
        }
        private string _Device_Number;
        public string Device_Number
        {
            get => _Device_Number;
            set => SetProperty(ref _Device_Number, value);
        }
        public async void OnAppearing()
        {
            await GetVat();
            await CheckDevice();
        }
        private async Task CheckDevice()
        {
            IsBusy = true;
            try
            {
                var exist_id = await LicenseService.GetLicense(vat, InfoStrings.Device_ID);
                if (exist_id == null)
                {
                    await Shell.Current.DisplayAlert("Η Συσκευή δεν υπάρχει στο σύστημα",
                    "", "ΟΚ");
                    return;
                }
                if (exist_id.Status != Response_Status.Exist)
                {
                    await Shell.Current.DisplayAlert("Η Συσκευή δεν υπάρχει στο σύστημα",
                    "", "ΟΚ");
                    return;
                }
                Device_Number = exist_id.Device_Number;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally { IsBusy = false; }
        }
        private string GetId()
        {
            return DependencyService.Get<IDevice>().GetIdentifier();
        }
    }
}
