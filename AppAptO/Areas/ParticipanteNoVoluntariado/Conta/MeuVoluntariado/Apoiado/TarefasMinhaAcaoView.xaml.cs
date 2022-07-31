using AppAptO.Models.FBData.Apoios;
using AppAptO.ViewModels.Conta.MeuVoluntariado;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Conta.MeuVoluntariado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TarefasMinhaAcaoView : ContentView
    {
        TarefasMinhaAcaoViewModel viewModel;
        public TarefasMinhaAcaoView()
        {
            InitializeComponent();
        }
        public TarefasMinhaAcaoView(PedidoApoio acao, string keyAcao, string KeyOrganizacao = "")
        {
            InitializeComponent();
            viewModel = new TarefasMinhaAcaoViewModel(acao, keyAcao, KeyOrganizacao);
            BindingContext = viewModel;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.CommandGoToTarefa.Execute(sender);
        }
    }
}