
using System;
using System.Collections.ObjectModel;

namespace AppAptO.Models.FBData.Apoios
{
    public class PedidoApoio
    {
        public string Titulo { get; set; } = "";
        public bool VisPublicacao { get; set; } = true;
        public string Localidade { get; set; }
        public string Descricao { get; set; }
        public string Area { get; set; }
        public string ChatKey { get; set; }
        public string UidApoiado { get; set; }
        public DateTime DiaPublicacao { get; set; }
        public ObservableCollection<Tarefa> Tarefas { get; set; } = new ObservableCollection<Tarefa>();
        public ObservableCollection<string> KeysUtilizadoresDisponiveis { get; set; } = new ObservableCollection<string>();
    }
}
