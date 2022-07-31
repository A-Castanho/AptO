using System.Collections.ObjectModel;

namespace AppAptO.Models.FBData.Apoios
{
    public class Tarefa
    {
        public string Titulo { get; set; } = "Nova Tarefa";
        public string Descricao { get; set; }
        public ObservableCollection<string> KeysVoluntariosEnvolvidos { get; set; } = new ObservableCollection<string>();
        //public ObservableCollection<NivelExecucaoTarefa> NiveisExecucao { get; set; } = new ObservableCollection<NivelExecucaoTarefa>();
        public bool Estado { get; set; } = false;
    }
}
