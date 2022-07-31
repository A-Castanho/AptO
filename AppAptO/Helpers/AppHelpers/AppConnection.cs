using AppAptO.PopUps;
using AppAptO.Views.Erro;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppAptO.Models.AppHelpers
{
    public static class AppConnection
    {
        /// <summary>
        /// Se conectado à internet devolve true, senão devolve um aviso e false
        /// (se a página atual for nula, define-a como a página de erro)
        /// </summary>
        public static bool IsConnected => CheckConnectivity();
        private static bool CheckConnectivity()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Application.Current.MainPage = Application.Current.MainPage ?? new SemInternetPage();
                Application.Current.MainPage.ShowPopup(new MensagemPopUp("Erro", "Não está conectado à internet.\nVerifique a conexão e tente outra vez.", "Cancelar"));
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
