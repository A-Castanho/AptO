using AppAptO.Services.Firebase.DataStore.Apoios;
using AppAptO.Services.Firebase.DataStore.Chats;
using AppAptO.Services.Firebase.DataStore.TiposDados;
using AppAptO.Services.Firebase.DataStore.Utilizadores;
using Firebase.Database;

namespace AppAptO.Services.Firebase
{
    public static class FBDataBase
    {

        private readonly static FirebaseClient firebaseClient = new FirebaseClient("https://apto-29930-default-rtdb.firebaseio.com/");
        /// <summary>
        /// DataStore das ofertas de apoio
        /// </summary>
        public static OfertasDataStore OfertasDS { get; } = new OfertasDataStore(firebaseClient);
        /// <summary>
        /// DataStore dos pedidos de apoio
        /// </summary>
        public static PedidosDataStore PedidosDS { get; } = new PedidosDataStore(firebaseClient);
        /// <summary>
        /// DataStore das localidades
        /// </summary>
        public static LocalidadesDataStore LocalidadeDS { get; } = new LocalidadesDataStore(firebaseClient);
        /// <summary>
        /// DataStore das áreas de apoio
        /// </summary>
        public static AreasDataStore AreasApoioDS { get; } = new AreasDataStore(firebaseClient);
        /// <summary>
        /// DataStore das áreas de experiencia dos utilizadores
        /// </summary>
        public static AptidoesDataStore AptidoesDS { get; } = new AptidoesDataStore(firebaseClient);
        /// <summary>
        /// DataStore dos utilizadores
        /// </summary>
        public static UtilizadorDataStore UtilizadorDS { get; } = new UtilizadorDataStore(firebaseClient);
        /// <summary>
        /// DataStore dos chats
        /// </summary>
        public static ChatDataStore ChatDS { get; } = new ChatDataStore(firebaseClient);
        /// <summary>
        /// DataStore das publicidades
        /// </summary>
        public static PublicidadeDataStore PublicidadeDS { get; } = new PublicidadeDataStore(firebaseClient);
        /// <summary>
        /// DataStore das mensagens 'sabias que'
        /// </summary>
        public static MsgSabiasDataStore MsgSabiasDS { get; } = new MsgSabiasDataStore(firebaseClient);

    }
}
