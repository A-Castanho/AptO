namespace AppAptO.ViewModels.PopUps
{
    public class MensagemPopupViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        private string titulo;
        public string Titulo
        {
            get => titulo;
            set => SetProperty(ref titulo, value);
        }
        private string mensagem;
        public string Mensagem
        {
            get => mensagem;
            set => SetProperty(ref mensagem, value);
        }
        private string textoSaida;
        public string TextoSaida
        {
            get => textoSaida;
            set => SetProperty(ref textoSaida, value);
        }

        public MensagemPopupViewModel()
        {
        }

        public MensagemPopupViewModel(string titulo, string mensagem, string textoSaida)
        {
            Titulo = titulo;
            Mensagem = mensagem;
            TextoSaida = textoSaida;
        }
    }
}
