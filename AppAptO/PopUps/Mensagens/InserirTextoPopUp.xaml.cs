using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace AppAptO.PopUps.Mensagens
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class InserirTextoPopUp : Popup<string>
    {
        /// <summary>
        /// Pop-Up com um editor para inserir texto (retornando-o)
        /// </summary>
        /// <param name="titulo">Título a aparecer no topo do pop-up</param>
        /// <param name="textoSubmeter">Texto para o botão de submeter</param>
        public InserirTextoPopUp(string titulo, string textoSubmeter = "Submeter")
        {
            InitializeComponent();
            lblTitulo.Text = titulo;
            btnSubmit.Text = textoSubmeter;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Dismiss(editor.Text);
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Dismiss("");
        }
    }
}