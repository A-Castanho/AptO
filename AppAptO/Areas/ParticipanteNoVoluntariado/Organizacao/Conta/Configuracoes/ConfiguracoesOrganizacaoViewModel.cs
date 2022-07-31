using AppAptO.Areas.ParticipanteNoVoluntariado.Organizacao.Conta.Configuracoes;
using AppAptO.Areas.ParticipanteNoVoluntariado.Particular.Conta.Configuracoes;
using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using AppAptO.ViewsPartial.Conta.Configuracoes;
using Firebase.Database;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.Views.Conta
{
    public class ConfiguracoesOrganizacaoViewModel : ObservableObject
    {
        private Dictionary<string, ContentView> DictionaryViews;

        private string title;
        public string Title
        {
            get => title;
            set
            {
                SetProperty(ref title, value);
            }
        }
        private UtilizadorOrganizacao utilizador;
        public UtilizadorOrganizacao Utilizador
        {
            get => utilizador;
            set
            {
                SetProperty(ref utilizador, value);
            }
        }

        private string keyUtilizador;

        public ICommand SelecionarViewCommand => new Command<string>(
        (string apontadorView) =>
        {
            Title = apontadorView;
            ViewAtual = DictionaryViews[apontadorView];
        });

        private ContentView viewAtual = new ContentView();
        public ContentView ViewAtual
        {
            get => viewAtual;
            set { SetProperty(ref viewAtual, value); }
        }
        public ConfiguracoesOrganizacaoViewModel(FirebaseObject<UtilizadorOrganizacao> fbutilizador)
        {
            Utilizador = fbutilizador.Object;
            keyUtilizador = fbutilizador.Key;
            Iniciar(fbutilizador.Object, fbutilizador.Key);
        }

        public ConfiguracoesOrganizacaoViewModel(UtilizadorOrganizacao utilizador, string key)
        {
            Utilizador = utilizador;
            keyUtilizador = key;
            Iniciar(utilizador, key);
        }

        public ConfiguracoesOrganizacaoViewModel()
        {
            Iniciar();
        }
        private async void Iniciar(UtilizadorOrganizacao utilizador = null, string key = null)
        {
            if (utilizador == null || key == null)
            {
                var fbUtilizador = await FBDataBase.UtilizadorDS.GetByUidAsync<UtilizadorOrganizacao>(AuthHelper.UtilizadorAtual.Object.Uid);
                utilizador = fbUtilizador.Object;
                key = fbUtilizador.Key;
            }

            DictionaryViews = new Dictionary<string, ContentView>
                {
                    {"Dados Pessoais",new EditarOrganizacaoView(utilizador, key)},
                    {"Membros",new EditarMembrosOrganizacaoView()},
                    {"Aptidoes",new AptidoesView()},
                };
            SelecionarViewCommand.Execute("Dados Pessoais");
        }
    }
}