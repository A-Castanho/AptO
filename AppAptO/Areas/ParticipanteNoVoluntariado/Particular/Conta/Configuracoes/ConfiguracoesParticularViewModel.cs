using AppAptO.Areas.ParticipanteNoVoluntariado.Particular.Conta.Configuracoes;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.ViewsPartial.Conta.Configuracoes;
using Firebase.Database;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta
{
    public class ConfiguracoesParticularViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
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
        private FirebaseObject<UtilizadorParticular> utilizador;
        public FirebaseObject<UtilizadorParticular> Utilizador
        {
            get => utilizador;
            set
            {
                utilizador = value;
                OnPropertyChanged();
            }
        }
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
        public ConfiguracoesParticularViewModel(FirebaseObject<UtilizadorParticular> fbutilizador)
        {
            Utilizador = fbutilizador;
            Iniciar(fbutilizador);
        }

        public ConfiguracoesParticularViewModel()
        {
        }
        private void Iniciar(FirebaseObject<UtilizadorParticular> fbutilizador)
        {
            DictionaryViews = new Dictionary<string, ContentView>
                {
                    {"Dados Pessoais", new EditarParticularView(fbutilizador)},
                    {"Disponibilidade", new EditarDisponibilidadeView(fbutilizador)},
                    {"Aptidoes", new AptidoesView()},
                };
            SelecionarViewCommand.Execute("Dados Pessoais");
        }

    }
}
