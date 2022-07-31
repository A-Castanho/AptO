using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using AppAptO.Views.Conta;
using AppAptO.Views.Conta.MeuVoluntariado.MeuPedido;
using Firebase.Database;
using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Flyout
{
    public class AppShellViewModel : ObservableObject
    {
        private bool visibilidadeOrgArea;
        public bool VisibilidadeOrgArea
        {
            get => visibilidadeOrgArea;
            set
            {
                SetProperty(ref visibilidadeOrgArea, value);
            }
        }
        public UtilizadorOrganizacao OrganizacaoPertencente { get; set; }
        public string KeyOrganizacaoPertencente { get; set; }
        private FirebaseObject<Utilizador> utilizador;
        public FirebaseObject<Utilizador> Utilizador
        {
            get => utilizador;
            set
            {
                VisibilidadeNotificacoes = value.Object.Avisos.Count > 0 || value.Object.Convites.Count > 0;
                SetProperty(ref utilizador, value);
            }
        }
        private bool visibilidadeNotificacoes;

        public bool VisibilidadeNotificacoes
        {
            get => visibilidadeNotificacoes;
            set
            {
                SetProperty(ref visibilidadeNotificacoes, value);
            }
        }
        public ICommand CommandLogout => new Command(
          async () =>
            {
                AuthHelper.SairSessao();
                await Shell.Current.Navigation.PopToRootAsync();
                Application.Current.MainPage = new AutenticacaoPage();
            });
        public ICommand CommandGoToPerfil => new Command(
        async () =>
        {
            if (Utilizador.Key != "Admin")
            {
                await Shell.Current.Navigation.PushAsync(new PerfilPage(AuthHelper.UtilizadorAtual));
                Shell.Current.FlyoutIsPresented = false;
            }
        });
        public ICommand CommandGoToOrgArea => new Command(
        async () =>
        {
            if (Utilizador.Object.IsApoiado)
                await Shell.Current.Navigation.PushAsync(new MeusPedidosPage(OrganizacaoPertencente));
            else
                await Shell.Current.Navigation.PushAsync(new MinhasAcoesPage(KeyOrganizacaoPertencente));
            Shell.Current.FlyoutIsPresented = false;
        });
        public ICommand CommandGoToNotificacoes => new Command(
        async () =>
        {
            await Shell.Current.Navigation.PushAsync(new ListaNotificacoesPage());
            Shell.Current.FlyoutIsPresented = false;
        });

        public AppShellViewModel()
        {
            Utilizador = AuthHelper.UtilizadorAtual;
            DefinirOrgArea();
            var disposable = FBDataBase.UtilizadorDS.DatabasePath
                .AsObservable<Utilizador>()
                .Subscribe(async (dbevent) =>
                {
                    if (dbevent.Object != null)
                    {
                        if (dbevent.Key == AuthHelper.UtilizadorAtual.Key)
                            Utilizador = await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(Utilizador.Object.Uid);
                    }
                });
        }
        private async void DefinirOrgArea()
        {
            try
            {

                if (!Utilizador.Object.IsOrganizacao && Utilizador.Key != "Admin")
                {
                    var particular = await FBDataBase.UtilizadorDS.GetByUidAsync<UtilizadorParticular>(Utilizador.Object.Uid);
                    if (!string.IsNullOrEmpty(particular.Object.UidGrupoPertencente))
                    {
                        VisibilidadeOrgArea = true;
                        var org = await FBDataBase.UtilizadorDS.GetByUidAsync<UtilizadorOrganizacao>(particular.Object.UidGrupoPertencente);
                        KeyOrganizacaoPertencente = org.Key;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n" +
                                "Error data: \n" + e.Data +
                                "\nMessage: \n" + e.Message +
                                "\nStackTrace: \n" + e.StackTrace +
                                "\nSource: \n" + e.Source +
                                "\nInnerException: \n" + e.InnerException +
                                "\nTargetSite: \n" + e.TargetSite
                                );
            }
        }
    }
}