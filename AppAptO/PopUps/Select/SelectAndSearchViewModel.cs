using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Xamarin.CommunityToolkit.ObjectModel;

namespace AppAptO.PopUps.Select
{
    public class SelectAndSearchViewModel : ObservableObject
    {
        //try to use propertyinfo instead of string
        private ObservableCollection<string> OriginalCollection = new ObservableCollection<string>();
        private ObservableCollection<string> pickableCollection = new ObservableCollection<string>();
        public ObservableCollection<string> PickableCollection
        {
            get { return pickableCollection; }
            set { SetProperty(ref pickableCollection, value); }
        }
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                SetProperty(ref searchText, value);
                SearchCollectionByText();
            }
        }
        private string selecionado;
        public string Selecionado
        {
            get { return selecionado; }
            set
            {
                SetProperty(ref selecionado, value);
            }
        }

        private void SearchCollectionByText()
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                PickableCollection = new ObservableCollection<string>(OriginalCollection
                    .Where(value => (value.ToLower().Contains(searchText.ToLower()))));
            }
            else
            {
                PickableCollection = OriginalCollection;
            }
        }

        public SelectAndSearchViewModel(IEnumerable<string> dados)
        {
            foreach (var value in dados)
            {
                OriginalCollection.Add(value);
            }
            PickableCollection = OriginalCollection;
        }

        public SelectAndSearchViewModel()
        {
        }
    }
}
