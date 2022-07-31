
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace AppAptO.Models.FBData.Utilizadores
{
    public class UtilizadorOrganizacao : Utilizador
    {
        public string LinkWebsite { get; set; } = "";
        public ObservableCollection<string> KeysUtilizadoresParticulares { get; set; } = new ObservableCollection<string>();
        public string CodEntrada { get; set; }
        //public string UIDUtilizadorRepresentante { get; set; }

        public UtilizadorOrganizacao()
        {
            IsOrganizacao = true;
            DefinirCodEntrada();
        }
        public UtilizadorOrganizacao(Utilizador utilizador, string linkWebsite = "", string codEntrada = "", ObservableCollection<string> keysUtilizadoresParticulares = null)
        {
            foreach (PropertyInfo property in utilizador.GetType().GetProperties())
            {
                property.SetValue(this, property.GetValue(utilizador));
            }
            LinkWebsite = linkWebsite;
            KeysUtilizadoresParticulares = keysUtilizadoresParticulares ?? this.KeysUtilizadoresParticulares;
            CodEntrada = codEntrada;
            IsOrganizacao = true;
            if (string.IsNullOrEmpty(codEntrada))
                DefinirCodEntrada();
        }
        /// <summary>
        /// Funde os valores da propriedade do utilizador de tipo geral com o de tipo organização num único
        /// </summary>
        /// <param name="utilizador"></param>
        /// <param name="utilizadorOrganizacao"></param>
        public UtilizadorOrganizacao(Utilizador utilizador, UtilizadorOrganizacao utilizadorOrganizacao)
        {
            var utilizadorProperties = utilizador.GetType().GetProperties();
            foreach (PropertyInfo property in utilizadorProperties)
            {
                property.SetValue(this, property.GetValue(utilizador));
            }
            var partProperties = utilizadorOrganizacao.GetType().GetProperties().ToList();
            foreach (PropertyInfo property in utilizadorProperties)
            {
                partProperties.Remove(partProperties.First(p => p.Name == property.Name));
            }
            foreach (PropertyInfo property in partProperties)
            {
                property.SetValue(this, property.GetValue(utilizadorOrganizacao));
            }
            if (string.IsNullOrEmpty(this.CodEntrada))
                DefinirCodEntrada();
        }
        public void DefinirCodEntrada()
        {
            string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            Random random = new Random();
            CodEntrada = new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
