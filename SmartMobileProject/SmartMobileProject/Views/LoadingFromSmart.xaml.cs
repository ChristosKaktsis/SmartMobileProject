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
    public partial class LoadingFromSmart : ContentPage
    {
        public LoadingFromSmart()
        {
            InitializeComponent();
            //BottomSheet.Opacity = 0;
        }
        private async void EIDOS_Button_Clicked(object sender, EventArgs e)
        {
            doneEidos.Scale = 0;
            base.OnBindingContextChanged();
            if (!(BindingContext is LoadFromSmartModel model))
                return;
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                IsEnabled = false
            });
            var error = await model.LoadItems();
            if (!error)
                doneEidos.Source = ImageSource.FromFile("error.png");
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                IsEnabled = true
            });        
            if (!model.AllLoading)
            await Task.WhenAll(
                doneEidos.ScaleTo(1, 300)
                );
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            doneSchedule.Scale = 0;
            base.OnBindingContextChanged();
            if (!(BindingContext is LoadFromSmartModel model))
                return;
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                IsEnabled = false
            });
            var error = await model.LoadAppointments();
            if (!error)
                doneSchedule.Source = ImageSource.FromFile("error.png");
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                IsEnabled = true
            });  
            if (!model.AllLoading)
                await Task.WhenAll(
                    doneSchedule.ScaleTo(1, 300)
                    );
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            done.Scale = 0;
            base.OnBindingContextChanged();
            if (!(BindingContext is LoadFromSmartModel model))
                return;
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                IsEnabled = false
            });
            var error = await model.LoadAll();
            if (!error)
                done.Source = ImageSource.FromFile("error.png");
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                IsEnabled = true
            });
            if (!model.AllLoading)
                await Task.WhenAll(
                    done.ScaleTo(1, 300)
                    );
        }

        private async void Pelates_Button_Clicked(object sender, EventArgs e)
        {
            donePelates.Scale = 0;
            base.OnBindingContextChanged();
            if (!(BindingContext is LoadFromSmartModel model))
                return;
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                IsEnabled = false
            });
            var error = await model.LoadPelates();
            if (!error)
                donePelates.Source = ImageSource.FromFile("error.png");
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                IsEnabled = true
            });
            if (!model.AllLoading)
                await Task.WhenAll(
                    donePelates.ScaleTo(1, 300)
                    );
        }
        protected override bool OnBackButtonPressed()
        {
            if (!(BindingContext is LoadFromSmartModel model))
                return false;
            return model.AllLoading;
        }
        //private async Task OpenPopUp(bool ok)
        //{
        //    //
        //    //BackGreyColor.IsVisible = ok;
        //    if (ok)
        //    {
        //        await BottomSheet.FadeTo(1, 200);
        //    }
        //    else
        //    {
        //        await BottomSheet.FadeTo(0, 250);
        //    }

        //    //
        //}
    }
}