using AppAptO.Models.DadosAplicacao;
using AppAptO.Services.Firebase;
using Firebase.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.Areas.Admin.AreaAdmin.Publicidades
{
    public class ListagemPublicidadesViewModel : ObservableObject
    {
        private ObservableCollection<FirebaseObject<Publicidade>> publicidadesOriginal;
        private ObservableCollection<FirebaseObject<Publicidade>> publicidades;

        public ObservableCollection<FirebaseObject<Publicidade>> Publicidades
        {
            get { return publicidades; }
            set { SetProperty(ref publicidades, value); }
        }
        private string pesquisa;
        public string Pesquisa
        {
            get { return pesquisa; }
            set { SetProperty(ref pesquisa, value); Pesquisar(); }
        }
        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        public ICommand CommandRefresh => new Command(
        () =>
        {
            DefinirPublicidades();
            Pesquisar();
        });
        private void Pesquisar()
        {
            IsBusy = true;
            try
            {
                string pesquisaTratada = Pesquisa.ToLower().Trim();
                Publicidades = new ObservableCollection<FirebaseObject<Publicidade>>(
                    publicidadesOriginal.Where
                    (
                        p => p.Object.Empresa.ToLower().Contains(pesquisaTratada)
                        || p.Object.Nome.ToLower().Contains(pesquisaTratada)
                    ));
            }
            catch { }
            IsBusy = false;
        }

        public ListagemPublicidadesViewModel()
        {
            DefinirPublicidades();
        }

        private async void DefinirPublicidades()
        {
            IsBusy = true;
            Publicidades = new ObservableCollection<FirebaseObject<Publicidade>>(
                await FBDataBase.PublicidadeDS.GetAllAsync());
            Publicidades.OrderBy(p => p.Object.DataAdicao);
            publicidadesOriginal = Publicidades;
            IsBusy = false;
        }
    }
}
