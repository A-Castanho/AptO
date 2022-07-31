using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Apoios.Notificacoes;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.PopUps.Mensagens;
using AppAptO.PopUps.Select;
using AppAptO.Services.Firebase;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Ofertas
{
    public class CrudOfertaViewModel : ObservableObject
    {
        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private bool visibilidadeEliminar;
        public bool VisibilidadeEliminar
        {
            get => visibilidadeEliminar;
            set => SetProperty(ref visibilidadeEliminar, value);
        }
        public ICommand SubmeterCommand => new Command(Submeter);
        public ICommand CommandEliminar { get; private set; }
        public ICommand CommandOpenSelectLocalidade => new Command(
        async () =>
        {
            try
            {
                var selecao = await Application.Current.MainPage.ShowPopupAsync(new SelectAndSearchPopUp(localidadesNomes));
                if (selecao != null)
                    LocalidadeSelecionada = selecao;
            }
            catch { }
        });

        private List<string> areas = new List<string>();
        public List<string> Areas
        {
            get => areas;
            set => SetProperty(ref areas, value);
        }
        private string areaSelecionada = "";
        public string AreaSelecionada
        {
            get => areaSelecionada;
            set
            {
                SetProperty(ref areaSelecionada, value);
                if (areaSelecionada == "Outro")
                {
                    VisibilidadeEntryArea = true;
                }
                else
                {
                    if (VisibilidadeEntryArea) { VisibilidadeEntryArea = false; }
                }
            }
        }
        private string areaPersonalizada;
        public string AreaPersonalizada
        {
            get => areaPersonalizada;
            set => SetProperty(ref areaPersonalizada, value);
        }
        private string localidadeSelecionada = "";
        public string LocalidadeSelecionada
        {
            get => localidadeSelecionada;
            set
            {
                SetProperty(ref localidadeSelecionada, value);
            }
        }
        public string NovaLocalidade { get; set; }
        private bool visibilidadeEntryArea = true;
        public bool VisibilidadeEntryArea
        {
            get => visibilidadeEntryArea;
            set
            {
                SetProperty(ref visibilidadeEntryArea, value);
                OfertaApoio.Area = "";
            }
        }

        //Dados da oferta de apoio
        private string ofertaApoiokey;

        private OfertaApoio ofertaApoio;
        private IEnumerable<string> localidadesNomes;

        public OfertaApoio OfertaApoio
        {
            get => ofertaApoio;
            set => SetProperty(ref ofertaApoio, value);
        }

        //Construtores
        public CrudOfertaViewModel(FirebaseObject<OfertaApoio> fbOferta)
        {
            Iniciar(fbOferta.Object, fbOferta.Key);
        }
        public CrudOfertaViewModel(OfertaApoio oferta, string ofertaKey)
        {
            Iniciar(oferta, ofertaKey);
        }
        private void Iniciar(OfertaApoio oferta, string ofertaKey)
        {
            VisibilidadeEliminar = true;
            OfertaApoio = oferta;
            ofertaApoiokey = ofertaKey;
            Title = "Editar Oferta";

            AreaPersonalizada = OfertaApoio.Area;
            AreaSelecionada = Areas.Contains(OfertaApoio.Area) ? OfertaApoio.Area : "Outro";
            LocalidadeSelecionada = OfertaApoio.Localidade;

            if (AuthHelper.UtilizadorAtual.Key == "Admin")
                CommandEliminar = new Command(EliminarAdmin);
            else
                CommandEliminar = new Command(EliminarProprio);

            SetDropdowns();
        }
        public CrudOfertaViewModel()
        {
            SetDropdowns();
            OfertaApoio = new OfertaApoio();
            Title = "Adicionar Oferta";

            AreaSelecionada = "Outro";
        }
        public async void EliminarProprio()
        {
            bool confirmacao = await Application.Current.MainPage.ShowPopupAsync(new EscolhaPopUp("Eliminar oferta", "De certeza que pretende eliminar esta oferta?"));
            if (confirmacao)
            {
                await FBDataBase.OfertasDS.DeleteByKey(ofertaApoiokey);
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Sucesso!", "A sua oferta foi eliminada.", "Ok"));
                await Shell.Current.Navigation.PopAsync();
            }
        }
        public async void EliminarAdmin()
        {
            var confirmacao = await Shell.Current.Navigation.ShowPopupAsync(new EscolhaPopUp("Cuidado!", "De certeza que pretende eliminar esta oferta de apoio do sistema?"));
            if (confirmacao)
            {
                var mensagem = await Shell.Current.Navigation.ShowPopupAsync(new InserirTextoPopUp("Eliminar publicação", "Insira o motivo de eliminação desta publicação"));
                if (!string.IsNullOrEmpty(mensagem))
                {
                    var apoiante = await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(ofertaApoio.UidApoiante);
                    apoiante.Object.Avisos.Add(new Aviso("Oferta de Apoio Eliminada",
                        "A sua oferta de apoio '" + OfertaApoio.Titulo + "' do dia " + OfertaApoio.DiaPublicacao.Date
                        + " foi eliminada pela administração.\nMotivo dado: \n" + mensagem,
                        "Oferta de apoio eliminada pela administração"));
                    await FBDataBase.OfertasDS.DeleteByKey(ofertaApoiokey);
                    await FBDataBase.UtilizadorDS.Update(apoiante.Object, apoiante.Key);
                    await Shell.Current.Navigation.ShowPopupAsync(new MensagemPopUp("Oferta eliminada", "A oferta de apoio foi eliminada com sucesso.", "Ok"));
                    await Shell.Current.Navigation.PopToRootAsync();
                }
                else
                    await Shell.Current.Navigation.ShowPopupAsync(new MensagemPopUp("Oferta não eliminada", "Não foi dado nenhum motivo para a eliminação logo a operação foi cancelada", "Ok"));
            }
        }

        private async void Submeter()
        {
            OfertaApoio.DiaPublicacao = DateTime.Now.Date;
            OfertaApoio.Area = AreaSelecionada == "Outro" ? AreaPersonalizada : AreaSelecionada;
            OfertaApoio.Localidade = LocalidadeSelecionada;
            string mensagem;
            if (!string.IsNullOrEmpty(ofertaApoiokey))
            {
                await FBDataBase.OfertasDS.Update(OfertaApoio, ofertaApoiokey);
                mensagem = "A oferta foi atualizada, agradecemos o seu apoio!";
            }
            else
            {
                OfertaApoio.UidApoiante = AuthHelper.UtilizadorAtual.Object.Uid;
                await FBDataBase.OfertasDS.Add(OfertaApoio);
                mensagem = "A oferta foi adicionada, agradecemos o seu apoio!";
            }
            Shell.Current.Navigation.ShowPopup(new MensagemPopUp("Sucesso!", mensagem, "Ok"));
            await App.Iniciar();
        }

        private async void SetDropdowns()
        {
            localidadesNomes = (await FBDataBase.LocalidadeDS.GetAllAsync()).Select(l => l.Object.Nome);
            Areas.AddRange((await FBDataBase.AreasApoioDS.GetAllAsync()).Select(fbArea => fbArea.Object.Nome));
        }
    }
}
