using System.Collections.Generic;

namespace AppAptO.Models.TiposDados
{
    public class Aptidao
    {
        public List<string> AreasEnglobadas { get; set; }
        public string NomeGeral { get; set; }

        public Aptidao(string nomeGeral, List<string> areasEnglobadas = null)
        {
            AreasEnglobadas = areasEnglobadas ?? new List<string>();
            NomeGeral = nomeGeral;
        }
    }
}
