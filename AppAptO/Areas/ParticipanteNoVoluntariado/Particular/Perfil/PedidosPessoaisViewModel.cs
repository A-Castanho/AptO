using AppAptO.Areas.Admin.Apoios.Pedidos;
using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using AppAptO.Views.Pedidos;
using Firebase.Database;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Particular.Perfil
{
    public class PedidosPessoaisViewModel : ObservableObject
    {
        private FirebaseObject<PedidoApoio> pedidoSelecionado;
        public FirebaseObject<PedidoApoio> PedidoSelecionado
        {
            get => pedidoSelecionado;
            set
            {
                if (pedidoSelecionado != value)
                {
                    SetProperty(ref pedidoSelecionado, value);
                }
            }
        }

        public ICommand PedidoSelecionadoCommand => new AsyncCommand(
        async () =>
        {
            if (AuthHelper.UtilizadorAtual.Key != "Admin")
                await Shell.Current.Navigation.PushAsync(new PedidoApoioPage(PedidoSelecionado.Object, PedidoSelecionado.Key));
            else
                await Shell.Current.Navigation.PushAsync(new AdminPedidoApoioPage(PedidoSelecionado));
        });

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string keyUtilizador;
        private ObservableCollection<FirebaseObject<PedidoApoio>> lista;
        public ObservableCollection<FirebaseObject<PedidoApoio>> Lista
        {
            get => lista;
            set => SetProperty(ref lista, value);
        }
        public PedidosPessoaisViewModel()
        {
        }
        public PedidosPessoaisViewModel(Utilizador utilizador, string key)
        {
            DefinirLista(utilizador.Uid);
            Title = "Pedidos por: " + utilizador.NomeExibicao;
            keyUtilizador = key;
        }

        private async void DefinirLista(string uidUtilizador)
        {
            IEnumerable<FirebaseObject<PedidoApoio>> ofertas = (await FBDataBase.PedidosDS.GetAllPostVisAsync()).Where(pedido => pedido.Object.UidApoiado == uidUtilizador);
            Lista = new ObservableCollection<FirebaseObject<PedidoApoio>>(ofertas);
        }

    }
}
