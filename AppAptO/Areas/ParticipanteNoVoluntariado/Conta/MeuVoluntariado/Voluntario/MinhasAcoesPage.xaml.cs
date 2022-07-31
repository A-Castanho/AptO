using AppAptO.ViewModels.Conta;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Conta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MinhasAcoesPage : ContentPage
    {
        private readonly MinhasAcoesViewModel viewModel;
        public MinhasAcoesPage()
        {
            InitializeComponent();
            viewModel = new MinhasAcoesViewModel();
            BindingContext = viewModel;
            viewModel.PesquisarLista();
            Lista.IsRefreshing = false;
        }
        public MinhasAcoesPage(string keyOrganizacao)
        {
            InitializeComponent();
            viewModel = new MinhasAcoesViewModel(keyOrganizacao);
            BindingContext = viewModel;
            viewModel.PesquisarLista(keyOrganizacao);
            Lista.IsRefreshing = false;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.CommandGoToAcao.Execute(sender);
        }
    }
}