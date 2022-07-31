using AppAptO.ViewModels.Pedidos;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Pedidos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PedidosMainPage : ContentPage
    {
        private readonly PedidosMainViewModel viewModel = new PedidosMainViewModel();
        public PedidosMainPage()
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.ComandoMostrarDetalhes.Execute((PedidosMainViewModel.ElementoLista)e.Item);
        }
    }
}