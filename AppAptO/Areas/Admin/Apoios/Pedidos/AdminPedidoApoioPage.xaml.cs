using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Views.Conta;
using Firebase.Database;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.Admin.Apoios.Pedidos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPedidoApoioPage : ContentPage
    {
        private readonly AdminPedidoApoioViewModel viewModel;
        public AdminPedidoApoioPage(FirebaseObject<PedidoApoio> pedidoApoio)
        {
            InitializeComponent();
            viewModel = new AdminPedidoApoioViewModel(pedidoApoio.Object, pedidoApoio.Key, ListUtilizadores.RowHeight);
            BindingContext = viewModel;
        }

        public AdminPedidoApoioPage(PedidoApoio pedidoApoio, string pedidoKey)
        {
            InitializeComponent();
            viewModel = new AdminPedidoApoioViewModel(pedidoApoio, pedidoKey, ListUtilizadores.RowHeight);
            BindingContext = viewModel;
        }

        private async void UtilizadorTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var utilizador = (FirebaseObject<Utilizador>)((StackLayout)sender).BindingContext;
            if (utilizador != null)
                await Navigation.PushAsync(new PerfilPage(utilizador.Object, utilizador.Key));
        }

    }
}