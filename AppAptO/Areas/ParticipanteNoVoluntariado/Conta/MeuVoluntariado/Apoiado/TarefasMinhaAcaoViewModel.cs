using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Services.Firebase;
using AppAptO.Views.Conta.MeuVoluntariado;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta.MeuVoluntariado
{
    public class TarefasMinhaAcaoViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        private bool conteudoIniciado = true;
        public bool ConteudoIniciado
        {
            get => conteudoIniciado;
            set => SetProperty(ref conteudoIniciado, value);
        }
        private PedidoApoio Acao { get; set; }
        private string KeyAcao { get; }
        private string KeyOrganizacao { get; }
        public Tarefa TarefaSelecionada { get; set; }
        private ObservableCollection<Tarefa> minhasTarefas;
        public ObservableCollection<Tarefa> MinhasTarefas
        {
            get => minhasTarefas;
            private set
            {
                SetProperty(ref minhasTarefas, value);
            }
        }
        public ICommand CommandReiniciar => new Command(
        async () =>
        {
            await Reiniciar();
        });

        private async Task Reiniciar()
        {
            ConteudoIniciado = false;
            Acao = await FBDataBase.PedidosDS.GetByKeyAsync(KeyAcao);
            DefinirTarefas();
            ConteudoIniciado = true;
        }

        public ICommand CommandGoToTarefa => new Command(
            async () => await Shell.Current.Navigation.PushAsync(new MinhaAcaoTarefaPage(TarefaSelecionada, MinhasTarefas.IndexOf(TarefaSelecionada), KeyAcao)));
        public TarefasMinhaAcaoViewModel() { }

        public TarefasMinhaAcaoViewModel(PedidoApoio acao, string keyAcao, string keyOrganizacao = "")
        {
            Acao = acao;
            KeyOrganizacao = keyOrganizacao;
            DefinirTarefas();
            KeyAcao = keyAcao;
        }

        private void DefinirTarefas()
        {
            MinhasTarefas = new ObservableCollection<Tarefa>
                            (Acao.Tarefas.Where(t => t.KeysVoluntariosEnvolvidos.Contains((KeyOrganizacao != "") ? AuthHelper.UtilizadorAtual.Key : KeyOrganizacao)));
        }
    }
}
