using AppAptO.Models.FBData.Utilizadores;
using Firebase.Database;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Particular.Perfil
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PedidosPessoaisView : ContentView
    {
        private readonly PedidosPessoaisViewModel viewModel;
        public PedidosPessoaisView()
        {
            InitializeComponent();
        }
        public PedidosPessoaisView(FirebaseObject<Utilizador> utilizador)
        {
            InitializeComponent();
            viewModel = new PedidosPessoaisViewModel(utilizador.Object, utilizador.Key);
            BindingContext = viewModel;
        }
        public PedidosPessoaisView(Utilizador utilizador, string key)
        {
            InitializeComponent();
            viewModel = new PedidosPessoaisViewModel(utilizador, key);
            BindingContext = viewModel;
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.PedidoSelecionadoCommand.Execute(sender);
        }
    }
}