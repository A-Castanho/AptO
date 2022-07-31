using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.PopUps.Crud;
using AppAptO.PopUps.Select;
using AppAptO.Services.Firebase;
using Firebase.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta.GestaoVoluntariado
{
    public class GestaoTarefasViewModel : ObservableObject
    {
        private bool isModoEdicao = true;
        public bool IsModoEdicao
        {
            get => isModoEdicao;
            set => SetProperty(ref isModoEdicao, value);
        }
        private PedidoApoio Pedido { get; }
        private string PedidoKey { get; }
        #region RegiãoTarefas
        public Tarefa ListTarefaEscolhida
        {
            get => listTarefaEscolhida; set
            {
                SetProperty(ref listTarefaEscolhida, value);
            }
        }
        private int tarefaIndex;
        private Tarefa tarefaSelecionada;
        public Tarefa TarefaSelecionada
        {
            get => tarefaSelecionada;
            set
            {
                SetProperty(ref tarefaSelecionada, value);
                tarefaIndex = ListaTarefas.IndexOf(value);
                VisibilidadeListaTarefas = value == null;
                DefinirListaUtilizadores();
            }
        }
        private bool visibilidadeListaTarefas = true;
        public bool VisibilidadeListaTarefas
        {
            get { return visibilidadeListaTarefas; }
            set { SetProperty(ref visibilidadeListaTarefas, value); }
        }
        private string nomeEstado = "Incompleta";
        public string NomeEstado
        {
            get { return nomeEstado; }
            set { SetProperty(ref nomeEstado, value); }
        }
        private ObservableCollection<FirebaseObject<Utilizador>> UtilizadoresInscritos { get; set; } = new ObservableCollection<FirebaseObject<Utilizador>>();
        private ObservableCollection<FirebaseObject<Utilizador>> UtilizadoresDisponiveis { get; set; } = new ObservableCollection<FirebaseObject<Utilizador>>();
        private ObservableCollection<Tarefa> listaTarefas = new ObservableCollection<Tarefa>();
        public ObservableCollection<Tarefa> ListaTarefas
        {
            get { return listaTarefas; }
            set { SetProperty(ref listaTarefas, value); }
        }
        #endregion
        #region Região Utilizadores
        public FirebaseObject<Utilizador> UtilizadorSelecionado { get; set; }
        private ObservableCollection<FirebaseObject<Utilizador>> listaUtilizadores = new ObservableCollection<FirebaseObject<Utilizador>>();
        private Tarefa listTarefaEscolhida;

        public ObservableCollection<FirebaseObject<Utilizador>> ListaUtilizadores
        {
            get { return listaUtilizadores; }
            set { SetProperty(ref listaUtilizadores, value); }
        }
        #endregion
        public ICommand CommandRemoverVoluntario => new Command(
        () =>
        {
            try
            {
                TarefaSelecionada.KeysVoluntariosEnvolvidos.Remove(UtilizadorSelecionado.Key);
                ListaUtilizadores.Remove(UtilizadorSelecionado);
                ListaTarefas.First(t => t == TarefaSelecionada).KeysVoluntariosEnvolvidos.Remove(UtilizadorSelecionado.Key);
            }
            catch { }
        });

        public ICommand CommandAdicionarVoluntario => new Command(
        async () =>
        {
            try
            {
                var keyUtilizador = (await Application.Current.MainPage.ShowPopupAsync<string>(new SelectUtilizadorPopUp(UtilizadoresInscritos)));
                if (ListaUtilizadores.FirstOrDefault(utilizador => utilizador.Key == keyUtilizador) == null)
                    ListaUtilizadores.Add(await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>((await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(keyUtilizador)).Uid));
            }
            catch { }
        });
        public ICommand CommandAdicionarTarefa => new Command(
        async () =>
        {
            try
            {
                Tarefa tarefa = await Application.Current.MainPage.ShowPopupAsync<Tarefa>(new CrudTarefaPopUp(new Tarefa()));
                if (!ListaTarefas.Select(t => t.Titulo).Contains(tarefa.Titulo))
                {
                    if (tarefa != null)
                        ListaTarefas.Add(tarefa);
                    TarefaSelecionada = ListaTarefas.Last();
                }
                else
                    await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Tarefa inválida!", "Já existe uma tarefa com o título determinado", "Ok"));
            }
            catch { }
        });
        public ICommand CommandEliminarTarefa => new Command(
        () =>
        {
            try
            {
                ListaTarefas.Remove(TarefaSelecionada);
                if (ListaTarefas.Count > 0)
                    TarefaSelecionada = ListaTarefas[0];
                else
                    TarefaSelecionada = null;
            }
            catch { }
        });
        public ICommand CommandMudarEstado => new Command(
        () =>
        {
            NomeEstado = (TarefaSelecionada.Estado) ? "Completa" : "Incompleta";
        });
        public ICommand CommandVerTarefas => new Command(
        () =>
        {
            VisibilidadeListaTarefas = !VisibilidadeListaTarefas;
        });
        public ICommand CommandSubmeter => new Command(
        async () =>
        {
            ListaTarefas.FirstOrDefault(tarefa => tarefa == TarefaSelecionada).KeysVoluntariosEnvolvidos = new ObservableCollection<string>(ListaUtilizadores.Select(utilizador => utilizador.Key));
            Pedido.Tarefas = ListaTarefas;
            await FBDataBase.PedidosDS.Update(Pedido, PedidoKey);
            await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Sucesso!", "A tarefa foi submetida", "Ok"));
            ListaTarefas[tarefaIndex] = TarefaSelecionada;

        });
        public GestaoTarefasViewModel() { }
        public GestaoTarefasViewModel(PedidoApoio pedido, string pedidoKey, bool isModoEdicao)
        {
            IniciarUtilizadores();
            ListaTarefas = pedido.Tarefas;
            Pedido = pedido;
            //if (ListaTarefas.Count > 0)
            //{
            //    TarefaSelecionada = ListaTarefas[0];
            //    NomeEstado = (TarefaSelecionada.Estado) ? "Completa" : "Incompleta";
            //}
            PedidoKey = pedidoKey;
            IsModoEdicao = isModoEdicao;
        }
        private async void IniciarUtilizadores()
        {
            try
            {
                UtilizadoresInscritos = new ObservableCollection<FirebaseObject<Utilizador>>((await FBDataBase.UtilizadorDS.GetAllAsync<Utilizador>())
                                .Where(utilizador => Pedido.KeysUtilizadoresDisponiveis.Contains(utilizador.Key)));
            }
            catch
            {
                UtilizadoresInscritos = new ObservableCollection<FirebaseObject<Utilizador>>();
            }
        }
        private async void DefinirListaUtilizadores()
        {
            if (TarefaSelecionada != null)
            {
                ListaUtilizadores = new ObservableCollection<FirebaseObject<Utilizador>>((await FBDataBase.UtilizadorDS.GetAllAsync<Utilizador>())
                                .Where(utilizador => TarefaSelecionada.KeysVoluntariosEnvolvidos.Contains(utilizador.Key)));
                UtilizadoresDisponiveis = UtilizadoresInscritos;
                foreach (var item in ListaUtilizadores)
                {
                    UtilizadoresDisponiveis.Remove(item);
                }

            }
        }
    }
}
