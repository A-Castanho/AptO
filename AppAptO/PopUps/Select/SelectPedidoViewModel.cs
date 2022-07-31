using AppAptO.Models.FBData.Apoios;
using AppAptO.Views.Pedidos;
using Firebase.Database;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.PopUps.Select
{
    public class SelectPedidoViewModel : ObservableObject
    {
        public FirebaseObject<PedidoApoio> PedidoSelecionado
        {
            get => pedidoSelecionado; set
            {
                SetProperty(ref pedidoSelecionado, value);
            }
        }
        private ObservableCollection<FirebaseObject<PedidoApoio>> listaPedidos = new ObservableCollection<FirebaseObject<PedidoApoio>>();
        private FirebaseObject<PedidoApoio> pedidoSelecionado;
        public ObservableCollection<FirebaseObject<PedidoApoio>> Lista
        {
            get { return listaPedidos; }
            set { SetProperty(ref listaPedidos, value); }
        }
        public ICommand CommandGoToPedido => new Command(
        async () =>
        {
            if (PedidoSelecionado != null)
                await Shell.Current.Navigation.PushAsync(new PedidoApoioPage(PedidoSelecionado.Object, PedidoSelecionado.Key));
        });
        public SelectPedidoViewModel() { }
        public SelectPedidoViewModel(IEnumerable<FirebaseObject<PedidoApoio>> listaUtilizadores)
        {
            IniciarLista(listaUtilizadores);
        }
        public void IniciarLista(IEnumerable<FirebaseObject<PedidoApoio>> listaUtilizadores)
        {
            Lista = new ObservableCollection<FirebaseObject<PedidoApoio>>(listaUtilizadores);
            if (Lista.Count > 0)
                PedidoSelecionado = Lista[0];
        }
    }
}
