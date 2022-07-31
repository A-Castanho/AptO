using System;

namespace AppAptO.Models.FBData.Apoios.Notificacoes
{
    public class Notificacao
    {
        /// <summary>
        /// Chave do utilizador que recebe o convite
        /// </summary>
        public string RecetorKey { get; set; }
        /// <summary>
        /// Mensagem a ser usada na listagem de notificaçõs
        /// </summary>
        public string Mensagem { get; set; }
        /// <summary>
        /// Chave do utilizador que recebe o convite
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}
