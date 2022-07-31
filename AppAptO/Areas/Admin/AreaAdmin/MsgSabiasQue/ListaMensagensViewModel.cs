using AppAptO.Models.DadosAplicacao;
using AppAptO.Services.Firebase;
using Firebase.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.Areas.Admin.AreaAdmin.MsgSabiasQue
{
    public class ListaMensagensViewModel : ObservableObject
    {
        private ObservableCollection<FirebaseObject<MensagemInicial>> mensagensOriginais;
        private ObservableCollection<FirebaseObject<MensagemInicial>> mensagens;
        private FirebaseObject<MensagemInicial> mensagemSelecionada;
        private string pesquisa;
        private bool isBusy = false;
        public ObservableCollection<FirebaseObject<MensagemInicial>> Mensagens { get => mensagens; set => SetProperty(ref mensagens, value); }
        public FirebaseObject<MensagemInicial> MensagensSelecionada { get => mensagemSelecionada; set => SetProperty(ref mensagemSelecionada, value); }
        public string Pesquisa
        {
            get => pesquisa; set
            {
                SetProperty(ref pesquisa, value);
                Pesquisar();
            }
        }
        public bool IsBusy { get => isBusy; set => SetProperty(ref isBusy, value); }

        public ICommand CommandGoToMensagem => new Command(
        async () => await Shell.AdminShell.Current.Navigation.PushAsync(new CrudMensagemPage(MensagensSelecionada)));

        public ICommand CommandGoToCreate => new Command(
        async () => await Shell.AdminShell.Current.Navigation.PushAsync(new CrudMensagemPage()));
        public ICommand CommandRefresh => new Command(IniciarLista);

        public ListaMensagensViewModel()
        {
            IniciarLista();
        }

        private async void IniciarLista()
        {
            IsBusy = true;
            mensagensOriginais = new ObservableCollection<FirebaseObject<MensagemInicial>>(await FBDataBase.MsgSabiasDS.GetAllAsync());
            Mensagens = mensagensOriginais;
            IsBusy = false;
        }
        private void Pesquisar()
        {
            IsBusy = true;
            try
            {
                string pesquisaTratada = Pesquisa.ToLower().Trim();
                Mensagens = new ObservableCollection<FirebaseObject<MensagemInicial>>(
                    mensagensOriginais.Where
                    (
                        p => p.Object.Texto.ToLower().Contains(pesquisaTratada)
                        || p.Object.Anotacao.ToLower().Contains(pesquisaTratada)
                    ));
            }
            catch { }
            IsBusy = false;
        }
    }
}
