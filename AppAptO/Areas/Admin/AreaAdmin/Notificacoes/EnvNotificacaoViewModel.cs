using AppAptO.Models.FBData.Apoios.Notificacoes;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.Services.Firebase;
using Firebase.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.Areas.Admin.AreaAdmin.Notificacoes
{
    public class EnvNotificacaoViewModel : ObservableObject
    {
        private ObservableCollection<string> keysUtilizadoresSelecionados = new ObservableCollection<string>();
        private ObservableCollection<string> parametrosFiltros;
        private string parametroFiltro;
        private ObservableCollection<FirebaseObject<Utilizador>> utilizadores;
        private ObservableCollection<FirebaseObject<Utilizador>> utilizadoresSelecionados;
        private bool visUtilizadores;
        private bool verSelecionados;
        private bool isBusy;

        public string Titulo { get; set; }
        public string Pesquisa { get; set; } = "";
        public string Mensagem { get; set; }
        public bool VisUtilizadores { get => visUtilizadores; set => SetProperty(ref visUtilizadores, value); }
        public bool VerSelecionados { get => verSelecionados; set => SetProperty(ref verSelecionados, value); }
        public bool IsBusy { get => isBusy; set => SetProperty(ref isBusy, value); }
        public ObservableCollection<FirebaseObject<Utilizador>> utilizadoresOriginais;

        public ObservableCollection<FirebaseObject<Utilizador>> Utilizadores { get => utilizadores; set => SetProperty(ref utilizadores, value); }
        public ObservableCollection<FirebaseObject<Utilizador>> UtilizadoresSelecionados { get => utilizadoresSelecionados; set => SetProperty(ref utilizadoresSelecionados, value); }
        public ObservableCollection<string> KeysUtilizadoresSelecionados { get => keysUtilizadoresSelecionados; set => SetProperty(ref keysUtilizadoresSelecionados, value); }
        public ObservableCollection<string> ParametrosFiltros { get => parametrosFiltros; set => SetProperty(ref parametrosFiltros, value); }
        public string ParametroFiltro { get => parametroFiltro; set => SetProperty(ref parametroFiltro, value); }
        public ICommand CommandSubmitUtilizadores => new Command(
        () =>
        {
            KeysUtilizadoresSelecionados = new ObservableCollection<string>(UtilizadoresSelecionados.Select(u => u.Key));
        });
        public ICommand CommandAlterarVisibilidadeArea => new Command<string>(
        (string area) => VisUtilizadores = area == "Utilizadores");
        public ICommand CommandSelectList => new Command(
        async () =>
        {
            bool confirmacao = await App.Current.MainPage.ShowPopupAsync(new EscolhaPopUp("Seleção de Utilizadores", "Pretende selecionar todos os utilizadores da pesquisa atual?"));
            if (confirmacao)
                KeysUtilizadoresSelecionados = new ObservableCollection<string>(Utilizadores.Select(u => u.Key));
        });
        public ICommand CommandPesquisar => new Command(
        async () =>
        {
            if (VerSelecionados)
                Utilizadores = utilizadoresOriginais;
            else
                Utilizadores = new ObservableCollection<FirebaseObject<Utilizador>>(utilizadoresOriginais.Where(u => !KeysUtilizadoresSelecionados.Contains(u.Key)));
            if (Pesquisa != "")
            {
                switch (ParametroFiltro)
                {
                    case "Uid":
                        Utilizadores = new ObservableCollection<FirebaseObject<Utilizador>>(
                        Utilizadores.Where
                        (
                            u => (u.Object.Uid.Trim().Contains(Pesquisa.Trim())
                        )));
                        break;
                    case "Key":
                        Utilizadores = new ObservableCollection<FirebaseObject<Utilizador>>(
                        Utilizadores.Where
                        (
                            u => (u.Key.Trim().Contains(Pesquisa.Trim())
                        )));
                        break;
                    case "Nome":
                        Utilizadores = new ObservableCollection<FirebaseObject<Utilizador>>(
                        Utilizadores.Where
                        (
                            u => (u.Object.NomeExibicao.ToLower().Trim().Contains(Pesquisa.ToLower().Trim())
                        )));
                        break;
                    case "Email":
                        Utilizadores = new ObservableCollection<FirebaseObject<Utilizador>>(
                        Utilizadores.Where
                        (
                            u => (u.Object.Email.ToLower().Trim().Contains(Pesquisa.ToLower().Trim())
                        )));
                        break;
                }
            }
        });
        public ICommand CommandSelectUtilizador => new Command<string>(
        async (string keyUtilizador) =>
        {
            if (KeysUtilizadoresSelecionados.Contains(keyUtilizador))
                await RemoverUtilizador(keyUtilizador);
            else
                await AdicioonarUtilizador(keyUtilizador);
            if (!VerSelecionados)
                CommandPesquisar.Execute(null);
        });

        private async Task AdicioonarUtilizador(string keyUtilizador)
        {
            var confirmacao = await App.Current.MainPage.ShowPopupAsync(new EscolhaPopUp("Utilizadores", "Pretende adicionar o utilizador "
                                                                            + Utilizadores.First(u => u.Key == keyUtilizador).Object.NomeExibicao +
                                                                            " à lista de utilizadores a que enviar a notificação?"));
            if (confirmacao)
                KeysUtilizadoresSelecionados.Add(keyUtilizador);
        }
        private async Task RemoverUtilizador(string keyUtilizador)
        {
            var confirmacao = await App.Current.MainPage.ShowPopupAsync(new EscolhaPopUp("Utilizadores", "Pretende remover o utilizador "
                                                                            + Utilizadores.First(u => u.Key == keyUtilizador).Object.NomeExibicao +
                                                                            " à lista de utilizadores a que enviar a notificação?"));
            if (confirmacao)
                KeysUtilizadoresSelecionados.Remove(keyUtilizador);
        }

        public ICommand CommandSubmitNotificacao => new Command(
        async () =>
        {
            if (KeysUtilizadoresSelecionados.Count > 0)
            {
                if (!string.IsNullOrEmpty(Titulo) && !string.IsNullOrEmpty(Mensagem))
                {
                    bool confirmacao = await App.Current.MainPage.ShowPopupAsync(new EscolhaPopUp("Envio da notificação", "De certeza que pretende enviar a notificação para " + KeysUtilizadoresSelecionados.Count + " utilizadores?"));
                    if (confirmacao)
                    {
                        Aviso notificacao = new Aviso(Titulo, Mensagem, "Mensagem da administração");
                        await EnviarNotificacao(notificacao);
                        await App.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Envio da notificação", "Notificação foi enviada aos utilizadores selecionados."));
                    }
                }
                else
                    await App.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Envio inválido", "Todos os campos devem ser preenchidos."));
            }
            else
                await App.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Envio inválido", "Não foram selecionados quaisquer utilizadores para receberem esta notificação"));
            KeysUtilizadoresSelecionados = new ObservableCollection<string>();
        });

        public EnvNotificacaoViewModel()
        {
            IniciarUtilizadores();
            InciarFiltro();
        }

        private void InciarFiltro()
        {
            ParametrosFiltros = new ObservableCollection<string>
            {
                "Uid",
                "Key",
                "Nome",
                "Email"
            };
            ParametroFiltro = "Nome";
        }

        private async Task EnviarNotificacao(Aviso notificacao)
        {
            IsBusy = true;
            foreach (string key in KeysUtilizadoresSelecionados)
            {
                notificacao.RecetorKey = key;
                var utilizador = await Services.Firebase.FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(key);
                utilizador.Avisos.Add(notificacao);
                await FBDataBase.UtilizadorDS.Update(utilizador, key);
            }
            IsBusy = false;
        }
        private async void IniciarUtilizadores()
        {
            utilizadoresOriginais = new ObservableCollection<FirebaseObject<Utilizador>>(await FBDataBase.UtilizadorDS.GetAllAsync<Utilizador>());
            utilizadoresOriginais.Remove(utilizadoresOriginais.First(u => u.Key == "Admin"));
            Utilizadores = utilizadoresOriginais;
        }
    }
}
