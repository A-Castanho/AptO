using AppAptO.Areas.Admin.Apoios.Ofertas;
using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.Services.Firebase;
using AppAptO.Views.Ofertas;
using Firebase.Database;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta
{
    public class OfertasPessoaisViewModel : ObservableObject
    {
        private FirebaseObject<OfertaApoio> ofertaSelecionada;
        public FirebaseObject<OfertaApoio> OfertaSelecionada
        {
            get => ofertaSelecionada;
            set
            {
                SetProperty(ref ofertaSelecionada, value);
            }
        }

        public ICommand OfertaSelecionadaCommand => new AsyncCommand(
        async () =>
        {
            if (AuthHelper.UtilizadorAtual.Key != "Admin")
                await Shell.Current.Navigation.PushAsync(new OfertaApoioPage(OfertaSelecionada.Object, OfertaSelecionada.Key));
            else
                await Shell.Current.Navigation.PushAsync(new AdminOfertaApoioPage(OfertaSelecionada));
        });

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private ObservableCollection<FirebaseObject<OfertaApoio>> lista = new ObservableCollection<FirebaseObject<OfertaApoio>>();
        public ObservableCollection<FirebaseObject<OfertaApoio>> Lista
        {
            get => lista;
            set => SetProperty(ref lista, value);
        }

        public async Task Eliminar(string id)
        {
            bool confirmacao = await Application.Current.MainPage.ShowPopupAsync(new EscolhaPopUp("Aviso", "Pretende eliminar esta oferta?\nEsta ação não é reversível!", "De certeza", "Cancelar"));
            if (confirmacao)
            {
                await FBDataBase.OfertasDS.DeleteByKey(id);
            }
        }
        public OfertasPessoaisViewModel(Utilizador utilizador, string key)
        {
            DefinirLista(utilizador.Uid);
            Title = "Ofertas por: " + utilizador.NomeExibicao;
        }

        private async void DefinirLista(string uidUtilizador)
        {
            IEnumerable<FirebaseObject<OfertaApoio>> ofertas = (await FBDataBase.OfertasDS.GetAllAsync()).Where(oferta => oferta.Object.UidApoiante == uidUtilizador);
            Lista = new ObservableCollection<FirebaseObject<OfertaApoio>>(ofertas);
        }

        public OfertasPessoaisViewModel()
        {
        }
    }
}