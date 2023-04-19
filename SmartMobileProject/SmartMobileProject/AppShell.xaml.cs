using SmartMobileProject.Models;
using SmartMobileProject.Views;
using SmartMobileProject.Views.Settings;
using System;

using Xamarin.Forms;

namespace SmartMobileProject
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public Πελάτης customer1;
        
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(UploadToSmart), typeof(UploadToSmart));
            Routing.RegisterRoute(nameof(LoadingFromSmart), typeof(LoadingFromSmart));
            Routing.RegisterRoute(nameof(ΠαραστατικόΒασικάΣτοιχείαPage), typeof(ΠαραστατικόΒασικάΣτοιχείαPage));
            Routing.RegisterRoute(nameof(ΕπιλογήΠροηγΠαρΠωλPage), typeof(ΕπιλογήΠροηγΠαρΠωλPage));
            Routing.RegisterRoute(nameof(ΓραμμέςΠαραστατικώνΠωλήσεωνListViewPage), typeof(ΓραμμέςΠαραστατικώνΠωλήσεωνListViewPage));
            Routing.RegisterRoute(nameof(ΓραμμέςΠαραστατικώνΠωλήσεωνDetailViewPage), typeof(ΓραμμέςΠαραστατικώνΠωλήσεωνDetailViewPage));
            Routing.RegisterRoute(nameof(ΓραμμέςΠαραστατικώνΠωλήσεωνQuickPickDetailViewPage), typeof(ΓραμμέςΠαραστατικώνΠωλήσεωνQuickPickDetailViewPage));
            Routing.RegisterRoute(nameof(ImageProductsPage), typeof(ImageProductsPage));
            Routing.RegisterRoute(nameof(ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodePage), typeof(ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodePage));
            Routing.RegisterRoute(nameof(ScannerViewPage), typeof(ScannerViewPage));
            Routing.RegisterRoute(nameof(ΠαραστατικόΟλοκλήρωσηViewPage), typeof(ΠαραστατικόΟλοκλήρωσηViewPage));
            Routing.RegisterRoute(nameof(ΠαραστατικόΕισπράξεωνΒασικάΣτοιχείαPage), typeof(ΠαραστατικόΕισπράξεωνΒασικάΣτοιχείαPage));
            Routing.RegisterRoute(nameof(ΓραμμέςΠαραστατικώνΕισπράξεωνListViewPage), typeof(ΓραμμέςΠαραστατικώνΕισπράξεωνListViewPage));
            Routing.RegisterRoute(nameof(ΛογαριασμόςDetailViewPage), typeof(ΛογαριασμόςDetailViewPage));
            Routing.RegisterRoute(nameof(ΑξιόγραφοDetailViewPage), typeof(ΑξιόγραφοDetailViewPage));
            Routing.RegisterRoute(nameof(ΕίδοςDetailViewPage), typeof(ΕίδοςDetailViewPage));
            Routing.RegisterRoute(nameof(TestΠελάτηςDetailViewPage), typeof(TestΠελάτηςDetailViewPage));
            Routing.RegisterRoute(nameof(ΔιευθύνσειςΠελάτηListViewPage), typeof(ΔιευθύνσειςΠελάτηListViewPage));
            Routing.RegisterRoute(nameof(ΔιευθύνσειςΠελάτηDetailViewPage), typeof(ΔιευθύνσειςΠελάτηDetailViewPage));
            Routing.RegisterRoute(nameof(ΕργασίεςDetailViewPage), typeof(ΕργασίεςDetailViewPage));
            Routing.RegisterRoute(nameof(ΕνέργειαDetailViewPage), typeof(ΕνέργειαDetailViewPage));
            Routing.RegisterRoute(nameof(BarCodeListViewPage), typeof(BarCodeListViewPage));
            Routing.RegisterRoute(nameof(BarCodeDetailViewPage), typeof(BarCodeDetailViewPage));
            Routing.RegisterRoute(nameof(ΚινήσειςΠελατώνViewPage), typeof(ΚινήσειςΠελατώνViewPage));
            Routing.RegisterRoute("Settings/PrintSettingsPage", typeof(PrintSettingsPage));
            Routing.RegisterRoute("Settings/ΣειρέςΠαρΕισπPage", typeof(ΣειρέςΠαρΕισπPage));
            Routing.RegisterRoute("Settings/ΣτοιχείαΕταιρίαςPage", typeof(ΣτοιχείαΕταιρίαςPage));
            Routing.RegisterRoute("Settings/ΠόληPage", typeof(ΠόληPage));
            Routing.RegisterRoute("Settings/ΦΠΑPage", typeof(ΦΠΑPage));
            Routing.RegisterRoute("Settings/ΤΚPage", typeof(ΤΚPage));
            Routing.RegisterRoute("Settings/ΔΟΥPage", typeof(ΔΟΥPage));
            Routing.RegisterRoute("Settings/ΕίδοςΟικογένειαPage", typeof(ΕίδοςΟικογένειαPage));
            Routing.RegisterRoute("Settings/ΕίδοςΥποοικογένειαPage", typeof(ΕίδοςΥποοικογένειαPage));
            Routing.RegisterRoute("Settings/ΕίδοςΟμάδαPage", typeof(ΕίδοςΟμάδαPage));
            Routing.RegisterRoute("Settings/ΕίδοςΚατηγορίεςPage", typeof(ΕίδοςΚατηγορίεςPage));
            Routing.RegisterRoute("Settings/ΛογαριασμοίΧρημΔιαθPage", typeof(ΛογαριασμοίΧρημΔιαθPage));
            Routing.RegisterRoute("Settings/ΚαθαρισμόςPage", typeof(ΚαθαρισμόςPage));
            Routing.RegisterRoute("Settings/ΤρόποςΠληρωμήςPage", typeof(ΤρόποςΠληρωμήςPage));
            Routing.RegisterRoute("Settings/ΤρόποςΑποστολήςPage", typeof(ΤρόποςΑποστολήςPage));
            Routing.RegisterRoute("Settings/ΜεταφορικόΜέσοPage", typeof(ΜεταφορικόΜέσοPage));
            Routing.RegisterRoute("Settings/ΠρότυπαPage", typeof(ΠρότυπαPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(LoadingFromSmart));
        }
        bool isLeaving = false;
        protected override bool OnBackButtonPressed()
        {
            var st = Shell.Current.Navigation.NavigationStack;
            var item = Shell.Current.CurrentItem.Route;
            if (st.Count == 1 && item != "MainPage" && item != "LoginPage")
            {
                isLeaving = true;
            }
            return base.OnBackButtonPressed();

        }
        protected async override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);
            if (isLeaving)
            {
                args.Cancel();
                isLeaving = false;
                await Shell.Current.GoToAsync("///MainPage");
            }

        }

        private async void Upload_MenuItem_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(UploadToSmart));
        }
       
        private async void Exit_MenuItem_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///LoginPage");
        }
    }
}
