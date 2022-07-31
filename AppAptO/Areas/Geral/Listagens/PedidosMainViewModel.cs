using AppAptO.Areas.Admin.Apoios.Pedidos;
using AppAptO.Models.AppHelpers;
using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.TiposDados;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.PopUps.Select;
using AppAptO.Services.Firebase;
using AppAptO.Views.Pedidos;
using Firebase.Database;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Pedidos
{
    class PedidosMainViewModel : ObservableObject
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
                            await Shell.Current.Navigation.PushAsync(new AdminPedidoApoioPage(this.PedidoApoio));
                        else
                            await Shell.Current.Navigation.PushAsync(new PedidoApoioPage(this.PedidoApoio.Object, PedidoApoio.Key));
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
            public FirebaseObject<PedidoApoio> PedidoApoio { get; set; }
            public Utilizador Utilizador { get; set; }
            private bool visibilidadeDetalhes;
            public bool VisibilidadeDetalhes
            {
                get => visibilidadeDetalhes; set
                {
                    SetProperty(ref visibilidadeDetalhes, value);
                }
            }
            public ElementoLista(FirebaseObject<PedidoApoio> fbPedidoApoio, Utilizador utilizador)
            {
                Utilizador = utilizador;
                PedidoApoio = fbPedidoApoio;
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

        private ObservableCollection<ElementoLista> elementosLista;

        public ICommand CommandIniciar => new Command(Iniciar);
        public ICommand ComandoVisibilidadeFiltros => new Command(
            execute: () => VisibilidadeFiltros = !VisibilidadeFiltros);
        public ICommand ComandoGoToAdicionar => new Command(
            execute: async () => await Shell.Current.Navigation.PushAsync(new CrudPedidoPage()));
        public ICommand CommandOpenSelectLocalidade => new Command(
        async () =>
        {
            try
            {
                var localidade = await Application.Current.MainPage.ShowPopupAsync(new SelectAndSearchPopUp(localidadesNomes));
                if (!string.IsNullOrEmpty(localidade))
                    LocalidadeSelecionada = localidade;
            }
            catch { }
        });
        public ICommand ComandoMostrarDetalhes => new Command<ElementoLista>(
        (ElementoLista elemento) =>
        {
            try
            {
                ElementosLista[ElementosLista.IndexOf(elemento)].VisibilidadeDetalhes = !ElementosLista[ElementosLista.IndexOf(elemento)].VisibilidadeDetalhes;
            }
            catch { }
        });
        public PedidosMainViewModel()
        {
            ElementosListaOriginal = new List<ElementoLista>();
            ElementosLista = new ObservableCollection<ElementoLista>();
            ConteudoDropdowAreas = new ObservableCollection<string>();
            VisibilidadeAdicao = AuthHelper.UtilizadorAtual.Object.IsApoiado;
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
            int totalpedidos = 0;
            IEnumerable<FirebaseObject<AreaApoio>> areas = await FBDataBase.AreasApoioDS.GetAllAsync();
            NomesAreas = new ObservableCollection<string>(areas.Select(area => area.Object.Nome));
            foreach (FirebaseObject<AreaApoio> fbArea in areas)
            {
                totalpedidos += fbArea.Object.NPedidos;
                ConteudoAdicionalAreas.Add("");
                ConteudoDropdowAreas.Add(ConteudoAdicionalAreas.Last() + fbArea.Object.Nome);
            }
            dropdownTodasAreas = "Todas as Áreas";

            ConteudoDropdowAreas.Add(dropdownTodasAreas);

            AreaSelecionada = dropdownTodasAreas;

            localidadesNomes = (await FBDataBase.LocalidadeDS.GetAllAsync()).Select(l => l.Object.Nome);
            LocalidadeSelecionada = "Portugal";

            IEnumerable<FirebaseObject<PedidoApoio>> pedidos = await FBDataBase.PedidosDS.GetAllPostVisAsync();
            int i = -1;
            foreach (FirebaseObject<PedidoApoio> pedido in pedidos)
            {
                i++;
                Utilizador utilizador = (await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(pedido.Object.UidApoiado)).Object;
                if (utilizador != null)
                {
                    ElementosListaOriginal.Add(new ElementoLista(pedido, utilizador));
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

                    areas.Remove(AreaSelecionada.Replace("Outro", ""));

                    foreach (string area in areas)
                    {
                        listaFiltrada.RemoveAll(elemento => elemento.PedidoApoio.Object.Area == area);
                    }
                    ElementosLista = new ObservableCollection<ElementoLista>(listaFiltrada);
                }
                else
                {
                    indexAreaSelecionada = ConteudoDropdowAreas.IndexOf(AreaSelecionada);
                    areaAProcurar = AreaSelecionada;
                    List<ElementoLista> listaFiltrada = ElementosListaOriginal.FindAll(elemento => elemento.PedidoApoio.Object.Area == areaAProcurar);
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
                    .FindAll(elemento => elemento.PedidoApoio.Object.Localidade.Contains(localidadeSelecionada));
                ElementosLista = new ObservableCollection<ElementoLista>(listaFiltrada);
            }
        }
    }
}
