using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.Services.Firebase;
using Firebase.Database;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta.Configuracoes
{
    public class EditarDisponibilidadeViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {

        private UtilizadorParticular utilizador;
        private string utilizadorKey;

        public UtilizadorParticular Utilizador
        {
            get => utilizador;
            set => SetProperty(ref utilizador, value);
        }
        public EditarDisponibilidadeViewModel() { }
        public EditarDisponibilidadeViewModel(FirebaseObject<UtilizadorParticular> fbUtilizador)
        {
            //  Disponibilidade = fbUtilizador.;
            Utilizador = fbUtilizador.Object;
            utilizadorKey = fbUtilizador.Key;
        }

        public ICommand SubmeterCommand => new Command(
            async () =>
            {
                await FBDataBase.UtilizadorDS.Update(Utilizador, utilizadorKey);
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Sucesso!", "A sua disponibilidade foi definida.", "Ok"));
            });
    }
}
