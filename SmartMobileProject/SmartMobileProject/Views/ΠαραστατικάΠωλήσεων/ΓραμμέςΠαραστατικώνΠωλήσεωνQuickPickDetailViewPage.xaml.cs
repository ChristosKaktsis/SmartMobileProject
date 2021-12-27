using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.DataForm;
using DevExpress.XamarinForms.DataGrid;
using DevExpress.XamarinForms.Editors;
using SmartMobileProject.Models;
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
    public partial class ΓραμμέςΠαραστατικώνΠωλήσεωνQuickPickDetailViewPage : ContentPage
    {
        public ΓραμμέςΠαραστατικώνΠωλήσεωνQuickPickDetailViewPage()
        {
            InitializeComponent();
            this.SearchType.ItemsSource = new List<string> { "Κωδικός", "Περιγραφή" };
            this.SearchType.SelectedIndex = 1;
            this.Search.TextChanged += Search_TextChanged;
        }
        private void Search_TextChanged(object sender, EventArgs e)
        {
           
            if (string.IsNullOrEmpty(this.Search.Text))
            {
                if (this.ComboBoxOikogeneia.SelectedItem == null)
                {
                    grid.FilterString = string.Empty;
                }
                else
                {
                    var p = "Contains(Οικογένεια.Περιγραφή , '" + ((ΟικογένειαΕίδους)(this.ComboBoxOikogeneia).SelectedValue).Περιγραφή + "')";
                    this.grid.FilterString =  p;
                }
                return;
            }
            if (SearchType.SelectedIndex == 1)
            {
                if (this.ComboBoxOikogeneia.SelectedItem == null)
                {
                    this.grid.FilterString = "Contains(Περιγραφή, '" + this.Search.Text + "')";
                }
                else
                {
                    var p = "Contains(Οικογένεια.Περιγραφή , '" + ((ΟικογένειαΕίδους)(this.ComboBoxOikogeneia).SelectedValue).Περιγραφή + "')";
                    this.grid.FilterString = "Contains(Περιγραφή, '" + this.Search.Text + "') and "+p;
                }
            }

            if (SearchType.SelectedIndex == 0)
            {
                if (this.ComboBoxOikogeneia.SelectedItem == null)
                {
                    this.grid.FilterString = "Contains(Κωδικός, '" + this.Search.Text + "')";
                }
                else
                {
                    var p = "Contains(Οικογένεια.Περιγραφή , '" + ((ΟικογένειαΕίδους)(this.ComboBoxOikogeneia).SelectedValue).Περιγραφή + "')";
                    this.grid.FilterString = "Contains(Κωδικός, '" + this.Search.Text + "') and " + p;
                }
            }
        }

        private void ComboBoxEdit_SelectionChanged(object sender, EventArgs e)
        {
            grid.FilterString = string.Empty;
            if (((ComboBoxEdit)sender).SelectedValue == null)
            {
                return;
            }
            this.grid.FilterString = "Contains(Οικογένεια.Περιγραφή , '" + ((ΟικογένειαΕίδους)((ComboBoxEdit)sender).SelectedValue).Περιγραφή + "')";
        }

        private void grid_Tap(object sender, DevExpress.XamarinForms.CollectionView.CollectionViewGestureEventArgs e)
        {
            //((Είδος)e.Item).Ποσότητα++;
           
            
          //  Console.WriteLine();
            
        }

        private void grid_SelectionChanged(object sender, DevExpress.XamarinForms.CollectionView.CollectionViewSelectionChangedEventArgs e)
        {
            if(e.DeselectedItems.Count == 1)
            {
                ((Είδος)e.DeselectedItems[0]).Ποσότητα = 0;
                
            }
            if (e.SelectedItems.Count == 1)
            {
                ((Είδος)e.SelectedItems[0]).Ποσότητα = (float)this.posotita.Value;
              
            }
            //  Console.WriteLine();
        }
    }
}