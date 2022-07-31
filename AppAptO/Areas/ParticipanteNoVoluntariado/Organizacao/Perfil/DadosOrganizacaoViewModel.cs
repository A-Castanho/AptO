using AppAptO.Models.FBData.Utilizadores;
using Xamarin.CommunityToolkit.ObjectModel;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Organizacao.Perfil
{
    public class DadosOrganizacaoViewModel : ObservableObject
    {
        private UtilizadorOrganizacao utilizador = new UtilizadorOrganizacao();
        public UtilizadorOrganizacao Utilizador
        {
            get => utilizador;
            set => SetProperty(ref utilizador, value);
        }
        public DadosOrganizacaoViewModel(UtilizadorOrganizacao utilizador)
        {
            Utilizador = utilizador;
        }
        public DadosOrganizacaoViewModel()
        {
        }
    }
}
