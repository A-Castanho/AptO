using System;

namespace AppAptO.Models.FBData.Apoios.Notificacoes
{
    public class Aviso : Notificacao
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        /// <summary>
        /// Construtor do aviso
        /// </summary>
        /// <param name="titulo">Título a aparecer no pop-up</param>
        /// <param name="descricao">Descrição a aparecer no pop-up</param>
        /// <param name="mensagemPopUp">Mensagem a aparecer na lista de notificações</param>
        public Aviso(string titulo, string descricao, string mensagemPopUp)
        {
            Titulo = titulo;
            Descricao = descricao;
            DateTime = DateTime.Now;
            Mensagem = mensagemPopUp;
        }
    }
}
