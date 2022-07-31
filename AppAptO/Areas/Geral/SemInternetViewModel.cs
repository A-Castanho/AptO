using AppAptO.Models.AppHelpers;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Erro
{
    public class SemInternetViewModel
    {
        public ICommand ComandoTestarConexao => new Command(TestarConexao);
        private async void TestarConexao()
        {
            if (AppConnection.IsConnected)
            {
                await App.Iniciar();
            }
        }
    }
}
