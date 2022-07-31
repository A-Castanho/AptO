using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps.Select;
using AppAptO.Services.Firebase;
using AppAptO.Views.Conta;
using Firebase.Database;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta
{
    public class ListagemContasViewModel : ObservableObject
    {
        Dictionary<string, List<int>> indicacoesAptidoes = new Dictionary<string, List<int>>();
        IEnumerable<FirebaseObject<Models.TiposDados.Aptidao>> aptidoes;
        public bool PesquisarComEmail { get; set; }
        private bool pesquisarVoluntarios = true;
        public bool PesquisarVoluntarios
        {
            get => pesquisarVoluntarios;
            set => SetProperty(ref pesquisarVoluntarios, value);
        }
        private bool pesquisarApoiados = true;
        public bool PesquisarApoiados
        {
            get => pesquisarApoiados;
            set => SetProperty(ref pesquisarApoiados, value);
        }
        private bool visibilidadeFiltros = false;
        public bool VisibilidadeFiltros
        {
            get => visibilidadeFiltros;
            set => SetProperty(ref visibilidadeFiltros, value);
        }
        private bool visibilidadeTextoAptidoes = false;
        public bool VisibilidadeTextoAptidoes
        {
            get => visibilidadeTextoAptidoes;
            set => SetProperty(ref visibilidadeTextoAptidoes, value);
        }
        private bool conteudoIniciado = true;
        public bool ConteudoIniciado
        {
            get => conteudoIniciado;
            set => SetProperty(ref conteudoIniciado, value);
        }
        private string textoAptidoes;
        public string TextoAptidoes
        {
            get => textoAptidoes;
            set
            {
                VisibilidadeTextoAptidoes = !string.IsNullOrEmpty(value);
                SetProperty(ref textoAptidoes, value);
            }
        }
        public string Localidade
        {
            get => localidade; private set
            {
                SetProperty(ref localidade, value);
            }
        }
        #region filtros
        private string pesquisa = "";
        public string Pesquisa
        {
            get => pesquisa;
            set
            {
                SetProperty(ref pesquisa, value);
            }
        }
        #endregion

        private FirebaseObject<Utilizador> utilizador;
        public FirebaseObject<Utilizador> UtilizadorSelecionado
        {
            get => utilizador;
            set => SetProperty(ref utilizador, value);
        }
        private ObservableCollection<FirebaseObject<Utilizador>> lista = new ObservableCollection<FirebaseObject<Utilizador>>();
        private string localidade;
        private IEnumerable<string> localidadesNomes;

        public ObservableCollection<FirebaseObject<Utilizador>> Lista
        {
            get => lista;
            set => SetProperty(ref lista, value);
        }
        public IEnumerable<FirebaseObject<Utilizador>> ListaOriginal { get; set; }


        public ICommand CommandOpenSelectLocalidade => new Command(
            async () => Localidade = await Application.Current.MainPage.ShowPopupAsync(new SelectAndSearchPopUp(localidadesNomes)));
        public ICommand CommandOpenFiltros => new Command(
            () => VisibilidadeFiltros = !VisibilidadeFiltros);
        public ICommand CommandLimparPesquisa => new Command(
        async () =>
        {
            Pesquisa = "";
            Localidade = "Portugal";
            indicacoesAptidoes = null;
            TextoAptidoes = "";
            await IniciarLista();
            PesquisarLista();
        });
        public ICommand CommandOpenAptidoes => new Command(
        async () =>
        {
            //Receber aptidões para filtrar
            MessagingCenter.Subscribe<Dictionary<string, List<int>>>(this, "FiltroAptidoes", (indicadorSelecionado) =>
            {
                indicacoesAptidoes = indicadorSelecionado;
            });
            await Shell.Current.Navigation.ShowPopupAsync(new SelectAptidoesPopUp(Lista));
            MessagingCenter.Unsubscribe<string>(this, "FiltroAptidoes");

            if (indicacoesAptidoes.Count > 0)
            {
                //Constroi o texto para dizer quais os filtros
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Filtros de aptidões: ");
                stringBuilder.AppendLine();
                foreach (var ap in indicacoesAptidoes)
                {
                    var aptidao = aptidoes.FirstOrDefault(a => a.Key == ap.Key);
                    foreach (var apInterior in ap.Value)
                    {
                        stringBuilder.Append(aptidao.Object.AreasEnglobadas[apInterior] + ", ");
                    }
                }
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
                TextoAptidoes = stringBuilder.ToString();
            }
            else
                TextoAptidoes = "";
        });

        public ICommand CommandPesquisar => new Command(
            () => PesquisarLista());
        public ICommand CommandGoToUtilizador => new Command(
            async () => await Shell.Current.Navigation.PushAsync(new PerfilPage(UtilizadorSelecionado.Object, UtilizadorSelecionado.Key)));


        public ListagemContasViewModel()
        {
            try
            {
                Iniciar();
                PesquisarLista();
            }
            catch { }
        }

        private async void Iniciar()
        {
            aptidoes = await FBDataBase.AptidoesDS.GetAllAsync();
            localidadesNomes = (await FBDataBase.LocalidadeDS.GetAllAsync()).Select(l => l.Object.Nome);
            Localidade = "Portugal";
        }

        public async void PesquisarLista()
        {
            ConteudoIniciado = false;
            if (ListaOriginal != null)
            {
                FiltrarLista();
            }
            else
            {
                await IniciarLista();
                PesquisarLista();
            }
            ConteudoIniciado = true;
        }

        private void FiltrarLista()
        {
            try
            {
                //Filtrar pela pesquisa em texto - Selecionar utilizadores que têm o nome ou email com o texto de pesquisa
                IEnumerable<FirebaseObject<Utilizador>> listaFiltrada = ListaOriginal;
                if (!string.IsNullOrWhiteSpace(Pesquisa.Trim()))
                {
                    if (PesquisarComEmail)
                        listaFiltrada = listaFiltrada.Where(utilizador => utilizador.Object.Email.ToLower().Contains(Pesquisa.ToLower().Trim()) ||
                            utilizador.Object.NomeExibicao.ToLower().Trim().Contains(Pesquisa.ToLower().Trim()));
                    else
                        listaFiltrada = listaFiltrada.Where(utilizador => utilizador.Object.NomeExibicao.ToLower().Trim().Contains(Pesquisa.ToLower().Trim()));
                }
                //Filtrar por apoiado ou voluntário
                if (!PesquisarApoiados || !PesquisarVoluntarios)
                {
                    if (!PesquisarApoiados)
                        listaFiltrada = listaFiltrada.Where(u => !u.Object.IsApoiado);
                    if (!PesquisarVoluntarios)
                        listaFiltrada = listaFiltrada.Where(u => !u.Object.IsVoluntario);
                }

                //Filtrar pela localidade
                if (!string.IsNullOrWhiteSpace(Localidade))
                {
                    if (Localidade != "Portugal")
                    {
                        listaFiltrada = listaFiltrada.Where
                            (elemento => elemento.Object.Localidade.StartsWith(Localidade));
                    }
                }
                //Filtrar pelas aptidões - Selecionar os utilizadores que contêm as aptidões selecionadas
                if (indicacoesAptidoes != null && indicacoesAptidoes.Count > 0)
                {
                    foreach (var aptidao in indicacoesAptidoes)
                    {
                        listaFiltrada = listaFiltrada.Where(
                            u => u.Object.Aptidoes.ContainsKey(aptidao.Key)
                        );
                    }
                }
                Lista = new ObservableCollection<FirebaseObject<Utilizador>>(listaFiltrada);
            }
            catch
            {
                //await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Não foi possível filtrar o conteudo", "Ok"));

            }
        }

        private async Task IniciarLista()
        {
            var dadosOriginais = new List<FirebaseObject<Utilizador>>(await FBDataBase.UtilizadorDS.GetAllAsync<Utilizador>());
            dadosOriginais.Remove(dadosOriginais.FirstOrDefault(u => u.Key == "Admin"));
            ListaOriginal = dadosOriginais;
        }
    }
}
