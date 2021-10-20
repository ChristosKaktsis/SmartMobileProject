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
	public partial class ΛογαριασμοίΧρημΔιαθDetailViewPage : ContentPage
	{
		public ΛογαριασμοίΧρημΔιαθDetailViewPage (object account)
		{	
			InitializeComponent();
			if (!(BindingContext is ΛογαριασμοίΧρημΔιαθDetailViewModel model))
				return;
			if (account != null)
			{
				model.Λογαρισαμοί = (ΛογαριασμοίΧρηματικώνΔιαθέσιμων)account;
			}
		}
	}
}