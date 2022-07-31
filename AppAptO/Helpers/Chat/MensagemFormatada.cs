using AppAptO.Models.Chats;
using System;

namespace AppAptO.Helpers.Chat
{
    public class MensagemFormatada
    {
        public Mensagem Mensagem { get; set; }
        public string NomeUtilizador { get; set; }
        public string KeyUtilizador { get; set; }
        public DateTime DateTimeEnvio { get; set; }

        public MensagemFormatada(Mensagem mensagem, string nomeUtilizador, string keyUtilizador, DateTime dateTimeEnvio)
        {
            Mensagem = mensagem;
            NomeUtilizador = nomeUtilizador;
            KeyUtilizador = keyUtilizador;
            DateTimeEnvio = dateTimeEnvio;
        }
    }
}
