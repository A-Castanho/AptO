using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using Firebase.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta.MeuVoluntariado
{
    public class MinhaAcaoTarefaViewModel : ObservableObject
    {
        private int TarefaIndex { get; }
        private string KeyAcao { get; }
        public Tarefa tarefa;
        public Tarefa Tarefa
        {
            get => tarefa;
            private set
            {
                SetProperty(ref tarefa, value);
            }
        }

        private string nomeEstado = "Incompleta";
        public string NomeEstado
        {
            get { return nomeEstado; }
            set { SetProperty(ref nomeEstado, value); }
        }
        public FirebaseObject<Utilizador> ParticipanteSelecionado { get; set; }
        private ObservableCollection<FirebaseObject<Utilizador>> listaEnvolvidos;
        public ObservableCollection<FirebaseObject<Utilizador>> ListaEnvolvidos
        {
            get { return listaEnvolvidos; }
            private set { SetProperty(ref listaEnvolvidos, value); }
        }
        public ICommand CommandGoToPerfilParticipante => new Command(
          async () =>
          {
              try
              {
                  await Shell.Current.Navigation.PushAsync(new Views.Conta.PerfilPage(ParticipanteSelecionado));
              }
              catch { }
          });

        public MinhaAcaoTarefaViewModel()
        {
        }

        public MinhaAcaoTarefaViewModel(Tarefa tarefa, int tarefaIndex, string keyAcao)
        {
            Tarefa = tarefa;
            TarefaIndex = tarefaIndex;
            KeyAcao = keyAcao;
            IniciarLista();
            NomeEstado = (tarefa.Estado) ? "Completa" : "Incompleta";
        }

        public ICommand CommandMudarEstado => new Command(
        async () =>
        {
            NomeEstado = (Tarefa.Estado) ? "Completa" : "Incompleta";
            var pedido = await FBDataBase.PedidosDS.GetByKeyAsync(KeyAcao);
            pedido.Tarefas[TarefaIndex].Estado = !pedido.Tarefas[TarefaIndex].Estado;
            await FBDataBase.PedidosDS.Update(pedido, KeyAcao);
        });
        private async void IniciarLista()
        {
            ListaEnvolvidos = new ObservableCollection<FirebaseObject<Utilizador>>((await FBDataBase.UtilizadorDS.GetAllAsync<Utilizador>())
                   .Where(utilizador => Tarefa.KeysVoluntariosEnvolvidos.Contains(utilizador.Key)));
        }
    }
}
