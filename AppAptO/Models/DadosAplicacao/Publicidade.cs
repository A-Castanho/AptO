using System;

namespace AppAptO.Models.DadosAplicacao
{
    public class Publicidade
    {
        public enum TipoPublicidade { Horizontal, Vertical }
        public string Nome { get; set; } = "";
        public string Redirecionamento { get; set; } = "";
        public string Empresa { get; set; } = "";
        public DateTime DataAdicao { get; set; }
        public int NivelPrioridade { get; set; } = 1;
        public string PathImagem { get; set; } = "";
        public TipoPublicidade Tipo { get; set; }
    }
}
