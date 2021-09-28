﻿using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace SmartMobileProject.ViewModels
{
    class ΕργασίεςStaticViewModel
    {
        public static UnitOfWork uow;
        public static Εργασία εργασία;
        public static Ενέργεια ενέργεια;
        public static XPCollection<Πελάτης> Πελάτες { get; set; }
        public static async void GetCurrentLocation(Εργασία Εργασία)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Εργασία.ΓεωγραφικόΠλάτος = location.Latitude;
                    Εργασία.ΓεωγραφικόΜήκος = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
    }
}
