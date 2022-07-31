using AppAptO.Models.FBConnections;
using AppAptO.Models.TiposDados;
using AppAptO.Services.Firebase;
using Firebase.Database;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.PopUps.Autenticacao
{
    public class AptidoesViewModel : ObservableObject
    {
        public class ElementoFbArea : ObservableObject
        {
            public ICommand CommandMudarVisibilidade => new Command(
            () =>
            {
                Visibilidade = !Visibilidade;
            });
            public double ListHeight { get => listHeight; set => SetProperty(ref listHeight, value); }

            private FirebaseObject<Aptidao> areaExperiencia;
            public FirebaseObject<Aptidao> AreaExperiencia
            {
                get { return areaExperiencia; }
                set { SetProperty(ref areaExperiencia, value); }
            }
            private bool visibilidade;
            private double listHeight = 0;

            public ElementoFbArea(FirebaseObject<Aptidao> area)
            {
                AreaExperiencia = area;
                ListHeight = AreaExperiencia.Object.AreasEnglobadas.Count * 52;
            }

            public bool Visibilidade
            {
                get { return visibilidade; }
                set { SetProperty(ref visibilidade, value); }
            }

        }

        private bool visibilidadeEscolhidas;
        private ObservableCollection<ElementoFbArea> areasGerais = new ObservableCollection<ElementoFbArea>();
        private ElementoFbArea areaGeralSelecionada;
        private ObservableCollection<string> titulosAreasInteriores = new ObservableCollection<string>();
        private ObservableCollection<string> titulosAreasInterioresSelecionadas = new ObservableCollection<string>();
        private ObservableCollection<string> titulosAreasEscolhidas = new ObservableCollection<string>();
        private Dictionary<string, Aptidao> areasInterioresProprias = new Dictionary<string, Aptidao>();

        public bool VisibilidadeEscolhidas
        {
            get { return visibilidadeEscolhidas; }
            set { SetProperty(ref visibilidadeEscolhidas, value); }
        }
        public ObservableCollection<ElementoFbArea> AreasGerais
        {
            get => areasGerais; set
            {
                SetProperty(ref areasGerais, value);
            }
        }
        public ElementoFbArea AreaGeralSelecionada
        {
            get => areaGeralSelecionada; set
            {
                SetProperty(ref areaGeralSelecionada, value);
                if (value != null)
                {
                    TitulosAreasInteriores = new ObservableCollection<string>(AreasGerais.FirstOrDefault(a => a.AreaExperiencia.Key == value.AreaExperiencia.Key).AreaExperiencia.Object.AreasEnglobadas);
                    TitulosAreasInterioresSelecionadas = new ObservableCollection<string>(AreasInterioresProprias[value.AreaExperiencia.Key].AreasEnglobadas);
                }
            }
        }
        public ObservableCollection<string> TitulosAreasInteriores
        {
            get => titulosAreasInteriores; set
            {
                SetProperty(ref titulosAreasInteriores, new ObservableCollection<string>(value));
            }
        }
        public ObservableCollection<string> TitulosAreasInterioresSelecionadas
        {
            get => titulosAreasInterioresSelecionadas; set
            {
                SetProperty(ref titulosAreasInterioresSelecionadas, new ObservableCollection<string>(value));
                AreasInterioresProprias[AreaGeralSelecionada.AreaExperiencia.Key].AreasEnglobadas = value.ToList();
                TitulosAreasEscolhidas.Clear();
                foreach (var area in AreasInterioresProprias)
                {
                    foreach (var areainterior in area.Value.AreasEnglobadas)
                    {
                        TitulosAreasEscolhidas.Add(areainterior);
                    }
                }
            }
        }
        public ObservableCollection<string> TitulosAreasEscolhidas
        {
            get => titulosAreasEscolhidas; set
            {
                VisibilidadeEscolhidas = value.ToList().Count > 0;
                SetProperty(ref titulosAreasEscolhidas, value);
            }
        }
        //string é a chave da área, AreaExperiencia é a área com a lista dos selecionados
        public Dictionary<string, Aptidao> AreasInterioresProprias
        {
            get => areasInterioresProprias; set
            {
                SetProperty(ref areasInterioresProprias, value);
            }
        }
        public AptidoesViewModel()
        {
            Iniciar();
        }
        private async void Iniciar()
        {
            var fbAreas = await FBDataBase.AptidoesDS.GetAllAsync();
            AreasGerais = new ObservableCollection<ElementoFbArea>(fbAreas.Select(a => new ElementoFbArea(a)));
            foreach (var area in AreasGerais)
            {
                AreasInterioresProprias.Add(area.AreaExperiencia.Key, new Aptidao(area.AreaExperiencia.Object.NomeGeral, new List<string>()));
            }
            TitulosAreasInterioresSelecionadas.CollectionChanged += TitulosAreasInterioresSelecionadas_CollectionChanged;
        }
        public ICommand CommandSubmeter => new Command(
        async () =>
        {
            Dictionary<string, List<int>> indicacoesAreasEscolhidas = new Dictionary<string, List<int>>();
            //Encontrar os indexes para cada area geral em que se escolheu
            foreach (var area in AreasInterioresProprias.Where(a => a.Value.AreasEnglobadas.Count > 0))
            {
                List<int> listaIndexes = new List<int>();
                var firebaseArea = AreasGerais.First(fbArea => fbArea.AreaExperiencia.Key == area.Key);
                foreach (var areaInterior in area.Value.AreasEnglobadas)
                {
                    int indexAreaUsada = firebaseArea.AreaExperiencia.Object.AreasEnglobadas.IndexOf(areaInterior);
                    listaIndexes.Add(indexAreaUsada);
                }
                indicacoesAreasEscolhidas.Add(firebaseArea.AreaExperiencia.Key, listaIndexes);
            }

            var utilizadorAtual = AuthHelper.UtilizadorAtual;
            utilizadorAtual.Object.Aptidoes = indicacoesAreasEscolhidas;
            await FBDataBase.UtilizadorDS.Update(utilizadorAtual.Object, utilizadorAtual.Key);
            await App.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Sucesso", "A sua lista de aptidões foi atualizada!"));
            await Shell.Current.Navigation.PopAsync();
        });
        public ICommand CommandMudarVisibilidade => new Command(
        () =>
        {
            AreaGeralSelecionada.Visibilidade = !AreaGeralSelecionada.Visibilidade;
        });
        //chamado do OnSelecttionChanged?
        private void TitulosAreasInterioresSelecionadas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                string area = (string)sender;
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    if (!AreasInterioresProprias[AreaGeralSelecionada.AreaExperiencia.Key].AreasEnglobadas.Contains(area))
                        AreasInterioresProprias[AreaGeralSelecionada.AreaExperiencia.Key].AreasEnglobadas.Add(area);
                }
                else
                {
                    if (e.Action == NotifyCollectionChangedAction.Remove)
                    {
                        if (AreasInterioresProprias[AreaGeralSelecionada.AreaExperiencia.Key].AreasEnglobadas.Contains(area))
                            AreasInterioresProprias[AreaGeralSelecionada.AreaExperiencia.Key].AreasEnglobadas.Remove(area);
                    }
                }
            }
        }
    }
}