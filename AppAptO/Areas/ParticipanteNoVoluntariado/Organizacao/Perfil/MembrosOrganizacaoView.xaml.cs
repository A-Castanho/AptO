using AppAptO.Models.FBData.Utilizadores;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Organizacao.Perfil
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MembrosOrganizacaoView : ContentView
    {
        private readonly MembrosOrganizacaoViewModel viewModel;
        public MembrosOrganizacaoView(UtilizadorOrganizacao utilizador)
        {
            InitializeComponent();
            viewModel = new MembrosOrganizacaoViewModel(utilizador);
            BindingContext = viewModel;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.CommandGoToPerfil.Execute(sender);
        }
    }
}