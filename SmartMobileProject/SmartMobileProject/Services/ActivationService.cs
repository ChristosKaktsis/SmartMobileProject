using SmartMobileProject.Constants;
using SmartMobileProject.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileProject.Services
{
    internal static class ActivationService
    {
        /// <summary>
        /// Check if device is register to Exelixis with company VAT
        /// </summary>
        /// <param name="vat">Company Vat</param>
        /// <returns>true if device is register to exelixis database </returns>
        public static async Task<bool> IsDeviceActive(string vat)
        {
            var license = await LicenseService.GetLicense(vat, InfoStrings.Device_ID);
            if (license == null) return false;
            if (license.Status != Response_Status.Exist) return false;
            return true;
        }
        public static async Task<bool> UseExpired(int limit)
        {
            if (limit <= 4) return false;
            if (string.IsNullOrEmpty(InfoStrings.Device_ID))
            {
                await Shell.Current.DisplayAlert("Η συσκευή δεν έχει ενεργοποιηθεί",
                    "Για να συνεχήσετε να χρησιμοποιείτε την εφαρμογή ενεργοποιήστε την συσκευή απο το μενου Activation.",
                    "OK");
                return true;
            }
            return false;
        }
    }
}
