using AppAptO.Models.FBData.Utilizadores;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Organizacao.Perfil
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DadosOrganizacaoView : ContentView
    {
        public DadosOrganizacaoView(UtilizadorOrganizacao utilizador)
        {
            InitializeComponent();
            BindingContext = new DadosOrganizacaoViewModel(utilizador);
        }
    }
}