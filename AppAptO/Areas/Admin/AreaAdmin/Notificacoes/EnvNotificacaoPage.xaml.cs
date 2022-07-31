using AppAptO.Models.FBData.Utilizadores;
using Firebase.Database;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.Admin.AreaAdmin.Notificacoes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnvNotificacaoPage : ContentPage
    {
        public EnvNotificacaoPage()
        {
            InitializeComponent();
            ((EnvNotificacaoViewModel)BindingContext).KeysUtilizadoresSelecionados.CollectionChanged += (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) =>
            {
                var keys = ((EnvNotificacaoViewModel)BindingContext).KeysUtilizadoresSelecionados;
                if (keys.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Keys dos Selecionados: ");
                    foreach (string item in keys)
                    {
                        sb.Append(item + ", ");
                    }
                    sb.Remove(sb.Length - 2, 2);
                    labelKeysUtilizadores.Text = sb.ToString();
                }
                else
                    labelKeysUtilizadores.IsVisible = false;

            };
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListView list = (ListView)sender;
            if (list.SelectedItem != null)
            {
                FirebaseObject<Utilizador> selecionado = (FirebaseObject<Utilizador>)list.SelectedItem;
                ((EnvNotificacaoViewModel)BindingContext).CommandSelectUtilizador.Execute(selecionado.Key);
            }
            list.SelectedItem = null;
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            ((EnvNotificacaoViewModel)BindingContext).CommandPesquisar.Execute(null);
        }
    }
}