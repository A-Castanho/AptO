using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using AppAptO.Views.Conta;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.PopUps.Select
{
    public class SelectUtilizadorViewModel : ObservableObject
    {
        private Dictionary<string, Utilizador> listaUtilizadores;
        private KeyValuePair<string, Utilizador> utilizadorSelecionado;
        public KeyValuePair<string, Utilizador> UtilizadorSelecionado
        {
            get => utilizadorSelecionado; set => SetProperty(ref utilizadorSelecionado, value);
        }

        public Dictionary<string, Utilizador> ListaUtilizadores
        {
            get { return listaUtilizadores; }
            set { SetProperty(ref listaUtilizadores, value); }
        }
        public ICommand CommandGoToUtilizador
        {
            get
            {
                return new Command(
                async () =>
                {
                    if (!string.IsNullOrEmpty(UtilizadorSelecionado.Key))
                        await Shell.Current.Navigation.PushAsync(new PerfilPage(UtilizadorSelecionado.Value, UtilizadorSelecionado.Key));
                });
            }
        }

        public SelectUtilizadorViewModel(Dictionary<string, Utilizador> listaUtilizadores)
        {
            IniciarLista(listaUtilizadores);
        }
        public SelectUtilizadorViewModel()
        {
            IniciarLista();
        }

        public async void IniciarLista(Dictionary<string, Utilizador> listaUtilizadores = null)
        {
            ListaUtilizadores = listaUtilizadores;
            if (listaUtilizadores != null)
            {
                ListaUtilizadores = listaUtilizadores;
                if (listaUtilizadores.Count > 0)
                    UtilizadorSelecionado = ListaUtilizadores.First();
            }
            else
                ListaUtilizadores = new Dictionary<string, Utilizador>((await FBDataBase.UtilizadorDS.GetAllAsync<Utilizador>()).ToDictionary(k => k.Key, u => u.Object));
        }
    }
}
