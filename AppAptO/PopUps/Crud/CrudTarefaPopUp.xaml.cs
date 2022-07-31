using AppAptO.Models.FBData.Apoios;
using AppAptO.ViewModels.PopUps;
using System;
using Xamarin.CommunityToolkit.UI.Views;

namespace AppAptO.PopUps.Crud
{
    public partial class CrudTarefaPopUp : Popup<Tarefa>
    {
        CrudTarefaViewModel crudTarefaViewModel;
        public CrudTarefaPopUp()
        {
            crudTarefaViewModel = new CrudTarefaViewModel();
            InitializeComponent();
            BindingContext = crudTarefaViewModel;
        }
        public CrudTarefaPopUp(Tarefa tarefa)
        {
            crudTarefaViewModel = new CrudTarefaViewModel(tarefa);
            InitializeComponent();
            BindingContext = crudTarefaViewModel;
        }

        private void Submeter_Clicked(object sender, EventArgs e)
        {
            Dismiss(crudTarefaViewModel.Tarefa);
        }
    }
}