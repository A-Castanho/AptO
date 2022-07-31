using AppAptO.Models.FBData.Apoios;
using AppAptO.ViewModels.Pedidos;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Pedidos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PedidoApoioPage : ContentPage
    {
        private readonly PedidoApoioViewModel viewModel;
        public PedidoApoioPage(PedidoApoio pedido, string pedidokey)
        {
            InitializeComponent();
            viewModel = new PedidoApoioViewModel(pedido, pedidokey);
            BindingContext = viewModel;
        }

    }
}