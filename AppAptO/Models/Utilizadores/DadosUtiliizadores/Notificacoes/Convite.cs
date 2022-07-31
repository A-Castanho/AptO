using AppAptO.Models.FBConnections;
using System;

namespace AppAptO.Models.FBData.Apoios.Notificacoes
{
    //Notificação
    public class Convite : Notificacao
    {
        public enum Tipo { OfertaApoio, PedidoApoio, ConvitePorPerfil };
        /// <summary>
        /// Chave da oferta/pedido de apoio para a qual o emissor enviou o convite
        /// </summary>
        public string ApoioKey { get; set; }
        /// <summary>
        /// Chave do utilizador que cria o convite
        /// </summary>
        public string EmissorKey { get; set; }
        public Tipo TipoConvite { get; set; }

        /// <summary>
        /// Convite de participação numa atividade de voluntariado recebida pelos utilizadores
        /// </summary>
        /// <param name="apoioKey">key do apoio na base de dados</param>
        /// <param name="tipoConvite">Tipo a que foi respondido (OfertaApoio/PedidoApoio)</param>
        /// <param name="recetorKey">Key do utilizador que irá receber o convite</param>
        /// <param name="emissorKey">Defaults to AuthHelper.UtilizadorAtual.Key</param>
        public Convite(string apoioKey, Tipo tipoConvite, string recetorKey, string emissorKey = null)
        {
            ApoioKey = apoioKey;
            EmissorKey = emissorKey ?? AuthHelper.UtilizadorAtual.Key;
            TipoConvite = tipoConvite;
            RecetorKey = recetorKey;
            DateTime = DateTime.Now;
            if (tipoConvite == Tipo.PedidoApoio)
            {
                Mensagem = "Um utilizador quer agir quanto ao seu pedido!";
            }
            else
            {
                Mensagem = "Um utilizador quer o seu apoio num projeto!";
            }
        }
    }
}
