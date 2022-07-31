using AppAptO.Models.FBData.Utilizadores;
using AppAptO.ViewModels.Conta;
using Firebase.Database;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.PartialViews.Conta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OfertasPessoaisView : ContentView
    {
        private readonly OfertasPessoaisViewModel viewModel;
        public OfertasPessoaisView()
        {
            InitializeComponent();
        }
        public OfertasPessoaisView(FirebaseObject<Utilizador> utilizador)
        {
            InitializeComponent();
            viewModel = new OfertasPessoaisViewModel(utilizador.Object, utilizador.Key);
            BindingContext = viewModel;
        }
        public OfertasPessoaisView(Utilizador utilizador, string key)
        {
            InitializeComponent();
            viewModel = new OfertasPessoaisViewModel(utilizador, key);
            BindingContext = viewModel;
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.OfertaSelecionadaCommand.Execute(sender);
        }
    }
}