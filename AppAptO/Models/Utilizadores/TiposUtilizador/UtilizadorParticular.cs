using AppAptO.Models.FBData.DadosUtiliizadores;
using System.Linq;
using System.Reflection;

namespace AppAptO.Models.FBData.Utilizadores
{
    public class UtilizadorParticular : Utilizador
    {
        private string primeiroNome;
        public string PrimeiroNome
        {
            get => primeiroNome;
            set { primeiroNome = value; NomeExibicao = PrimeiroNome + " " + UltimoNome; }
        }
        private string ultimoNome;
        private Disponibilidade disponibilidade = new Disponibilidade();
        public string UltimoNome
        {
            get => ultimoNome;
            set { ultimoNome = value; NomeExibicao = PrimeiroNome + " " + UltimoNome; }
        }
        public string UidGrupoPertencente { get; set; }
        public string Telemovel { get; set; }

        public Disponibilidade Disponibilidade { get => disponibilidade; set => SetProperty(ref disponibilidade, value); }
        public UtilizadorParticular()
        {
        }

        /// <summary>
        /// Funde os valores da propriedade do utilizador de tipo geral com o de tipo particular num único
        /// </summary>
        /// <param name="utilizador"></param>
        /// <param name="utilizadorParticular"></param>
        public UtilizadorParticular(Utilizador utilizador, UtilizadorParticular utilizadorParticular)
        {
            var utilizadorProperties = utilizador.GetType().GetProperties();
            foreach (PropertyInfo property in utilizadorProperties)
            {
                property.SetValue(this, property.GetValue(utilizador));
            }
            var partProperties = utilizadorParticular.GetType().GetProperties().ToList();
            foreach (PropertyInfo property in utilizadorProperties)
            {
                partProperties.Remove(partProperties.First(p => p.Name == property.Name));
            }
            foreach (PropertyInfo property in partProperties)
            {
                property.SetValue(this, property.GetValue(utilizadorParticular));
            }
        }

        public UtilizadorParticular(Utilizador utilizador,
            string primeiroNome, string ultimoNome,
            string uidGrupoPertencente = "", string telemovel = "")
        {
            foreach (PropertyInfo property in utilizador.GetType().GetProperties())
            {
                property.SetValue(this, property.GetValue(utilizador));
            }

            Telemovel = telemovel;
            UidGrupoPertencente = uidGrupoPertencente;
            PrimeiroNome = primeiroNome;
            UltimoNome = ultimoNome;
        }
    }
}
