using SmartMobileProject.Models;
using SmartMobileProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views.Settings
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ΤρόποςΑποστολήςDetailViewPage : ContentPage
	{
		public ΤρόποςΑποστολήςDetailViewPage (object Τρόποςαποστολής)
		{
			InitializeComponent ();
			if (!(BindingContext is ΤρόποςΑποστολήςDetailViewModel model))
				return;
			if (Τρόποςαποστολής != null)
			{
				model.ΤρόποςΑποστολής = (ΤρόποςΑποστολής)Τρόποςαποστολής;
			}
		}
	}
}