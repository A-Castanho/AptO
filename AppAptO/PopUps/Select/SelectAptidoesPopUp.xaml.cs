using AppAptO.Models.FBData.Utilizadores;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.PopUps.Select
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectAptidoesPopUp : Popup
    {
        SelectAptidoesViewModel viewModel;
        public SelectAptidoesPopUp(IEnumerable<FirebaseObject<Utilizador>> utilizadores)
        {
            InitializeComponent();
            MinhasAreasCollectionView.IsVisible = false;
            viewModel = new SelectAptidoesViewModel(utilizadores);
            viewModel.TitulosAreasEscolhidas.CollectionChanged += (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) =>
            {
                if (viewModel.TitulosAreasEscolhidas.Count > 0)
                    MinhasAreasCollectionView.IsVisible = true;
                else if (MinhasAreasCollectionView.IsVisible)
                    MinhasAreasCollectionView.IsVisible = false;

            };
            BindingContext = viewModel;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            viewModel.DevolverAptidoes();
            Dismiss("");
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var cv = (CollectionView)sender;
                var items = cv.SelectedItems.Select(obj => (string)obj);

                viewModel.TitulosAreasInterioresSelecionadas = new ObservableCollection<string>(items);
            }
            catch { }
        }
    }
}