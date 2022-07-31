using AppAptO.Areas.Admin.Shell;
using AppAptO.Areas.ParticipanteNoVoluntariado.Flyout;
using AppAptO.Models.AppHelpers;
using AppAptO.Models.FBConnections;
using AppAptO.PopUps;
using AppAptO.Services.Firebase;
using AppAptO.Themes;
using AppAptO.Views.Aplicacao;
using AppAptO.Views.Conta;
using AppAptO.Views.Erro;
using AppAptO.Views.Flyout.Particular;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace AppAptO
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }
        private static async Task IniciarTiposDados()
        {
            if ((await FBDataBase.AreasApoioDS.GetAllAsync()).Count() == 0)
            {
                await FBDataBase.AreasApoioDS.InsertInitialData();
            }
            if ((await FBDataBase.LocalidadeDS.GetAllAsync()).Count() == 0)
            {
                await FBDataBase.LocalidadeDS.InsertInitialData();
            }
            if ((await FBDataBase.AptidoesDS.GetAllAsync()).Count() == 0)
            {
                await FBDataBase.AptidoesDS.InsertInitialData();
            }
        }

        protected override void OnStart()
        {
            StartApp();
        }

        private async void StartApp()
        {
            Application.Current.MainPage = new TituloPage();
            if (AppConnection.IsConnected)
            {
                await StorageHelper.IniciarAsync();
                await IniciarTiposDados();
                await Iniciar();
                await MostrarMensagemInicial();
            }
            else
            {
                MainPage = new SemInternetPage();
            }

        }

        private async Task MostrarMensagemInicial()
        {
            var mensagens = await FBDataBase.MsgSabiasDS.GetAllAsync();
            if (mensagens.Count() > 0)
            {
                var index = new Random().Next(mensagens.Count() - 1);
                var selecionada = mensagens.ToList()[index].Object;
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Sabias que...", selecionada.Texto, "Avançar"));
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        public static async Task Iniciar()
        {
            try
            {

                bool utilizadorRecuperado = await AuthHelper.RecuperarInformacoesIniciadas();
                if (utilizadorRecuperado)
                {
                    try
                    {
                        if (AuthHelper.UtilizadorAtual.Key != "Admin")
                        {
                            if (AuthHelper.UtilizadorAtual.Object.IsOrganizacao)
                            {
                                if (AuthHelper.UtilizadorAtual.Object.IsApoiado)
                                    SetAppForUser(new ApoiadoTheme(), new ApoiadoOrganizacaoShell());
                                else
                                    SetAppForUser(new VoluntarioTheme(), new VoluntarioOrganizacaoShell());
                            }
                            else
                            {
                                if (AuthHelper.UtilizadorAtual.Object.IsApoiado)
                                    SetAppForUser(new ApoiadoTheme(), new ApoiadoParticularShell());
                                else
                                    SetAppForUser(new VoluntarioTheme(), new VoluntarioParticularShell());
                            }
                        }
                        else
                            SetAppForUser(new AdminTheme(), new AdminShell());
                    }
                    catch
                    {
                        Application.Current.MainPage.ShowPopup(new MensagemPopUp("Erro", "Ocorreu um erro ao carregar os seus dados\nPor favor reinicie a sua conta", "Ok"));
                        AuthHelper.SairSessao();
                        Application.Current.MainPage = new AutenticacaoPage();
                    }
                }
                else
                {
                    Application.Current.MainPage = new AutenticacaoPage();
                }
                NavigationPage.SetHasNavigationBar(Application.Current, false);
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
        public static void SetAppForUser(ResourceDictionary theme, Page shell)
        {
            Application.Current.Resources.MergedDictionaries.Add(theme);
            Current.MainPage = shell;
        }
    }
}
