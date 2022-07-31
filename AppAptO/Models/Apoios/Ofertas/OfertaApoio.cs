using System;

namespace AppAptO.Models.FBData.Apoios
{
    public class OfertaApoio
    {
        public string Titulo { get; set; } = "";
        public string Localidade { get; set; }
        public string Descricao { get; set; }
        public string Area { get; set; }
        public string UidApoiante { get; set; }
        public DateTime DiaPublicacao { get; set; }
    }
}
