using AppAptO.Models.AppHelpers;
using AppAptO.Models.FBData.Apoios;
using AppAptO.PopUps;
using AppAptO.ViewModels.Pedidos;
using Firebase.Database;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppAptO.Views.Erro;

namespace AppAptO.Views.Pedidos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrudPedidoPage : ContentPage
    {
        private readonly CrudPedidoViewModel viewModel;
        public CrudPedidoPage()
        {
            if (AppConnection.IsConnected)
            {

                InitializeComponent();
                viewModel = new CrudPedidoViewModel();
                BindingContext = viewModel;
            }
            else
            {
                OnNoInternet();
            }
        }
        private async void OnNoInternet() {
            await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "A operação foi cancelada devido à falta de conexão na internet", "Ok"));
            await Navigation.PushAsync(new SemInternetPage());
        }
        public CrudPedidoPage(FirebaseObject<PedidoApoio> fbPedido)
        {

            if (AppConnection.IsConnected)
            {
                InitializeComponent();
                viewModel = new CrudPedidoViewModel(fbPedido.Object, fbPedido.Key);
                BindingContext = viewModel;
            }
            else
            {
                OnNoInternet();
            }
        }
        public CrudPedidoPage(PedidoApoio pedido, string pedidoKey)
        {

            if (AppConnection.IsConnected)
            {
                InitializeComponent();
                viewModel = new CrudPedidoViewModel(pedido, pedidoKey);
                BindingContext = viewModel;
            }
            else
            {
                OnNoInternet();
            }
        }
    }
}