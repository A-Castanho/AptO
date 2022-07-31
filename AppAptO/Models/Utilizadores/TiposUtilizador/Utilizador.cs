
using AppAptO.Models.FBData.Apoios.Notificacoes;
using AppAptO.Models.Utilizadores.TiposUtilizador;
using System.Collections.Generic;

namespace AppAptO.Models.FBData.Utilizadores
{
    public class Utilizador : UtilizadorBase
    {
        public string CodPostal { get; set; } = "";
        public string Morada { get; set; } = "";
        public bool IsApoiado { get; set; }
        public bool IsVoluntario { get; set; }
        public bool IsOrganizacao { get; set; }
        public string Localidade { get; set; } = "";
        public List<Convite> Convites { get; set; } = new List<Convite>();
        public List<Aviso> Avisos { get; set; } = new List<Aviso>();
        //1º string = código da areaexperiencia, list = lista de indexs da experiencia correspondentes
        public Dictionary<string, List<int>> Aptidoes { get; set; } = new Dictionary<string, List<int>>();
    }
}
