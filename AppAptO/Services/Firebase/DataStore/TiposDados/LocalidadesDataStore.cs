using AppAptO.Models.FBData.TiposDados;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppAptO.Services.Firebase.DataStore.TiposDados
{
    public class LocalidadesDataStore
    {
        public LocalidadesDataStore(FirebaseClient firebaseClient)
        {
            DatabasePath = firebaseClient.Child("Tipos de Dados")
            .Child("Localidades");
        }
        public ChildQuery DatabasePath { get; }

        private async Task<string> Add(Localidade item)
        {
            return (await DatabasePath.PostAsync(JsonConvert.SerializeObject(item))).Key;
        }

        public async Task<IEnumerable<FirebaseObject<Localidade>>> GetAllAsync()
        {
            return await DatabasePath.OnceAsync<Localidade>();
        }

        public async Task<Localidade> GetByKeyAsync(string key)
        {
            return await DatabasePath.Child(key).OnceSingleAsync<Localidade>();
        }
        public async Task<FirebaseObject<Localidade>> GetByNomeAsync(string nome)
        {
            return (await DatabasePath
                .OnceAsync<Localidade>()).FirstOrDefault(item => item.Object.Nome == nome);
        }
        public async Task UpdateByKey(string key, int? nOfertas = null, int? nPedidos = null)
        {
            Localidade localidadeAtualizada = (await GetByKeyAsync(key));
            if (nOfertas >= 0)
                localidadeAtualizada.NOfertas = nOfertas ?? localidadeAtualizada.NOfertas;
            if (nPedidos >= 0)
                localidadeAtualizada.NPedidos = nPedidos ?? localidadeAtualizada.NOfertas;
            await DatabasePath.Child(key).PutAsync(localidadeAtualizada);
            if (localidadeAtualizada.Nome.Contains(","))
            {
                int index = localidadeAtualizada.Nome.IndexOf(",");
                string nomeDistrito = localidadeAtualizada.Nome.Replace(localidadeAtualizada.Nome.Substring(index), "");
                await UpdateByNome(nomeDistrito);
            }
        }
        public async Task UpdateByNome(string nome, int? nOfertas = null, int? nPedidos = null)
        {
            var fbLocalidade = (await GetByNomeAsync(nome));
            var localidadeAtualizada = fbLocalidade.Object;
            localidadeAtualizada.NOfertas = nOfertas ?? localidadeAtualizada.NOfertas;
            localidadeAtualizada.NPedidos = nPedidos ?? localidadeAtualizada.NOfertas;
            await DatabasePath.Child(fbLocalidade.Key).PutAsync(localidadeAtualizada);
            if (localidadeAtualizada.Nome.Contains(","))
            {
                int index = localidadeAtualizada.Nome.IndexOf(",");
                string nomeDistrito = localidadeAtualizada.Nome.Replace(localidadeAtualizada.Nome.Substring(index), "");
                //UpdateByNome();
            }
        }
        public async Task InsertInitialData()
        {
            await Add(new Localidade("Portugal"));
            await Add(new Localidade("Aveiro"));
            await Add(new Localidade("Aveiro, Anta e Guetim"));
            await Add(new Localidade("Aveiro, Esgueira"));
            await Add(new Localidade("Aveiro, Esmoriz"));
            await Add(new Localidade("Aveiro, Espinho"));
            await Add(new Localidade("Aveiro, Gafanha da Nazaré"));
            await Add(new Localidade("Aveiro, Glória e Vera Cruz"));
            await Add(new Localidade("Aveiro, Murtosa"));
            await Add(new Localidade("Aveiro, Oliveira do Bairro"));
            await Add(new Localidade("Aveiro, Ovar, São João,Arada e São Vicente de Pereira Jusã"));
            await Add(new Localidade("Aveiro, Sever do Vouga"));
            await Add(new Localidade("Aveiro, Vila de Cucujães"));
            await Add(new Localidade("Aveiro, Ílhavo (São Salvador)"));
            await Add(new Localidade("Beja"));
            await Add(new Localidade("Beja, Altivo"));
            await Add(new Localidade("Beja, Amarelaja"));
            await Add(new Localidade("Beja, Barrancos"));
            await Add(new Localidade("Beja, Cuba"));
            await Add(new Localidade("Beja, Mértola"));
            await Add(new Localidade("Beja, Ourique"));
            await Add(new Localidade("Beja, Pias"));
            await Add(new Localidade("Beja, São Teotónio"));
            await Add(new Localidade("Beja, Vidigueira"));
            await Add(new Localidade("Beja, Vila Nova de Milfontes"));
            await Add(new Localidade("Braga"));
            await Add(new Localidade("Braga, Arconzelo"));
            await Add(new Localidade("Braga, Fafe"));
            await Add(new Localidade("Braga, Nogueira, Fraião e Lamaçães"));
            await Add(new Localidade("Braga, Vieira do Minho"));
            await Add(new Localidade("Bragança"));
            await Add(new Localidade("Bragança, Alfandega da Fé"));
            await Add(new Localidade("Bragança, Carrazeda de Ansiães"));
            await Add(new Localidade("Bragança, Freixo de Espada à Cinta e Mazouco"));
            await Add(new Localidade("Bragança, Freixo de Espada à Cinta"));
            await Add(new Localidade("Bragança, Macedo de Cavaleiros"));
            await Add(new Localidade("Bragança, Miranda do Douro"));
            await Add(new Localidade("Bragança, Mirandela"));
            await Add(new Localidade("Bragança, Torre de Moncorvo"));
            await Add(new Localidade("Bragança, Vimioso"));
            await Add(new Localidade("Bragança, Vinhais"));
            await Add(new Localidade("Castelo Branco"));
            await Add(new Localidade("Castelo Branco, Alcains"));
            await Add(new Localidade("Castelo Branco, Boidobra"));
            await Add(new Localidade("Castelo Branco, Cantar-Galo e Vila do Carvalho"));
            await Add(new Localidade("Castelo Branco, Castelo Branco"));
            await Add(new Localidade("Castelo Branco, Cernache do Bonjardim, Nesperal e Palhais"));
            await Add(new Localidade("Castelo Branco, Fundão, Valverde, Donas, Aldeia de Joanes e Aldeia Nova do Cabo"));
            await Add(new Localidade("Castelo Branco, Penamacor"));
            await Add(new Localidade("Castelo Branco, Sertã"));
            await Add(new Localidade("Castelo Branco, Teixoso e Sarzedo"));
            await Add(new Localidade("Castelo Branco, Tortosendo"));
            await Add(new Localidade("Castelo Branco, Vila de Rei"));
            await Add(new Localidade("Castelo Branco, Vila Velha de Rodão"));
            await Add(new Localidade("Coimbra"));
            await Add(new Localidade("Coimbra, Arganil"));
            await Add(new Localidade("Coimbra, Buarcos"));
            await Add(new Localidade("Coimbra, Eiras e São Paulo de Frades"));
            await Add(new Localidade("Coimbra, Figueira de Lorvão"));
            await Add(new Localidade("Coimbra, Góis"));
            await Add(new Localidade("Coimbra, Mira"));
            await Add(new Localidade("Coimbra, Miranda do Corvo"));
            await Add(new Localidade("Coimbra, Pampilhosa da Serra"));
            await Add(new Localidade("Coimbra, Penacova"));
            await Add(new Localidade("Coimbra, Santa Clara e Castelo Viegas"));
            await Add(new Localidade("Coimbra, Santo António dos Olivais"));
            await Add(new Localidade("Coimbra, Soure"));
            await Add(new Localidade("Coimbra, São Martinho do Bispo e Ribeira de Frades"));
            await Add(new Localidade("Coimbra, Tavarede"));
            await Add(new Localidade("Coimbra, Tábua"));
            await Add(new Localidade("Évora"));
            await Add(new Localidade("Évora, Arraiolos"));
            await Add(new Localidade("Évora, Bacelo E Senhora Da Saúde"));
            await Add(new Localidade("Évora, Canaviais"));
            await Add(new Localidade("Évora, Malagueira E Horta Das Figueiras"));
            await Add(new Localidade("Évora, Mora"));
            await Add(new Localidade("Évora, Mourão"));
            await Add(new Localidade("Évora, Nossa Senhora Da Vila, Nossa Senhora Do Bispo E Silveiras"));
            await Add(new Localidade("Évora, Portel"));
            await Add(new Localidade("Évora, Redondo"));
            await Add(new Localidade("Évora, Reguengos de Monsaraz"));
            await Add(new Localidade("Évora, Vendas Novas"));
            await Add(new Localidade("Évora, Viana do Alentejo"));
            await Add(new Localidade("Faro"));
            await Add(new Localidade("Faro, Algoz E Tunes"));
            await Add(new Localidade("Faro, Aljezur"));
            await Add(new Localidade("Faro, Almancil"));
            await Add(new Localidade("Faro, Armação De Pera"));
            await Add(new Localidade("Faro, Castro Marim Conceição E Estoi"));
            await Add(new Localidade("Faro, Ferreiras"));
            await Add(new Localidade("Faro, Moncarapacho E Fuseta"));
            await Add(new Localidade("Faro, Monchique"));
            await Add(new Localidade("Faro, Montenegro"));
            await Add(new Localidade("Faro, Olhão"));
            await Add(new Localidade("Faro, Portimão"));
            await Add(new Localidade("Faro, Quarteira"));
            await Add(new Localidade("Guarda"));
            await Add(new Localidade("Guarda, Almeida"));
            await Add(new Localidade("Guarda, Figueira de Castelo Rodrigo Fornos de Algodres"));
            await Add(new Localidade("Guarda, Guarda"));
            await Add(new Localidade("Guarda, Pinhel"));
            await Add(new Localidade("Guarda, Seia, São Romão e Lapa Dos Dinheiros"));
            await Add(new Localidade("Guarda, Vila Nova de Foz Côa"));
            await Add(new Localidade("Leiria"));
            await Add(new Localidade("Leiria, Alvaiázere"));
            await Add(new Localidade("Leiria, Ansião"));
            await Add(new Localidade("Leiria, Atouguia da Baleia"));
            await Add(new Localidade("Leiria, Batalha"));
            await Add(new Localidade("Leiria, Leiria, Pousos, Barreira E Cortes"));
            await Add(new Localidade("Leiria, Maceira"));
            await Add(new Localidade("Leiria, Marinha Grande"));
            await Add(new Localidade("Leiria, Marrazes E Barosa"));
            await Add(new Localidade("Leiria, Nazaré"));
            await Add(new Localidade("Leiria, Pedrógão Grande"));
            await Add(new Localidade("Leiria, Peniche"));
            await Add(new Localidade("Leiria, Pombal"));
            await Add(new Localidade("Lisboa"));
            await Add(new Localidade("Lisboa, Agualva E Mira-Sintra"));
            await Add(new Localidade("Lisboa, Alcabideche"));
            await Add(new Localidade("Lisboa, Algueirão-Mem Martins"));
            await Add(new Localidade("Lisboa, Algés, Linda-A-Velha E Cruz Quebrada-Dafundo"));
            await Add(new Localidade("Lisboa, Alverca Do Ribatejo E Sobralinho"));
            await Add(new Localidade("Lisboa, Arruda dos Vinhos"));
            await Add(new Localidade("Lisboa, Azambuja"));
            await Add(new Localidade("Lisboa, Benfica"));
            await Add(new Localidade("Lisboa, Cacém E São Marcos"));
            await Add(new Localidade("Lisboa, Camarate, Unhos E Apelação"));
            await Add(new Localidade("Lisboa, Carcavelos E Parede"));
            await Add(new Localidade("Portalegre "));
            await Add(new Localidade("Portalegre, Alegrete"));
            await Add(new Localidade("Portalegre, Alter do Chão"));
            await Add(new Localidade("Portalegre, Assunção"));
            await Add(new Localidade("Portalegre, Assunção, Ajuda, Salvador E Santo Ildefonso"));
            await Add(new Localidade("Portalegre, Avis"));
            await Add(new Localidade("Portalegre, Caia, São Pedro E Alcáçova"));
            await Add(new Localidade("Portalegre, Crato E Mártires, Flor Da Rosa E Vale Do Peso"));
            await Add(new Localidade("Portalegre, Espírito Santo, Nossa Senhora Da Graça E São Simão"));
            await Add(new Localidade("Portalegre, Fortios"));
            await Add(new Localidade("Portalegre, Fronteira"));
            await Add(new Localidade("Portalegre, Monforte"));
            await Add(new Localidade("Porto"));
            await Add(new Localidade("Porto, Aldoar, Foz Do Douro E Nevogilde"));
            await Add(new Localidade("Porto, Campanhã"));
            await Add(new Localidade("Porto, Canidelo"));
            await Add(new Localidade("Porto, Cedofeita, Santo Ildefonso, Sé, Miragaia, São Nicolau E Vitória"));
            await Add(new Localidade("Porto, Custóias, Leça Do Balio E Guifões"));
            await Add(new Localidade("Porto, Ermesinde"));
            await Add(new Localidade("Porto, Fânzeres E São Pedro Da Cova"));
            await Add(new Localidade("Porto, Gondomar (São Cosme), Valbom E Jovim"));
            await Add(new Localidade("Porto, Lordelo Do Ouro E Massarelos"));
            await Add(new Localidade("Porto, Mafamude E Vilar Do Paraíso"));
            await Add(new Localidade("Porto, Paranhos"));
            await Add(new Localidade("Porto, Paredes"));
            await Add(new Localidade("Porto, Paços de Ferreira"));
            await Add(new Localidade("Porto, Penafiel"));
            await Add(new Localidade("Porto, Perafita, Lavra E Santa Cruz Do Bispo"));
            await Add(new Localidade("Porto, Ramalde"));
            await Add(new Localidade("Porto, Rio Tinto"));
            await Add(new Localidade("Porto, São Mamede De Infesta E Senhora Da Hora"));
            await Add(new Localidade("Porto, Valongo"));
            await Add(new Localidade("Porto, Vila do Conde"));
            await Add(new Localidade("Porto, Águas Santas"));
            await Add(new Localidade("Santarém"));
            await Add(new Localidade("Santarém, Almeirim"));
            await Add(new Localidade("Santarém, Alpiarça"));
            await Add(new Localidade("Santarém, Benavente"));
            await Add(new Localidade("Santarém, Constância"));
            await Add(new Localidade("Santarém, Fazendas de Almeirim"));
            await Add(new Localidade("Santarém, Ferreira do Zêzere"));
            await Add(new Localidade("Santarém, Fátima"));
            await Add(new Localidade("Santarém, Golegã"));
            await Add(new Localidade("Santarém, Nossa Senhora da Piedade Nossa Senhora de Fátima"));
            await Add(new Localidade("Santarém, Rio Maior"));
            await Add(new Localidade("Santarém, Samora Correia"));
            await Add(new Localidade("Santarém, Sardoal"));
            await Add(new Localidade("Santarém, São João Baptista"));
            await Add(new Localidade("Santarém, Tomar (São João Baptista) E Santa Maria Dos Olivais"));
            await Add(new Localidade("Santarém, Torres Novas (São Pedro), Lapas E Ribeira Branca"));
            await Add(new Localidade("Santarém, União de Freguesias da cidade de Santarém"));
            await Add(new Localidade("Santarém, Vila Nova da Barquinha"));
            await Add(new Localidade("Setúbal"));
            await Add(new Localidade("Setúbal, Alcochete"));
            await Add(new Localidade("Setúbal, Alhos Vedros"));
            await Add(new Localidade("Setúbal, Almada, Cova Da Piedade, Pragal E Cacilhas"));
            await Add(new Localidade("Setúbal, Alto Do Seixalinho, Santo André E Verderena"));
            await Add(new Localidade("Setúbal, Amora"));
            await Add(new Localidade("Setúbal, Azeitão (São Lourenço E São Simão)"));
            await Add(new Localidade("Setúbal, Baixa Da Banheira E Vale Da Amoreira"));
            await Add(new Localidade("Setúbal, Barreiro E Lavradio"));
            await Add(new Localidade("Setúbal, Charneca De Caparica E Sobreda"));
            await Add(new Localidade("Setúbal, Corroios"));
            await Add(new Localidade("Setúbal, Costa Da Caparica"));
            await Add(new Localidade("Setúbal, Fernão Ferro"));
            await Add(new Localidade("Setúbal, Laranjeiro E Feijó"));
            await Add(new Localidade("Setúbal, Moita"));
            await Add(new Localidade("Setúbal, Palmela"));
            await Add(new Localidade("Setúbal, Pinhal Novo"));
            await Add(new Localidade("Setúbal, Quinta do Conde"));
            await Add(new Localidade("Setúbal, Seixal, Arrentela E Aldeia De Paio Pires"));
            await Add(new Localidade("Setúbal, Sines"));
            await Add(new Localidade("Vila Real"));
            await Add(new Localidade("Vila Real, Alijó"));
            await Add(new Localidade("Vila Real, Lordelo"));
            await Add(new Localidade("Vila Real, Mateus"));
            await Add(new Localidade("Vila Real, Mondim de Basto"));
            await Add(new Localidade("Vila Real, Mouços E Lamares"));
            await Add(new Localidade("Vila Real, Murça"));
            await Add(new Localidade("Vila Real, Peso da Régua E Godim"));
            await Add(new Localidade("Vila Real, Sabrosa"));
            await Add(new Localidade("Vila Real, Santa Cruz/trindade E Sanjurge"));
            await Add(new Localidade("Vila Real, Santa Maria Maior"));
            await Add(new Localidade("Vila Real, Vila Pouca De Aguiar"));
            await Add(new Localidade("Viseu"));
            await Add(new Localidade("Viseu, Abraveses"));
            await Add(new Localidade("Viseu, Armamar"));
            await Add(new Localidade("Viseu, Castro Daire"));
            await Add(new Localidade("Viseu, Cinfães"));
            await Add(new Localidade("Viseu, Moimenta da Beira"));
            await Add(new Localidade("Viseu, Nelas"));
            await Add(new Localidade("Viseu, Repeses E São Salvador"));
            await Add(new Localidade("Viseu, Resende"));
            await Add(new Localidade("Viseu, Rio de Loba"));
            await Add(new Localidade("Viseu, S. João Da Pesqueira"));
            await Add(new Localidade("Viseu, S. Pedro Do Sul"));
            await Add(new Localidade("Viseu, Satão"));
            await Add(new Localidade("Viseu, São Pedro Do Sul, Várzea E Baiões"));
            await Add(new Localidade("Viseu, Tabuaço"));
            await Add(new Localidade("Viseu, Viseu"));
            await Add(new Localidade("Ilha da Madeira"));
            await Add(new Localidade("Ilha da Madeira, Arco da Calheta"));
            await Add(new Localidade("Ilha da Madeira, Calheta"));
            await Add(new Localidade("Ilha da Madeira, Camacha"));
            await Add(new Localidade("Ilha da Madeira, Campanario"));
            await Add(new Localidade("Ilha da Madeira, Canhas"));
            await Add(new Localidade("Ilha da Madeira, Caniçal"));
            await Add(new Localidade("Ilha da Madeira, Caniço"));
            await Add(new Localidade("Ilha da Madeira, Câmara de Lobos"));
            await Add(new Localidade("Ilha da Madeira, Estreito da Calheta"));
            await Add(new Localidade("Ilha da Madeira, Fajã da Ovelha"));
            await Add(new Localidade("Ilha da Madeira, Gaula"));
            await Add(new Localidade("Ilha da Madeira, Jardim do Mar"));
            await Add(new Localidade("Ilha da Madeira, Machico"));
            await Add(new Localidade("Ilha do Porto Santo"));
            await Add(new Localidade("Ilha de Santa Maria"));
            await Add(new Localidade("Ilha de Santa Maria, Almagreira"));
            await Add(new Localidade("Ilha de Santa Maria, Santa Bárbara"));
            await Add(new Localidade("Ilha de Santa Maria, Santo Espírito"));
            await Add(new Localidade("Ilha de Santa Maria, São Pedro"));
            await Add(new Localidade("Ilha de Santa Maria, Vila do Porto"));
            await Add(new Localidade("Ilha de São Miguel"));
            await Add(new Localidade("Ilha de São Miguel, Arrifes"));
            await Add(new Localidade("Ilha de São Miguel, Capelas"));
            await Add(new Localidade("Ilha de São Miguel, Fajã de Baixo"));
            await Add(new Localidade("Ilha de São Miguel, Fajã de Cima"));
            await Add(new Localidade("Ilha de São Miguel, Fenais da Luz"));
            await Add(new Localidade("Ilha de São Miguel, Lomba da Maia"));
            await Add(new Localidade("Ilha de São Miguel, Nordeste"));
            await Add(new Localidade("Ilha de São Miguel, Pico da Pedra"));
            await Add(new Localidade("Ilha de São Miguel, Ponta Delgada (Santa Clara)"));
            await Add(new Localidade("Ilha de São Miguel, Ponta Delgada (São Sebastião)"));
            await Add(new Localidade("Ilha de São Miguel, Ponta Garça"));
            await Add(new Localidade("Ilha de São Miguel, Povoação"));
            await Add(new Localidade("Ilha de São Miguel, Rabo de Peixe"));
            await Add(new Localidade("Ilha de São Miguel, Relva"));
            await Add(new Localidade("Ilha de São Miguel, Ribeira Grande (Matriz)"));
            await Add(new Localidade("Ilha de São Miguel, Ribeira Seca"));
            await Add(new Localidade("Ilha de São Miguel, Ribeirinha"));
            await Add(new Localidade("Ilha de São Miguel, Santo António"));
            await Add(new Localidade("Ilha Terceira"));
            await Add(new Localidade("Ilha Terceira, Altares"));
            await Add(new Localidade("Ilha Terceira, Cinco Ribeiras"));
            await Add(new Localidade("Ilha Terceira, Doze Ribeiras"));
            await Add(new Localidade("Ilha Terceira, Feteira"));
            await Add(new Localidade("Ilha Terceira, Porto Judeu"));
            await Add(new Localidade("Ilha Terceira, Posto Santo"));
            await Add(new Localidade("Ilha Terceira, Raminho"));
            await Add(new Localidade("Ilha Terceira, Ribeirinha"));
            await Add(new Localidade("Ilha Terceira, Santa Bárbara"));
            await Add(new Localidade("Ilha Terceira, São Bartolomeu de Regatos"));
            await Add(new Localidade("Ilha Terceira, São Bento"));
            await Add(new Localidade("Ilha Terceira, Terra Chã"));
            await Add(new Localidade("Ilha Terceira, Vila de São Sebastião"));
            await Add(new Localidade("Ilha da Graciosa"));
            await Add(new Localidade("Ilha da Graciosa, Guadalupe"));
            await Add(new Localidade("Ilha da Graciosa, Luz"));
            await Add(new Localidade("Ilha da Graciosa, Sata Cruz da Graciosa"));
            await Add(new Localidade("Ilha de São Jorge"));
            await Add(new Localidade("Ilha de São Jorge, Norte Grande (Neves)"));
            await Add(new Localidade("Ilha de São Jorge, Rosais"));
            await Add(new Localidade("Ilha de São Jorge, Santa Bárbara (Manadas)"));
            await Add(new Localidade("Ilha de São Jorge, Santo Amaro"));
            await Add(new Localidade("Ilha de São Jorge, Urzelina (São Mateus)"));
            await Add(new Localidade("Ilha do Pico"));
            await Add(new Localidade("Ilha do Pico, Bandeiras"));
            await Add(new Localidade("Ilha do Pico, Calheta de Nesquim"));
            await Add(new Localidade("Ilha do Pico, Candelária"));
            await Add(new Localidade("Ilha do Pico, Criação Velha Lajes do Pico"));
            await Add(new Localidade("Ilha do Pico, Madalena"));
            await Add(new Localidade("Ilha do Pico, Piedade"));
            await Add(new Localidade("Ilha do Pico, Prainha"));
            await Add(new Localidade("Ilha do Pico, Ribeiras"));
            await Add(new Localidade("Ilha do Pico, Ribeirinha"));
            await Add(new Localidade("Ilha do Pico, Santa Luzia"));
            await Add(new Localidade("Ilha do Pico, Santo Amaro"));
            await Add(new Localidade("Ilha do Pico, Santo António"));
            await Add(new Localidade("Ilha do Pico, São Caetano"));
            await Add(new Localidade("Ilha do Pico, São João"));
            await Add(new Localidade("Ilha do Pico, São Mateus"));
            await Add(new Localidade("Ilha do Pico, São Roque do Pico"));
        }
    }
}
