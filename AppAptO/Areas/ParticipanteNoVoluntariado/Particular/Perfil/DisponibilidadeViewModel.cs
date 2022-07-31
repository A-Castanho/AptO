using AppAptO.Models.FBData.Utilizadores;

namespace AppAptO.ViewModels.Conta
{
    public class DisponibilidadeViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        private UtilizadorParticular utilizador;

        public UtilizadorParticular Utilizador
        {
            get => utilizador;
            set
            {
                SetProperty(ref utilizador, value);
            }
        }

        public DisponibilidadeViewModel()
        {
        }

        public DisponibilidadeViewModel(UtilizadorParticular utilizador)
        {
            Utilizador = utilizador;
        }
    }
}
