using SmartMobileProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadToSmart : ContentPage
    {
        public UploadToSmart()
        {
            InitializeComponent();
        }
      
        private async void Button_Clicked(object sender, EventArgs e)
        {
            done.Scale = 0;
            done.Source = ImageSource.FromFile("checkmark.png");
            base.OnBindingContextChanged();
            if (!(BindingContext is UploadToSmartModel model))
                return;
            var error = await model.SendAllItems();
            if (!error)
                done.Source = ImageSource.FromFile("error.png");
            if (!model.AllItemsLoading)
                await Task.WhenAll(
                    done.ScaleTo(1, 300)
                    );
        }
        private async void Pelates_Button_Clicked(object sender, EventArgs e)
        {
            done1.Scale = 0;
            done1.Source = ImageSource.FromFile("checkmark.png");
            base.OnBindingContextChanged();
            if (!(BindingContext is UploadToSmartModel model))
                return;
            var error = await model.SendPelatesItems();
            if(!error)
                done1.Source = ImageSource.FromFile("error.png");
            if (!model.PelatesItemsLoading)
                await done1.ScaleTo(1, 300);
        }
        private async void Poliseis_Button_Clicked(object sender, EventArgs e)
        {
            done2.Scale = 0;
            done2.Source = ImageSource.FromFile("checkmark.png");
            base.OnBindingContextChanged();
            if (!(BindingContext is UploadToSmartModel model))
                return;
            var error = await model.SendPoliseisItems();
            if (!error)
                done2.Source = ImageSource.FromFile("error.png");
            if (!model.PoliseisItemsLoading)
                await Task.WhenAll(
                    done2.ScaleTo(1, 300)
                    );
        }
        private async void Eispraxeis_Button_Clicked(object sender, EventArgs e)
        {
            done3.Scale = 0;
            done3.Source = ImageSource.FromFile("checkmark.png");
            base.OnBindingContextChanged();
            if (!(BindingContext is UploadToSmartModel model))
                return;
            var error = await model.SendEispraxeisItems();
            if (!error)
                done3.Source = ImageSource.FromFile("error.png");
            if (!model.EispraxeisItemsLoading)
                await Task.WhenAll(
                    done3.ScaleTo(1, 300)
                    );
        }
    }
}