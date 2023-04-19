using Xamarin.Essentials;

namespace SmartMobileProject.Constants
{
    internal static class InfoStrings
    {
        public static string Device_ID //Device id for license
        {
            get => Preferences.Get(nameof(Device_ID), string.Empty);
            set
            {
                Preferences.Set(nameof(Device_ID), value);
            }
        }
        public static int Demo_Limit
        {
            get => Preferences.Get(nameof(Demo_Limit), 0);
            set
            {
                Preferences.Set(nameof(Demo_Limit), value);
            }
        }
    }
}
