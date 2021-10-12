using SmartMobileProject.Droid;
using SmartMobileProject.Services;

[assembly: Xamarin.Forms.Dependency(typeof(UniqueIdAndroid))]
namespace SmartMobileProject.Droid
{
    public class UniqueIdAndroid : IDevice
    {
        public string GetIdentifier()
        {
            
            return Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, 
                Android.Provider.Settings.Secure.AndroidId);
        }
    }
}