using AppAptO.Models.FBConnections;
using AppAptO.Views.Conta;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppAptO.Areas.Geral.Configuracoes
{
    public class MainConfigViewModel
    {
        private readonly ContentPage AccConfigPage;
        public MainConfigViewModel()
        {
            if (AuthHelper.UtilizadorAtual.Object.IsOrganizacao)
                AccConfigPage = new ConfiguracoesOrganizacaoPage();
            else
                AccConfigPage = new ConfiguracoesParticularPage();
        }

        public ICommand CommandGoToContactos => new Command(
        async () => await Shell.Current.Navigation.PushAsync(new AppContactosPage()));
        public ICommand CommandGoToSobre => new Command(
        async () => await Shell.Current.Navigation.PushAsync(new AppSobrePage()));
        public ICommand CommandGoToContaDef => new Command(
        async () => await Shell.Current.Navigation.PushAsync(AccConfigPage));


    }
}
