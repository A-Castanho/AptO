using AppAptO.Models.FBData.Utilizadores;
using AppAptO.ViewModels.PopUps.Select;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace AppAptO.PopUps.Select
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectUtilizadorPopUp : Popup<string>
    {
        private readonly SelectUtilizadorViewModel viewModel;
        public SelectUtilizadorPopUp()
        {
            InitializeComponent();
            viewModel = new SelectUtilizadorViewModel();
            BindingContext = viewModel;
        }
        public SelectUtilizadorPopUp(ObservableCollection<FirebaseObject<Utilizador>> lista)
        {
            InitializeComponent();
            viewModel = new SelectUtilizadorViewModel(lista.ToDictionary(k => k.Key, u => u.Object));
            BindingContext = viewModel;
        }
        public SelectUtilizadorPopUp(Dictionary<string, Utilizador> lista)
        {
            InitializeComponent();
            viewModel = new SelectUtilizadorViewModel(lista);
            BindingContext = viewModel;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(viewModel.UtilizadorSelecionado.Key))
                Dismiss(viewModel.UtilizadorSelecionado.Key);
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Dismiss("");
        }
    }
}