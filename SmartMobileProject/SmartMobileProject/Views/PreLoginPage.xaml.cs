﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PreLoginPage : ContentPage
	{
        public PreLoginPage()
		{
			InitializeComponent ();
		}
		protected override bool OnBackButtonPressed()
		{
			return true;
		}
	}
}