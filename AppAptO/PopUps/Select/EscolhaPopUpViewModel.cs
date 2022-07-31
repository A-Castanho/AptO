using Xamarin.CommunityToolkit.ObjectModel;

namespace AppAptO.ViewModels.PopUps
{
    public class EscolhaPopUpViewModel : ObservableObject
    {
        public string titulo;
        public string Titulo
        {
            get => titulo;
            set => SetProperty(ref titulo, value);
        }
        public string mensagem;
        public string Mensagem
        {
            get => mensagem;
            set => SetProperty(ref mensagem, value);
        }
        private string textoSaidaPositiva;
        public string TextoSaidaPositiva
        {
            get => textoSaidaPositiva;
            set => SetProperty(ref textoSaidaPositiva, value);
        }
        private string textoSaidaNegativa;
        public string TextoSaidaNegativa
        {
            get => textoSaidaNegativa;
            set => SetProperty(ref textoSaidaNegativa, value);
        }

        public EscolhaPopUpViewModel()
        {
        }

        public EscolhaPopUpViewModel(string titulo, string mensagem, string textoSaidaPositiva = "Sim", string textoSaidaNegativa = "Não")
        {
            Titulo = titulo;
            Mensagem = mensagem;
            TextoSaidaPositiva = textoSaidaPositiva;
            TextoSaidaNegativa = textoSaidaNegativa;
        }
    }
}
