using System;
using Xamarin.CommunityToolkit.ObjectModel;

namespace AppAptO.Models.FBData.DadosUtiliizadores
{
    public class Disponibilidade : ObservableObject
    {
        private DisponibilidadeDiaria segunda = new DisponibilidadeDiaria();
        private DisponibilidadeDiaria terca = new DisponibilidadeDiaria();
        private DisponibilidadeDiaria quarta = new DisponibilidadeDiaria();
        private DisponibilidadeDiaria quinta = new DisponibilidadeDiaria();
        private DisponibilidadeDiaria sexta = new DisponibilidadeDiaria();
        private DisponibilidadeDiaria sabado = new DisponibilidadeDiaria();
        private DisponibilidadeDiaria domingo = new DisponibilidadeDiaria();

        public class DisponibilidadeDiaria : ObservableObject
        {
            private TimeSpan inicio = new TimeSpan();
            private TimeSpan fim = new TimeSpan();
            public TimeSpan Inicio { get => inicio; set => SetProperty(ref inicio, value); }
            public TimeSpan Fim { get => fim; set => SetProperty(ref fim, value); }
        }

        public Disponibilidade()
        {
        }

        public Disponibilidade(DisponibilidadeDiaria disponibilidadeTotal)
        {
            Segunda = disponibilidadeTotal;
            Terca = disponibilidadeTotal;
            Quarta = disponibilidadeTotal;
            Quinta = disponibilidadeTotal;
            Sexta = disponibilidadeTotal;
            Sabado = disponibilidadeTotal;
            Domingo = disponibilidadeTotal;
        }

        public DisponibilidadeDiaria Segunda { get => segunda; set => SetProperty(ref segunda, value); }
        public DisponibilidadeDiaria Terca { get => terca; set => SetProperty(ref terca, value); }
        public DisponibilidadeDiaria Quarta { get => quarta; set => SetProperty(ref quarta, value); }
        public DisponibilidadeDiaria Quinta { get => quinta; set => SetProperty(ref quinta, value); }
        public DisponibilidadeDiaria Sexta { get => sexta; set => SetProperty(ref sexta, value); }
        public DisponibilidadeDiaria Sabado { get => sabado; set => SetProperty(ref sabado, value); }
        public DisponibilidadeDiaria Domingo { get => domingo; set => SetProperty(ref domingo, value); }
    }
}
