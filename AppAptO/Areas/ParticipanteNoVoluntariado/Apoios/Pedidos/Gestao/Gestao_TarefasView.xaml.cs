
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.ViewModels.Conta.GestaoVoluntariado;
using AppAptO.Views.Conta;
using Firebase.Database;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Apoios.Pedidos.Gestao
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Gestao_TarefasView : ContentView
    {
        private GestaoTarefasViewModel viewModel;
        public Gestao_TarefasView(PedidoApoio pedido, string pedidoKey, bool isModoEdicao = true)
        {
            InitializeComponent();
            viewModel = new GestaoTarefasViewModel(pedido, pedidoKey, isModoEdicao);
            BindingContext = viewModel;
        }
        private void ListTarefas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.VisibilidadeListaTarefas = !viewModel.VisibilidadeListaTarefas;
        }
        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            viewModel.CommandMudarEstado.Execute(sender);
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //viewModel.CommandGoToUtilizador.Execute(sender);
            var utilizador = (FirebaseObject<Utilizador>)((Frame)sender).BindingContext;
            if (utilizador != null)
                await Shell.Current.Navigation.PushAsync(new PerfilPage(utilizador.Object, utilizador.Key));
        }
    }
}