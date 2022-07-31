using AppAptO.Models.FBData.Apoios;

namespace AppAptO.ViewModels.PopUps
{
    public class CrudTarefaViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        private Tarefa tarefa;
        public Tarefa Tarefa
        {
            get
            {
                return tarefa;
            }
            set
            {
                SetProperty(ref tarefa, value);
            }
        }
        private NivelExecucaoTarefa nivelExecucaoSelecionado;
        public NivelExecucaoTarefa NivelExecucaoSelecionado
        {
            get
            {
                return nivelExecucaoSelecionado;
            }
            set
            {
                SetProperty(ref nivelExecucaoSelecionado, value);
            }
        }
        //public ICommand CommandAdicionarNivel => new Command(
        //() =>
        //{
        //    Tarefa.NiveisExecucao.Add(new NivelExecucaoTarefa());
        //});

        public CrudTarefaViewModel()
        {
            Tarefa = new Tarefa();
        }

        public CrudTarefaViewModel(Tarefa tarefa)
        {
            Tarefa = tarefa;
        }
    }
}
