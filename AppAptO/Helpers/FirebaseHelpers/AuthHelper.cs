using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AppAptO.Models.FBConnections
{
    public static class AuthHelper
    {
        public static FirebaseObject<Utilizador> UtilizadorAtual { get; private set; }
        private static readonly FirebaseConfig firebaseConfig = new FirebaseConfig("AIzaSyCHkWBEAi3QWr9AQVkolCsar9VVgEryUpw");
        private static readonly FirebaseAuthProvider authProvider = new FirebaseAuthProvider(firebaseConfig);

        public static async Task<bool> ReiniciarUtilizador()
        {
            try
            {
                UtilizadorAtual = await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(UtilizadorAtual.Object.Uid);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Checks if the email is already being used by an account
        /// </summary>
        /// <returns>Returns true if it is being used and false if not</returns>
        public static async Task<bool> IsEmailUsed(string email)
        {
            var usedEmails = (await FBDataBase.UtilizadorDS.GetAllAsync<Utilizador>()).Select(user => user.Object.Email);
            return usedEmails.Contains(email);
        }
        public static async Task<bool> EnviarRecuperacaoPassword(string email)
        {
            try
            {
                await authProvider.SendPasswordResetEmailAsync(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task Registar(UtilizadorOrganizacao utilizador, string password, Stream imagemStream)
        {
            FirebaseAuthLink auth = await authProvider.CreateUserWithEmailAndPasswordAsync(utilizador.Email, password, "", false);
            utilizador.FotoUrl = imagemStream != null ? await StorageHelper.AdicionarStreamImagemPerfilAsync(imagemStream, auth.User.LocalId) : StorageHelper.UrlImgUtilizadorIndefinido;
            utilizador.Uid = auth.User.LocalId;
            await FBDataBase.UtilizadorDS.Add(utilizador);
        }
        public static async Task Registar(UtilizadorParticular utilizador, string password, Stream imagemStream)
        {
            FirebaseAuthLink auth = await authProvider.CreateUserWithEmailAndPasswordAsync(utilizador.Email, password, "", false);
            utilizador.FotoUrl = imagemStream != null ? await StorageHelper.AdicionarStreamImagemPerfilAsync(imagemStream, auth.User.LocalId) : StorageHelper.UrlImgUtilizadorIndefinido;
            utilizador.Uid = auth.User.LocalId;
            await FBDataBase.UtilizadorDS.Add(utilizador);
        }
        public static async Task<string> IniciarSessao(string email, string password)
        {
            try
            {
                FirebaseAuthLink auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);

                //Guardar o conteudo do login nas preferências para depois ser recuperado
                FirebaseAuthLink content = await auth.GetFreshAuthAsync();
                string serializedContent = JsonConvert.SerializeObject(content);
                Preferences.Set("FirebaseRefreshToken", serializedContent);
                await DefinirUtilizador(auth.User.LocalId);
                return UtilizadorAtual.Key;
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n" +
                       "Error data: \n" + e.Data +
                       "\nMessage: \n" + e.Message +
                       "\nStackTrace: \n" + e.StackTrace +
                       "\nSource: \n" + e.Source +
                       "\nInnerException: \n" + e.InnerException +
                       "\nTargetSite: \n" + e.TargetSite
                       );

                return null;
            }
        }
        public static void SairSessao()
        {
            Preferences.Remove("FirebaseRefreshToken");
            UtilizadorAtual = null;
        }
        public static async Task<bool> RecuperarInformacoesIniciadas()
        {
            try
            {
                if (!string.IsNullOrEmpty(Preferences.Get("FirebaseRefreshToken", "")))
                {
                    FirebaseAuthProvider authProvider = new FirebaseAuthProvider(firebaseConfig);
                    //recuperar o conteúdo iniciado anteriormente
                    FirebaseAuth savedAuth = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
                    FirebaseAuthLink refreshedContent = await authProvider.RefreshAuthAsync(savedAuth);
                    Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                    await DefinirUtilizador(refreshedContent.User.LocalId);
                    return true;
                }
                { return false; }
            }
            catch { return false; }
        }

        private static async Task DefinirUtilizador(string uid)
        {
            var a = await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(uid);
            UtilizadorAtual = await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(uid);
            if (UtilizadorAtual != null)
            {
                var disposable = FBDataBase.UtilizadorDS.DatabasePath
                                .AsObservable<Utilizador>()
                                .Subscribe(async (dbevent) =>
                                {
                                    if (dbevent.Object != null)
                                    {
                                        if (dbevent.Key == UtilizadorAtual.Key)
                                            UtilizadorAtual = (await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(UtilizadorAtual.Object.Uid));
                                    }
                                });
            }
        }
    }
}
