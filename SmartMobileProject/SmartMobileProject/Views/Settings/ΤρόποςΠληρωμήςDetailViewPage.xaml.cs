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
	public partial class ΤρόποςΠληρωμήςDetailViewPage : ContentPage
	{
		public ΤρόποςΠληρωμήςDetailViewPage (object Τρόποςπληρωμής)
		{
			InitializeComponent ();
			if (!(BindingContext is ΤρόποςΠληρωμήςDetailViewModel model))
				return;
			if (Τρόποςπληρωμής != null)
			{
				model.ΤρόποςΠληρωμής = (ΤρόποςΠληρωμής)Τρόποςπληρωμής;
			}
		}
	}
}