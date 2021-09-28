using DevExpress.XamarinForms.CollectionView;
using SmartMobileProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ΕργασίεςViewPage : ContentPage
    {
        ΕργασίεςViewModel model;
        public ΕργασίεςViewPage()
        {
            InitializeComponent();
            model = BindingContext as ΕργασίεςViewModel;
            BottomSheet.Opacity = 0;
            BottomSheet.TranslationY = 500;
        }
        void SwipeItem_Invoked(System.Object sender, SwipeItemTapEventArgs e)
        {
            this.grid.DeleteItem(e.ItemHandle);
            model.Διαγραφή.Execute(e.Item);
        }
        private async void plusBtn_Clicked(object sender, EventArgs e)
        {
            BackGreyColor.IsVisible = true;
            model.ΝέαΕργασία.Execute(null);
            await Task.WhenAll(
                BottomSheet.TranslateTo(0, 0, 100, Easing.SinIn),
                BottomSheet.FadeTo(1, 200));
            
        }
        async void ClosePopup_Clicked(System.Object sender, System.EventArgs e)
        {
            BackGreyColor.IsVisible = false;
            model.Reload.Execute(null);
            await Task.WhenAll(
                BottomSheet.FadeTo(0, 250),
                BottomSheet.TranslateTo(0, 600, 100)
                );
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            BackGreyColor.IsVisible = false;
            model.Αποθήκευση.Execute(null);
            await Task.WhenAll(
                BottomSheet.FadeTo(0, 250),
                BottomSheet.TranslateTo(0, 600, 100)
                );  
        }

        private  void grid_Tap(object sender, CollectionViewGestureEventArgs e)
        {
            model.CheckTask(e.Item);
           // this.grid.MoveItem(e.ItemHandle, grid.ItemCount - 1, null);
        }

        private void SwipeItem_Invoked_Edit(object sender, SwipeItemTapEventArgs e)
        {
            model.Επεξεργασία.Execute(e.Item);
           
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            model.Εργασίες.Reload();
            model.Πελάτες.Reload();
        }
    }
}