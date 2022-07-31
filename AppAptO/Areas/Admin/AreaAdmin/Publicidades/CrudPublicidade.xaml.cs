using AppAptO.Models.DadosAplicacao;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.Admin.AreaAdmin.Publicidades
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrudPublicidade : ContentPage
    {
        public CrudPublicidade()
        {
            InitializeComponent();

            BindingContext = new CrudPublicidadeViewModel();
        }
        public CrudPublicidade(Publicidade publicidade, string key)
        {
            InitializeComponent();
            BindingContext = new CrudPublicidadeViewModel(publicidade, key);
            picker.SelectedItem = publicidade.Tipo;
        }
    }
}