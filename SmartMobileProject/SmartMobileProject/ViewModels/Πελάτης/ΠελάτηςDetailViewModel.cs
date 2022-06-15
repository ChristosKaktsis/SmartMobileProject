using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;
using SmartMobileProject.Views;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.IO;

namespace SmartMobileProject.ViewModels
{
    [QueryProperty(nameof(IsFromEdit), nameof(IsFromEdit))]
    class ΠελάτηςDetailViewModel : BaseViewModel
    {
        App app = (App)Application.Current; //
   
        private Πελάτης icustomer;
       
       //flags   
        public bool saveButtonPressed = false;
        public bool isFromEdit = false;
        public bool stopNavigating = true;
        public bool afmIsFocused;
        public bool eponimiaIsFocused;
        public static bool hasError = false;
        //
        //Error Messages
        string errorMessage;
        string afmErrorMessage;
        string eponimiaErrorMessage;
        string dieuthinsiErrorMessage;
        private ImageSource imageSource;
        private string _imageInBytes;
        private Πρότυπα πρότυπο;

        //

        public Πελάτης customer
        {
            get
            {
                return icustomer;
            }
            set
            {
                //icustomer = value;
                SetProperty(ref icustomer, value);

                if (value == null)
                    return;
                ImageSource = ShowImage(value.ImageBytes);
                Πρότυπο = app.uow.Query<Πρότυπα>().Where(x => x.Περιγραφή == value.Σημείωση2).FirstOrDefault(); 
            }
        }

        public Πρότυπα Πρότυπο
        {
            get
            {
                return πρότυπο;
            }
            set
            {
                //icustomer = value;
                SetProperty(ref πρότυπο, value);

                if (value == null)
                    return;
                customer.Σημείωση2 = value.Περιγραφή;
            }
        }

        private ImageSource ShowImage(string imageBytes)
        {
            if (string.IsNullOrWhiteSpace(imageBytes))
                return null;
            var bytearray = Convert.FromBase64String(imageBytes);
            Stream stream = new MemoryStream(bytearray);
            var imageSource = ImageSource.FromStream(() => stream);
            return imageSource;
        }

