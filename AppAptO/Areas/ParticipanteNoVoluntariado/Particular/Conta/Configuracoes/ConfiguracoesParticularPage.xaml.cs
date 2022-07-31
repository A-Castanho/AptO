using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using AppAptO.ViewModels.Conta;
using Firebase.Database;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Conta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfiguracoesParticularPage : ContentPage
    {
        ConfiguracoesParticularViewModel viewModel;
        public ConfiguracoesParticularPage()
        {
            InitializeComponent();
            IniciarProprio();
        }

        private async void IniciarProprio()
        {
            var utilizadorParticular = await FBDataBase.UtilizadorDS.GetByUidAsync<UtilizadorParticular>(AuthHelper.UtilizadorAtual.Object.Uid);
            viewModel = new ConfiguracoesParticularViewModel(utilizadorParticular);
            BindingContext = viewModel;
        }

        public ConfiguracoesParticularPage(FirebaseObject<UtilizadorParticular> fbUtilizador)
        {
            InitializeComponent();
            viewModel = new ConfiguracoesParticularViewModel(fbUtilizador);
            BindingContext = viewModel;
        }
    }
}