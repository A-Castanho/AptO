using AppAptO.Models.FBData.Utilizadores;
using Firebase.Database;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Conta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfiguracoesOrganizacaoPage : ContentPage
    {
        ConfiguracoesOrganizacaoViewModel viewModel;
        public ConfiguracoesOrganizacaoPage(FirebaseObject<UtilizadorOrganizacao> fbUtilizador)
        {
            InitializeComponent();
            viewModel = new ConfiguracoesOrganizacaoViewModel(fbUtilizador);
            BindingContext = viewModel;
        }
        public ConfiguracoesOrganizacaoPage(UtilizadorOrganizacao utilizador, string key)
        {
            InitializeComponent();
            viewModel = new ConfiguracoesOrganizacaoViewModel(utilizador, key);
            BindingContext = viewModel;
        }
        public ConfiguracoesOrganizacaoPage()
        {
            InitializeComponent();
            viewModel = new ConfiguracoesOrganizacaoViewModel();
            BindingContext = viewModel;
        }
    }
}