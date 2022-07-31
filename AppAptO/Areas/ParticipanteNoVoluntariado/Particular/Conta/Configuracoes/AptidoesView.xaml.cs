using AppAptO.PopUps.Autenticacao;
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Particular.Conta.Configuracoes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AptidoesView : ContentView
    {

        AptidoesViewModel viewModel;
        public AptidoesView()
        {
            InitializeComponent();
            MinhasAptidoes.IsVisible = false;
            viewModel = new AptidoesViewModel();
            viewModel.TitulosAreasEscolhidas.CollectionChanged += (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) =>
            {
                if (viewModel.TitulosAreasEscolhidas.Count > 0)
                    MinhasAptidoes.IsVisible = true;
                else if (MinhasAptidoes.IsVisible)
                    MinhasAptidoes.IsVisible = false;

            };
            BindingContext = viewModel;
        }

        private void ListInteriores_SelectionChanged(object sender, SelectionChangedEventArgs eventArgs)
        {
            try
            {
                var cv = (CollectionView)sender;
                var items = cv.SelectedItems.Select(obj => (string)obj);

                viewModel.TitulosAreasInterioresSelecionadas = new ObservableCollection<string>(items);
            }
            catch
            {
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var a = (AptidoesViewModel.ElementoFbArea)((Frame)sender).BindingContext;
            viewModel.AreaGeralSelecionada = a;
        }
    }
}