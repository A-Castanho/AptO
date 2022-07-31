using System;
using System.Collections.ObjectModel;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.PopUps.Autenticacao
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EscolherExperienciaPage : ContentPage
    {
        AptidoesViewModel viewModel;
        public EscolherExperienciaPage()
        {
            InitializeComponent();
            MinhasAreasCollectionView.IsVisible = false;
            viewModel = new AptidoesViewModel();
            viewModel.TitulosAreasEscolhidas.CollectionChanged += (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) =>
            {
                if (viewModel.TitulosAreasEscolhidas.Count > 0)
                    MinhasAreasCollectionView.IsVisible = true;
                else if (MinhasAreasCollectionView.IsVisible)
                    MinhasAreasCollectionView.IsVisible = false;

            };
            BindingContext = viewModel;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.CommandMudarVisibilidade.Execute(null);
        }
        private void ListInteriores_SelectionChanged(object sender, SelectionChangedEventArgs eventArgs)
        {
            try
            {
                var cv = (CollectionView)sender;
                var items = cv.SelectedItems.Select(obj => (string)obj);

                //viewModel.TitulosAreasInterioresSelecionadas = new ObservableCollection<string>(items);
                viewModel.TitulosAreasInterioresSelecionadas = new ObservableCollection<string>(items);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n" +
                    "Error data: \n" + e.Data +
                    "\nMessage: \n" + e.Message +
                    "\nStackTrace: \n" + e.StackTrace +
                    "\nSource: \n" + e.Source +
                    "\nInnerException: \n" + e.InnerException +
                    "\nTargetSite: \n" + e.TargetSite
                    );
            }
        }
    }
}