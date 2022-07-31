using AppAptO.ViewModels.Ofertas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Ofertas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OfertasMainPage : ContentPage
    {
        private readonly OfertasMainViewModel viewModel = new OfertasMainViewModel();
        public OfertasMainPage()
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.ComandoMostrarDetalhes.Execute((OfertasMainViewModel.ElementoLista)e.Item);
            // viewModel.ComandoAnalisarElemento.Execute(sender);
        }
    }
}