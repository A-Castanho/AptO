namespace AppAptO.Models.FBData.TiposDados
{
    public class Localidade
    {
        public string Nome { get; set; }
        public int NOfertas { get; set; }
        public int NPedidos { get; set; }

        public Localidade(string nome, int nOfertas = 0, int nPedidos = 0)
        {
            Nome = nome;
            NOfertas = nOfertas;
            NPedidos = nPedidos;
        }
    }
}
