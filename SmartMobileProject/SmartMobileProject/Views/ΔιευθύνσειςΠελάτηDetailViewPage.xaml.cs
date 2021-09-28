using SmartMobileProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ΔιευθύνσειςΠελάτηDetailViewPage : ContentPage
    {
        ΔιευθύνσειςΠελάτηDetailViewModel model;
        public ΔιευθύνσειςΠελάτηDetailViewPage()
        {
            InitializeComponent();
            model = BindingContext as ΔιευθύνσειςΠελάτηDetailViewModel;
            map.Pins.Remove(map.Pins.FirstOrDefault());
            Pin pin = new Pin
            {
                Label = model.name,
                Address = model.Address.Addresstring,
                Type = PinType.Place,
                Position = new Position(model.Address.ΓεωγραφικόΠλάτος, model.Address.ΓεωγραφικόΜήκος)
            };
            map.Pins.Add(pin);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(model.Address.ΓεωγραφικόΠλάτος, model.Address.ΓεωγραφικόΜήκος),
                                             Distance.FromKilometers(0.5)));
        }
    }
}