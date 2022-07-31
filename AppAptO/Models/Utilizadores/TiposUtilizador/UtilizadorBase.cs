using System.Collections.ObjectModel;
using Xamarin.CommunityToolkit.ObjectModel;

namespace AppAptO.Models.Utilizadores.TiposUtilizador
{
    public class UtilizadorBase : ObservableObject
    {
        public string NomeExibicao { get; set; } = "";
        public string Email { get; set; } = "";
        public string Uid { get; set; } = "";
        public string FotoUrl { get; set; } = "";
        public string Telefone { get; set; } = "";
        public string Sobre { get; set; } = "";
        public ObservableCollection<string> ChatsKeys { get; set; } = new ObservableCollection<string>();
    }
}
