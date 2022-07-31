using AppAptO.Models.FBConnections;
using System;

namespace AppAptO.Models.Chats
{
    public class Mensagem
    {
        public string Texto { get; set; }
        public DateTime DateTime { get; set; }
        public string KeyEmissor { get; set; }
        public Mensagem(string texto, string keyEmissor)
        {
            Texto = texto;
            KeyEmissor = keyEmissor;
            DateTime = DateTime.Now;
        }
        public Mensagem(string texto)
        {
            Texto = texto;
            KeyEmissor = AuthHelper.UtilizadorAtual.Key;
            DateTime = DateTime.Now;
        }

        public Mensagem()
        {
            KeyEmissor = AuthHelper.UtilizadorAtual.Key;
            DateTime = DateTime.Now;
        }
    }
}
