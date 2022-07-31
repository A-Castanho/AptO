using AppAptO.Models.FBConnections;
using Xamarin.Forms;

namespace AppAptO.Helpers.Chat
{
    class ChatTemplateSelector : DataTemplateSelector
    {
        DataTemplate incomingDataTemplate;
        DataTemplate outgoingDataTemplate;

        public ChatTemplateSelector()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as MensagemFormatada;
            if (messageVm == null)
                return null;

            return (messageVm.KeyUtilizador == AuthHelper.UtilizadorAtual.Key) ? outgoingDataTemplate : incomingDataTemplate;
        }

    }
}
