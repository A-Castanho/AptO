using AppAptO.Models.FBData.Utilizadores;
using AppAptO.ViewModels.Conta.Configuracoes;
using Firebase.Database;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.ViewsPartial.Conta.Configuracoes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarOrganizacaoView : ContentView
    {
        EditarOrganizacaoViewModel viewModel;
        public EditarOrganizacaoView(UtilizadorOrganizacao utilizador, string utilizadorKey)
        {
            InitializeComponent();
            viewModel = new EditarOrganizacaoViewModel(utilizador, utilizadorKey);
            BindingContext = viewModel;
        }
        public EditarOrganizacaoView(FirebaseObject<UtilizadorOrganizacao> utilizador)
        {
            InitializeComponent();
            viewModel = new EditarOrganizacaoViewModel(utilizador);
            this.BindingContext = viewModel;
        }
    }
}