using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΚαθαρισμόςViewModel : BaseViewModel
    {
        bool allLoading = false;
        public ICommand Καθαρισμός { set; get; }
        public ΚαθαρισμόςViewModel()
        {
            Shell.Current.FlyoutIsPresented = false;
            Καθαρισμός = new Command(ClearAllDataPressed);
        }

        private void ClearAllDataPressed(object obj)
        {
            throw new NotImplementedException();
        }

        public bool AllLoading
        {
            get { return allLoading; }
            set
            {
                SetProperty(ref allLoading, value);
            }
        }
    }
}
