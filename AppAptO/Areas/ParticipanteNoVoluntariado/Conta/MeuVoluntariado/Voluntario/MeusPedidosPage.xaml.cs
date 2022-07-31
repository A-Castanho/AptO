using AppAptO.Models.FBData.Utilizadores;
using AppAptO.ViewModels.Conta.MeuVoluntariado.MeusPedido;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Conta.MeuVoluntariado.MeuPedido
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeusPedidosPage : ContentPage
    {
        private readonly MeusPedidosViewModel viewModel;
        public MeusPedidosPage()
        {
            InitializeComponent();
            viewModel = new MeusPedidosViewModel();
            BindingContext = viewModel;
            Lista.IsRefreshing = false;
        }
        public MeusPedidosPage(UtilizadorOrganizacao organizacaoPertencente)
        {
            InitializeComponent();
            viewModel = new MeusPedidosViewModel(organizacaoPertencente);
            BindingContext = viewModel;
            Lista.IsRefreshing = false;
        }
        protected override void OnAppearing()
        {
            //viewModel.CommandPesquisar.Execute("");
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.CommandGoToAcao.Execute(sender);
        }
    }
}