        public XPCollection<ΔΟΥ> DOYCollection { get; set; }
        public XPCollection<Πρότυπα> ΠρότυπαCollection { get; set; }
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                SetProperty(ref errorMessage, value);
     

            }
        }
        public string AfmErrorMessage
        {
            get
            {
                return afmErrorMessage;
            }
            set
            {            
                SetProperty(ref afmErrorMessage, value);
                OnErrorMessageChanged(value);
                            
            }
        }
        public string EponimiaErrorMessage
        {
            get
            {
                return eponimiaErrorMessage;
            }
            set
            {
                SetProperty(ref eponimiaErrorMessage, value);
                OnErrorMessageChanged(value);
            }
        }
        public string DieuthinsiErrorMessage
        {
            get
            {
                return dieuthinsiErrorMessage;
            }
            set
            {
                SetProperty(ref dieuthinsiErrorMessage, value);
                OnErrorMessageChanged(value);
            }
        }
        public bool IsFromEdit
        {
            get
            {
                return isFromEdit;
            }
            set
            {
                isFromEdit = value;
                SetProperty(ref isFromEdit, value);
                SetTitle();
            }
        }
        public bool AfmIsFocused
        {
            get { return afmIsFocused; }
            set
            {            
                SetProperty(ref afmIsFocused, value);
                OnAfmFocusedChanged(value);
            }
        }
        public bool EponimiaIsFocused
        {
            get { return eponimiaIsFocused; }
            set
            {
                SetProperty(ref eponimiaIsFocused, value);
                OnEponimiaFocusedChanged(value);
            }
        }
        public bool HasError
        {
            get { return hasError; }
            set
            {             
                SetProperty(ref hasError, value);
            }
        }

        public ΠελάτηςDetailViewModel()
        {
           
            //
            //Πάρε τον πελάτη απο το App και βαλ τον στο icustomer
            //
            AppShell appShell = (AppShell)Application.Current.MainPage;
            customer = appShell.customer1;
                       
            if (customer == null)
            {
                App app1 = (App)Application.Current;      
                appShell.customer1 = new Πελάτης(app.uow);
                appShell.customer1.SmartOid = Guid.NewGuid();
                appShell.customer1.ΗμνίαΔημ = DateTime.Now;
                customer = appShell.customer1;
                appShell.πωλητής.Πελάτες.Add(customer);
            }            
            //set the DOY Collection
            DOYCollection = new XPCollection<ΔΟΥ>(app.uow);
            ΠρότυπαCollection = new XPCollection<Πρότυπα>(app.uow);

            //commands
            Δημιουργία = new Command(Create);
            Αποθήκευση = new Command(Save);
            Διευθύνσεις = new Command(OpenAddress);
            Φωτογραφία = new Command(TakeImage);
            Πίσω = new Command(GoBack);
        }

        private  void SetTitle()
        {
            if (IsFromEdit)
            {
                Title = "Επεξεργασια";
            }
            else
            {
                Title = "Νεος Πελάτης"; 
            }
        }
        private void Create(object obj)
        {
            //
            //Πάρε τον πελάτη απο το App και βαλ τον στο icustomer
            //
            AppShell appShell = (AppShell)Application.Current.MainPage;
            this.icustomer = appShell.customer1;

            if (customer == null)
            {             
                customer = new Πελάτης(app.uow);
            }

        }
        private async void Save(object obj)
        {

            if (ChechError())
            {
                await Application.Current.MainPage.DisplayAlert("Alert", ErrorMessage, "OK");
                return;
            }
            customer.ΚεντρικήΔιευθυνση = customer.ΔιευθύνσειςΠελάτη.FirstOrDefault();

            if (app.uow.InTransaction)
            {
                try
                {
                    if (CheckPelatis())
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "Υπάρχει ίδη πελάτης με ΑΦΜ :" + customer.ΑΦΜ, "OK");
                        return;
                    }
                    customer.CanUpload = true;
                    app.uow.CommitChanges();
                }
                catch(Exception e)
                {
                    //Console.WriteLine(e);
                    return;
                }
            }

            //tell the app that the save button is pressed
            saveButtonPressed = true;
            
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
            //ask if new Order
            var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", "Θέλετε να δημιουργήσετε νέο παραστατικό πωλήσεων ; ", "Ναί", "Όχι");
            if (answer)
            {
                if (!IsTrialOn)
                    return;
                //setthe static class for new order
                ΝέοΠαραστατικόViewModel.Order = null;

                ΝέοΠαραστατικόViewModel.uow = new UnitOfWork();
                //set politi
                AppShell app = (AppShell)Application.Current.MainPage;
                var p = ΝέοΠαραστατικόViewModel.uow.Query<Πωλητής>().Where(x => x.Oid == app.πωλητής.Oid);
                ΝέοΠαραστατικόViewModel.politis = p.FirstOrDefault();
                ΝέοΠαραστατικόViewModel.πελατης = customer;
                await Shell.Current.GoToAsync(nameof(ΠαραστατικόΒασικάΣτοιχείαPage));
            }
           

        }
        private async void OpenAddress(object obj)
        {
            await Shell.Current.GoToAsync(nameof(ΔιευθύνσειςΠελάτηListViewPage));
        }
        void OnAfmFocusedChanged(bool hasFocus)
        {
            if (!hasFocus)
            {
               
                if (string.IsNullOrEmpty(customer.ΑΦΜ))
                {
                    AfmErrorMessage = "Shoud Not Be Empty";
                    HasError = true;
                }
                else
                {
                    AfmErrorMessage = string.Empty;
                    HasError = false;
                }
            }
        }
        void OnEponimiaFocusedChanged(bool hasFocus)
        {
            if (!hasFocus)
            {
                if (string.IsNullOrEmpty(customer.Επωνυμία))
                {
                    EponimiaErrorMessage = "Shoud Not Be Empty";
                    HasError = true;
                }
                else
                {
                    EponimiaErrorMessage = string.Empty;
                    HasError = false;
                }
            }
        }
        //
        //error Checks
        //
        bool ChechError()
        {
            CheckAfm();
            CheckEponimia();
            CheckDieuthinsi();
            if (!string.IsNullOrEmpty(AfmErrorMessage) || !string.IsNullOrEmpty(EponimiaErrorMessage) || !string.IsNullOrEmpty(DieuthinsiErrorMessage))
            {
                HasError = true;
                ErrorMessage = AfmErrorMessage +"\n"+ EponimiaErrorMessage+"\n"+ DieuthinsiErrorMessage;
            }
            else
            {
                HasError = false;
            }
            return HasError;
        }
        void CheckAfm()
        {
            if (string.IsNullOrEmpty(customer.ΑΦΜ))
            {
                AfmErrorMessage = "Το ΑΦΜ Δεν Πρέπει να είναι κενό";
            }
            else
            {
                AfmErrorMessage = string.Empty;
            }
        }
        void CheckEponimia()
        {
            if (string.IsNullOrEmpty(customer.Επωνυμία))
            {
                EponimiaErrorMessage = "Η Επωνυμία Δεν Πρέπει να είναι κενή";
            }
            else
            {
                EponimiaErrorMessage = string.Empty;
            }
        }
        void CheckDieuthinsi()
        {
            if (customer.ΔιευθύνσειςΠελάτη.Count == 0)
            {  
                DieuthinsiErrorMessage = "Δεν Υπαρχει καμία Διεύθυνση";           
            }
            else
            {
                DieuthinsiErrorMessage = string.Empty;
            }
        }
        bool CheckPelatis()
        {
           
            var p = app.uow.Query<Πελάτης>().Where(x => x.Πωλητής == customer.Πωλητής 
            && x.ΑΦΜ == customer.ΑΦΜ);  
            
            if (p.Any() && p.FirstOrDefault().Oid!= customer.Oid)
                return true;

            return false;
        }
        void OnErrorMessageChanged(string newMessage)
        {
            if (newMessage == null)
            {
                return;
            }
           
            
        }
        //
        //Take photo
        public ImageSource ImageSource
        {
            get { return imageSource; }
            set => SetProperty(ref imageSource, value);
        }
        public string ImageInBytes
        {
            get => _imageInBytes;
            set
            {
                SetProperty(ref _imageInBytes, value);
                
                customer.ImageBytes = value;
            }
        }
        private async void TakeImage()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                
                await LoadPhotoAsync(photo);//set it to image source in UI
                byte[] imageByte = await ResizeImage(photo); //small size image
                ConvertImage(imageByte); //  to base64 save to customer.ImageInBytes
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
                Console.WriteLine($"CapturePhotoAsync THREW: {fnsEx.Message}");
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
                Console.WriteLine($"CapturePhotoAsync THREW: {pEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }
        public async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                ImageSource = null;
                ImageInBytes = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            
            ImageSource = ImageSource.FromFile(newFile);
            //ConvertImage(newFile);
            customer.ImageName = photo.FileName;//save image name
        }
        void ConvertImage(byte[] imageByte)
        {
            //byte[] imageByte = File.ReadAllBytes(p);
            ImageInBytes = Convert.ToBase64String(imageByte);
        }
        protected async Task<byte[]> ResizeImage(FileResult imageFile)
        {
            byte[] imageData;

            Stream stream = await imageFile.OpenReadAsync();
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                imageData = ms.ToArray();
            }

            var x = 400;
            var y = 400;

            byte[] resizedImage = DependencyService.Get<IResizeImageService>().ResizeImage(imageData, (float)x, (float)y);

            return resizedImage;
        }
        /// <summary>
        /// Γινεται commit στην βαση
        /// </summary>
        public ICommand Αποθήκευση { set; get; }
        public ICommand Δημιουργία { set; get; }
        public ICommand Διευθύνσεις { set; get; }
        public ICommand Πίσω { get; set; }
        public ICommand Φωτογραφία { get; set; }
    }
}
