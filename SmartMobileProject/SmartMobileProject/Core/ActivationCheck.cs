using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartMobileProject.Core
{
    class ActivationCheck
    {
        public static Task<bool> CheckActivationCode()
        {
            return Task.Run(() =>
            {
                Preferences.Set("Lock", false);
                Preferences.Set("Active", true);
                return true;
            });
        }
    }
}
