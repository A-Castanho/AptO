using AppAptO.PopUps.Select;
using AppAptO.Services.Firebase;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace AppAptO.ViewModels
{
    //Não usada atualmente
    internal class VMMainPage : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        private ObservableCollection<Object> lista;

        public ObservableCollection<Object> Lista
        {
            get { return lista; }
            set { SetProperty(ref lista, value); }
        }
        private string selecionada;
        public string Selecionada
        {
            get { return selecionada; }
            set { SetProperty(ref selecionada, value); }
        }

        public ICommand CommandOpenPopUp => new Command(
        async () =>
        {
            var lista = (await FBDataBase.LocalidadeDS.GetAllAsync()).Select(l => l.Object.Nome);
            Selecionada = await Application.Current.MainPage.Navigation.ShowPopupAsync(new SelectAndSearchPopUp(lista));
        });
        public VMMainPage()
        {

        }
    }
}
