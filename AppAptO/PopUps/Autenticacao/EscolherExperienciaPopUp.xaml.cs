using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace AppAptO.PopUps.Autenticacao
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EscolherExperienciaPopUp : Popup<string>
    {
        public EscolherExperienciaPopUp()
        {
            InitializeComponent();
            BindingContext = new AptidoesViewModel();
        }
    }
}