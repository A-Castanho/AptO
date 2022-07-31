using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppAptO.ViewModels.Conta
{
    public class DadosParticularViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        private ObservableCollection<string> aptidoes = new ObservableCollection<string>();

        public ObservableCollection<string> Aptidoes
        {
            get { return aptidoes; }
            set { SetProperty(ref aptidoes, value); }
        }

        private UtilizadorParticular utilizador = new UtilizadorParticular();
        public UtilizadorParticular Utilizador
        {
            get => utilizador;
            set => SetProperty(ref utilizador, value);
        }
        public DadosParticularViewModel(UtilizadorParticular utilizador)
        {
            Utilizador = utilizador;
            SetAptidoes();
        }

        public async void SetAptidoes()
        {
            var fbAptidoes = (await FBDataBase.AptidoesDS.GetAllAsync()).ToList();
            foreach (var ap in Utilizador.Aptidoes)
            {
                var aptidao = fbAptidoes.FirstOrDefault(a => a.Key == ap.Key);
                foreach (var apInterior in ap.Value)
                {
                    Aptidoes.Add(aptidao.Object.AreasEnglobadas[apInterior]);
                }
            }
        }

        public DadosParticularViewModel()
        {
        }
    }
}
