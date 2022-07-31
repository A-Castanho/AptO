using Firebase.Storage;
using System.IO;
using System.Threading.Tasks;

namespace AppAptO.Models.FBConnections
{
    public static class StorageHelper
    {
        #region variaveis
        private static readonly FirebaseStorage Storage = new FirebaseStorage("apto-29930.appspot.com");
        private static string dadosAplicacao = "Aplicação";
        private static string childPublicidades = "Publicidade";
        private static string childChats = "Chats";
        private static string childImagensChat = "Imagens Chat";

        public static string UrlImgUtilizadorIndefinido { get; private set; }

        private const string childUtilizadores = "Utilizadores";
        private const string childFotosPerfil = "ImagensPerfil";
        #endregion
        public static async Task IniciarAsync()
        {
            UrlImgUtilizadorIndefinido = await Storage.Child(childUtilizadores)
                .Child(childFotosPerfil)
                .Child("ImagemPredefinida.png")
                .GetDownloadUrlAsync();
        }

        public static async Task<string> AdicionarStreamImagemPublicidadeAsync(Stream stream, string keyPublicidade)
        {
            return await Storage.Child(dadosAplicacao)
                .Child(childPublicidades)
                .Child(keyPublicidade)
                .PutAsync(stream);
        }
        public static async Task EliminarStreamPublicidadeAsync(string keyPublicidade)
        {
            await Storage.Child(dadosAplicacao)
                .Child(childPublicidades)
                .Child(keyPublicidade)
                .DeleteAsync();
        }
        public static async Task<string> AtualizarStreamPublicidadeAsync(Stream stream, string keyPublicidade)
        {
            return await Storage.Child(dadosAplicacao)
                .Child(childPublicidades)
                .Child(keyPublicidade)
                .PutAsync(stream);
        }
        /// <summary>
        /// Adiciona a stream ao storage bucket e devolve o url do resultado
        /// </summary>
        public static async Task<string> AdicionarStreamImagemPerfilAsync(Stream stream, string uidUtilizador)
        {
            return await Storage.Child(childUtilizadores)
                .Child(childFotosPerfil)
                .Child(uidUtilizador)
                .PutAsync(stream);
        }
        public static async Task EliminarStreamImagemPerfilAsync(string uidUtilizador)
        {
            await Storage.Child(childUtilizadores)
                .Child(childFotosPerfil)
                .Child(uidUtilizador)
                .DeleteAsync();
        }
        public static async Task<string> AtualizarStreamImagemPerfilAsync(Stream stream, string uidUtilizador)
        {
            return await Storage.Child(childUtilizadores)
                .Child(childFotosPerfil)
                .Child(uidUtilizador)
                .PutAsync(stream);
        }

        public async static Task<string> AtualizarStreamImagemChatAsync(Stream stream, string chatKey)
        {
            return await Storage.Child(childChats)
                .Child(childImagensChat)
                .Child(chatKey)
                .PutAsync(stream);
        }
    }
}
