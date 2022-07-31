using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using Firebase.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppAptO.Areas.Admin.Apoios.Pedidos
{
    public class AdminPedidoApoioViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        private string PedidoKey { get; }
        public FirebaseObject<Utilizador> UtilizadorSelecionado { get; set; }
        private ObservableCollection<FirebaseObject<Utilizador>> listaUtilizadores = new ObservableCollection<FirebaseObject<Utilizador>>();

        private int listHeight;
        public int ListHeight
        {
            get { return listHeight; }
            set { SetProperty(ref listHeight, value); }
        }
        public ObservableCollection<FirebaseObject<Utilizador>> ListaUtilizadores
        {
            get { return listaUtilizadores; }
            set
            {
                ListHeight = value.Count * rowHeight;
                SetProperty(ref listaUtilizadores, value);
            }
        }
        private FirebaseObject<Utilizador> apoiado;
        private readonly int rowHeight;

        public PedidoApoio PedidoApoio { get; set; }
        public FirebaseObject<Utilizador> Apoiado
        {
            get => apoiado;
            private set => SetProperty(ref apoiado, value);
        }
        public AdminPedidoApoioViewModel(PedidoApoio pedidoApoio, string pedidoApoioKey, int rowHeight)
        {
            PedidoApoio = pedidoApoio;
            PedidoKey = pedidoApoioKey;
            DefinirUtilizador();
            DefinirUtilizadores();
            this.rowHeight = rowHeight;
        }
        public AdminPedidoApoioViewModel() { }

        private async void DefinirUtilizadores()
        {
            ListaUtilizadores = new ObservableCollection<FirebaseObject<Utilizador>>((await FBDataBase.UtilizadorDS.GetAllAsync<Utilizador>())
                        .Where(utilizador => PedidoApoio.KeysUtilizadoresDisponiveis.Contains(utilizador.Key)));
        }
        private async void DefinirUtilizador()
        {
            Apoiado = (await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(PedidoApoio.UidApoiado));
        }
        public ICommand CommandGoToPerfil => new Command(
            async () => await Xamarin.Forms.Shell.Current.Navigation.PushAsync(new Views.Conta.PerfilPage(Apoiado)));
        public ICommand CommandGoToEditar => new Command(
            async () => await Xamarin.Forms.Shell.Current.Navigation.PushAsync(new Views.Pedidos.CrudPedidoPage(PedidoApoio, PedidoKey)));

    }
}

