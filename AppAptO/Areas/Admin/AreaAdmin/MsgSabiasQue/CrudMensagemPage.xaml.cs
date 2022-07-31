using AppAptO.Models.DadosAplicacao;
using Firebase.Database;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.Admin.AreaAdmin.MsgSabiasQue
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrudMensagemPage : ContentPage
    {

        public CrudMensagemPage()
        {
            InitializeComponent();
        }

        public CrudMensagemPage(FirebaseObject<MensagemInicial> mensagemSelecionada)
        {
            InitializeComponent();
            BindingContext = new CrudMensagemViewModel(mensagemSelecionada);
        }
    }
}