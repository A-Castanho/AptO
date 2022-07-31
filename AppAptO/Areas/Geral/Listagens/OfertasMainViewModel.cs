using AppAptO.Areas.Admin.Apoios.Ofertas;
using AppAptO.Models.AppHelpers;
using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.TiposDados;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.PopUps.Select;
using AppAptO.Services.Firebase;
using AppAptO.Views.Ofertas;
using Firebase.Database;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Ofertas
{
    public class OfertasMainViewModel : ObservableObject
    {
        private bool visibilidadeAdicao;
        public bool VisibilidadeAdicao
        {
            get { return visibilidadeAdicao; }
            set { SetProperty(ref visibilidadeAdicao, value); }
        }
        public class ElementoLista : ObservableObject
        {
            public ICommand ComandoAnalisarElemento => new Command(
            async () =>
            {
                if (AppConnection.IsConnected)
                {
                    try
                    {
                        if (AuthHelper.UtilizadorAtual.Key == "Admin")
                            await Shell.Current.Navigation.PushAsync(new AdminOfertaApoioPage(this.OfertaApoio));
                        else
                            await Shell.Current.Navigation.PushAsync(new OfertaApoioPage(this.OfertaApoio));
                    }
                    catch
                    {
                        await Shell.Current.Navigation.ShowPopupAsync(new MensagemPopUp("Erro", "Não foi possível aceder à publicação"));
                    }
                }
                else
                {
                    await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "A operação foi cancelada devido à falta de conexão na internet", "Ok"));
                }
                
            });
            private bool visibilidadeDetalhes;

            public FirebaseObject<OfertaApoio> OfertaApoio { get; set; }
            public Utilizador Utilizador { get; set; }
            public bool VisibilidadeDetalhes
            {
                get => visibilidadeDetalhes; set
                {
                    SetProperty(ref visibilidadeDetalhes, value);
                }
            }
            public ElementoLista(FirebaseObject<OfertaApoio> fbOfertaApoio, Utilizador utilizador)
            {
                Utilizador = utilizador;
                OfertaApoio = fbOfertaApoio;
            }
            public ElementoLista() { }
        }

        #region campos e properties de uso ElementoLista

        private ElementoLista elementoSelecionado = new ElementoLista();
        public ElementoLista ElementoSelecionado
        {
            get => elementoSelecionado;
            set => SetProperty(ref elementoSelecionado, value);
        }
        private List<ElementoLista> ElementosListaOriginal;
        public ObservableCollection<ElementoLista> ElementosLista
        {
            get => elementosLista;
            set => SetProperty(ref elementosLista, value);
        }
        #endregion
        #region Filtros
        private bool conteudoIniciado;
        public bool ConteudoIniciado
        {
            get => conteudoIniciado;
            set => SetProperty(ref conteudoIniciado, value);
        }
        private bool visibilidadeFiltros;
        public bool VisibilidadeFiltros
        {
            get => visibilidadeFiltros;
            set => SetProperty(ref visibilidadeFiltros, value);
        }

        private IEnumerable<string> localidadesNomes;
        //Parte adicionada ao nome da area no texto do dropdown
        private List<string> ConteudoAdicionalAreas;
        //Texto que aprece no dropdown das áreas
        public ObservableCollection<string> ConteudoDropdowAreas { get; set; }
        public ObservableCollection<string> NomesAreas { get; set; }
        //Instância do dropdown referente a todas as áreas
        private string dropdownTodasAreas = "";
        //Área selecionada no dropdown
        private string areaSelecionada = "";
        public string AreaSelecionada
        {
            get => areaSelecionada;
            set
            {
                SetProperty(ref areaSelecionada, value);
                FiltrarArea();
            }
        }

        //Localidade selecionada no dropdown
        private string localidadeSelecionada = "";
        public string LocalidadeSelecionada
        {
            get => localidadeSelecionada;
            set
            {
                SetProperty(ref localidadeSelecionada, value);
                FiltrarLocalidade();
            }
        }
        #endregion

        private ObservableCollection<ElementoLista> elementosLista = new ObservableCollection<ElementoLista>();

        #region commands
        public ICommand CommandIniciar => new Command(Iniciar);
        public ICommand ComandoVisibilidadeFiltros => new Command(
            () => VisibilidadeFiltros = !VisibilidadeFiltros);
        public ICommand ComandoGoToAdicionar => new Command(
            async () => await Shell.Current.Navigation.PushAsync(new CrudOfertaPage()));

        public ICommand CommandOpenSelectLocalidade => new Command(
            async () =>
            {
                var localidade = await Application.Current.MainPage.ShowPopupAsync(new SelectAndSearchPopUp(localidadesNomes));
                if (!string.IsNullOrEmpty(localidade))
                    LocalidadeSelecionada = localidade;
            });
        public ICommand ComandoMostrarDetalhes => new Command<ElementoLista>(
            (ElementoLista elemento) => ElementosLista[ElementosLista.IndexOf(elemento)].VisibilidadeDetalhes = !ElementosLista[ElementosLista.IndexOf(elemento)].VisibilidadeDetalhes);
        #endregion

        public OfertasMainViewModel()
        {
            ElementosListaOriginal = new List<ElementoLista>();
            ElementosLista = new ObservableCollection<ElementoLista>();
            ConteudoDropdowAreas = new ObservableCollection<string>();
            VisibilidadeAdicao = AuthHelper.UtilizadorAtual.Object.IsVoluntario;
            Iniciar();
        }

        /// <summary>
        /// Prepara o sistema da lista de elementos a apresentar
        /// </summary>
        private async void Iniciar()
        {
            ConteudoIniciado = false;
            LimparListas();
            await IniciarComponentes();
            ConteudoIniciado = true;
        }

        private async Task IniciarComponentes()
        {
            //Iniciar Areas com as armazenadas na base de dados + uma opção para todas as Areas
            int totalOfertas = 0;
            IEnumerable<FirebaseObject<AreaApoio>> areas = await FBDataBase.AreasApoioDS.GetAllAsync();
            NomesAreas = new ObservableCollection<string>(areas.Select(area => area.Object.Nome));
            foreach (FirebaseObject<AreaApoio> fbArea in areas)
            {
                totalOfertas += fbArea.Object.NOfertas;
                ConteudoAdicionalAreas.Add("");
                ConteudoDropdowAreas.Add(ConteudoAdicionalAreas.Last() + fbArea.Object.Nome);
            }
            dropdownTodasAreas = "Todas as Áreas";

            ConteudoDropdowAreas.Add(dropdownTodasAreas);

            AreaSelecionada = dropdownTodasAreas;

            localidadesNomes = (await FBDataBase.LocalidadeDS.GetAllAsync()).Select(l => l.Object.Nome);
            LocalidadeSelecionada = "Portugal";

            IEnumerable<FirebaseObject<OfertaApoio>> ofertas = await FBDataBase.OfertasDS.GetAllAsync();
            int i = -1;
            foreach (FirebaseObject<OfertaApoio> oferta in ofertas)
            {
                i++;
                Utilizador utilizador = (await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(oferta.Object.UidApoiante)).Object;
                if (utilizador != null)
                {
                    ElementosListaOriginal.Add(new ElementoLista(oferta, utilizador));
                }
            }
            ElementosLista = new ObservableCollection<ElementoLista>(ElementosListaOriginal);
        }

        private void LimparListas()
        {
            //Inicializar as listas
            ConteudoDropdowAreas = new ObservableCollection<string>();
            ConteudoAdicionalAreas = new List<string>();
            ElementosListaOriginal = new List<ElementoLista>();
            ElementosLista = new ObservableCollection<ElementoLista>();
        }

        private void FiltrarArea()
        {
            if (AreaSelecionada == dropdownTodasAreas)
            {
                ElementosLista = new ObservableCollection<ElementoLista>(ElementosListaOriginal);
            }
            else
            {
                int indexAreaSelecionada = ConteudoDropdowAreas.IndexOf(AreaSelecionada);
                string areaAProcurar = NomesAreas[indexAreaSelecionada];
                if (areaAProcurar == "Outro")
                {
                    List<ElementoLista> listaFiltrada = ElementosListaOriginal;

                    List<string> areas = ConteudoDropdowAreas.ToList();
                    areas.Remove(AreaSelecionada);

                    List<string> textoAdicionalAreas = ConteudoAdicionalAreas;
                    areas.Remove(AreaSelecionada.Replace("Outro", ""));

                    foreach (string area in areas)
                    {
                        listaFiltrada.RemoveAll(elemento => elemento.OfertaApoio.Object.Area == area);
                    }
                    ElementosLista = new ObservableCollection<ElementoLista>(listaFiltrada);
                }
                else
                {
                    indexAreaSelecionada = ConteudoDropdowAreas.IndexOf(AreaSelecionada);
                    areaAProcurar = AreaSelecionada;
                    List<ElementoLista> listaFiltrada = ElementosListaOriginal.FindAll(elemento => elemento.OfertaApoio.Object.Area == areaAProcurar);
                    ElementosLista = new ObservableCollection<ElementoLista>(listaFiltrada);
                }
            }
        }
        private void FiltrarLocalidade()
        {
            if (LocalidadeSelecionada == "Portugal")
            {
                ElementosLista = new ObservableCollection<ElementoLista>(ElementosListaOriginal);
            }
            else
            {
                List<ElementoLista> listaFiltrada = ElementosListaOriginal
                    .FindAll(elemento => elemento.OfertaApoio.Object.Localidade.Contains(localidadeSelecionada));
                ElementosLista = new ObservableCollection<ElementoLista>(listaFiltrada);
            }
        }
    }
}
