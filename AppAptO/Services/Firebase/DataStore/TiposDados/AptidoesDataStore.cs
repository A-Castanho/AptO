using AppAptO.Models.TiposDados;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppAptO.Services.Firebase.DataStore.TiposDados
{
    public class AptidoesDataStore : IDataStore<Aptidao>
    {
        public AptidoesDataStore(FirebaseClient firebaseClient)
        {
            DatabasePath = firebaseClient.Child("Tipos de Dados")
            .Child("Aptidoes");
        }

        public ChildQuery DatabasePath { get; }
        public async Task<string> Add(Aptidao item)
        {
            return (await DatabasePath.PostAsync(JsonConvert.SerializeObject(item))).Key;
        }
        public async Task DeleteByKey(string key)
        {
            await DatabasePath.Child(key).DeleteAsync();
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FirebaseObject<Aptidao>>> GetAllAsync()
        {
            return await DatabasePath.OnceAsync<Aptidao>();
        }

        public async Task<Aptidao> GetByKeyAsync(string id)
        {
            return await DatabasePath.Child(id).OnceSingleAsync<Aptidao>();
        }
        public async Task<FirebaseObject<Aptidao>> GetByNomeAsync(string nome)
        {
            return (await DatabasePath
                .OnceAsync<Aptidao>()).FirstOrDefault(item => item.Object.NomeGeral == nome);
        }

        public async Task Update(Aptidao item, string id)
        {
            await DatabasePath.Child(id).PutAsync(item);
        }
        public async Task InsertInitialData()
        {
            await Add(new Aptidao("Artes do Espetáculo",
                new List<string>()
                {
                    "Intérprete de Dança Contemporânea",
                    "Intérprete/Ator/Atriz",
                    "Técnicas de Produção e Tecnologias da Música"
                }));
            await Add(new Aptidao("Audiovisuais e Produção dos Media",
                new List<string>()
                {
                    "Operação de Fotografia",
                    "Operação de Impressão",
                    "Operação de Pré-Impressão",
                    "Operação Gráfico de Acabamentos",
                    "Técnicas de Animação 2D e 3D",
                    "Técnicas de Artes Gráficas",
                    "Técnicas de Audiovisuais",
                    "Técnicas de Desenho Digital 3D",
                    "Técnicas de Design de Comunicação Gráfica",
                    "Técnicas de Fotografia",
                    "Técnicas de Multimédia",
                    "Técnicas de Som",
                    "Técnicas Especialista em Desenvolvimento de Produtos Multimédia",
                }));
            await Add(new Aptidao("Artesanato",
                new List<string>()
                {
                    "Artífice de Ferro",
                    "Artífice Tanoeiro/a",
                    "Assistente de Ourivesaria",
                    "Bordador/a",
                    "Calceteiro/a",
                    "Canteiro/a",
                    "Florista",
                    "Oleiro/a",
                    "Tecelão/Tecedeira",
                    "Artesanato Metal",
                    "Artesanato Têxtil",
                    "Marcenaria (Embutidor/a)",
                    "Marcenaria (Entalhador/a)",
                    "Pintura em Azulejo",
                    "Construção de Instrumentos Musicais",
                    "Ourivesaria",
                    "Ourivesaria de Pratas Graúdas/Cinzelador",
                    "Pintura Decorativa",
                    "Vidro Artístico"
                }));
            await Add(new Aptidao("História e Arqueologia",
                new List<string>()
                {
                    "Arqueologia",
                    "Museografia e Gestão do Património",
                    "Conservação e Restauro de Madeira (Escultura e Talha)"
                }));
            await Add(new Aptidao("Biblioteconomia, Arquivo e Documentação (BAD)",
                new List<string>()
                {
                    "Técnicas de Informação, Documentação e Comunicação"
                }));
            await Add(new Aptidao("Comércio",
                new List<string>()
                {
                    "Operação de Distribuição",
                    "Operação de Logística",
                    "Técnicas Comercial",
                    "Técnicas de Comunicação e Serviço Digital",
                    "Técnicas de Distribuição",
                    "Técnicas de Logística",
                    "Técnicas de Marketing",
                    "Técnicas de Vendas",
                    "Técnicas de Vitrinismo",
                    "Técnicas Especialista em Comércio Internacional"
                }));
            await Add(new Aptidao("Marketing e Publicidade",
                new List<string>()
                {
                    "Técnicas de Comunicação - Marketing, Relações Públicas e Publicidade",
                    "Técnicas de Organização de Eventos"
                }));
            await Add(new Aptidao("Finanças, Banca e Seguros",
                new List<string>()
                {
                    "Técnicas Comercial Bancário/a",
                    "Técnicas de Banca e Seguros",
                    "Técnicas Especialista em Banca e Seguros"
                }));
            await Add(new Aptidao("Contabilidade e Fiscalidade",
                new List<string>()
                {
                    "Técnicas de Contabilidade",
                    "Técnicas Especialista em Contabilidade e Fiscalidade"
                }));
            await Add(new Aptidao("Gestão e Administração",
                new List<string>()
                {
                    "Técnicas de Apoio à Gestão"
                }));
            await Add(new Aptidao("Secretariado e Trabalho Administrativo",
                new List<string>()
                {
                    "Assistente Administrativo/a",
                    "Técnicas Administrativo/a",
                    "Técnicas de Secretariado"
                }));
            await Add(new Aptidao("Enquadramento na Organização/Empresa",
                new List<string>()
                {
                    "Técnicas da Qualidade",
                    "Técnicas de Relações Laborais",
                    "Técnicas Especialista de Auditoria a Sistemas de Gestão",
                    "Técnicas Especialista em Gestão da Qualidade, Ambiente e Segurança"
                }));
            await Add(new Aptidao("Direito",
                new List<string>()
                {
                    "Técnicas de Serviços Jurídicos"
                }));
            await Add(new Aptidao("Ciências Informáticas",
                new List<string>()
                {
                    "Operação de Informática",
                    "Programador/a de Informática",
                    "Técnicas de Informática - Instalação e Gestão de Redes",
                    "Técnicas de Informática - Sistemas",
                    "Técnicas Especialista em Aplicações Informáticas de Gestão",
                    "Técnicas Especialista em Cibersegurança",
                    "Técnicas Especialista em Gestão de Redes e Sistemas Informáticos",
                    "Técnicas Especialista em Tecnologias e Programação de Sistemas de Informação"
                }));
            await Add(new Aptidao("Metalurgia e Metalomecânica",
                new List<string>()
                {
                    "Desenhador/a de Construções Mecânicas",
                    "Eletromecânico/a de Manutenção Industrial",
                    "Fresador/a Mecânico/a",
                    "Operação de Fundição",
                    "Operação de Fundição Injetada",
                    "Operação de Máquinas - Ferramenta CNC",
                    "Operação de Máquinas Ferramentas",
                    "Serralheiro/a Civil",
                    "Serralheiro/a de Moldes, Cunhos e Cortantes",
                    "Serralheiro/a Mecânico/a",
                    "Serralheiro/a Mecânico/a de Manutenção",
                    "Soldador/a",
                    "Técnicas de CAD/CAM",
                    "Técnicas de Desenho de Construções Mecânicas",
                    "Técnicas de Desenho de Cunhos e Cortantes",
                    "Técnicas de Desenho de Moldes",
                    "Técnicas de Fabrico de Componentes de Construção Metálica",
                    "Técnicas de Fabrico e Manutenção de Cunhos e Cortantes",
                    "Técnicas de Laboratório - Fundição",
                    "Técnicas de Manutenção Industrial de Metalurgia e Metalomecânica",
                    "Técnicas de Maquinação e Programação CNC",
                    "Técnicas de Planeamento Industrial de Metalurgia e Metalomecânica",
                    "Técnicas de Produção Aeronáutica - Maquinação CNC",
                    "Técnicas de Produção Aeronáutica - Processos Especiais",
                    "Técnicas de Produção Aeronáutica - Produção e Transformação de Compósitos",
                    "Técnicas de Produção Aeronáutica - Qualidade e Controlo Industrial",
                    "Técnicas de Produção e Montagem de Moldes",
                    "Técnicas de Projeto Aeronáutico",
                    "Técnicas de Projeto de Moldes e Modelos - Fundição",
                    "Técnicas de Soldadura",
                    "Técnicas Especialista em Eletromedicina",
                    "Técnicas Especialista em Gestão da Produção (Supervisor de Produção) – indústria metalúrgica e metalomecânica",
                    "Técnicas Especialista em Manutenção Industrial / Mecatrónica",
                    "Técnicas Especialista em Tecnologia de Materiais - Metalurgia e Metalomecânica",
                    "Técnicas Técnicas Especialista em Tecnologia Mecânica",
                    "Técnicas Especialista em Tecnologia Mecatrónica"
                }));
            await Add(new Aptidao("Eletricidade e Energia",
                new List<string>()
                {
                    "Eletricista de Instalações",
                    "Eletricista de Redes",
                    "Eletromecânico/a de Eletrodomésticos",
                    "Eletromecânico/a de Refrigeração e Climatização - Sistemas Domésticos e Comerciais",
                    "Desenhador/a de Sistemas de Refrigeração e Climatização",
                    "Técnicas de Eletrotecnia",
                    "Técnicas de Instalações Elétricas",
                    "Técnicas de Redes Elétricas",
                    "Técnicas de Refrigeração e Climatização",
                    "Técnicas Instalador/a de Sistemas Eólicos",
                    "Técnicas Instalador/a de Sistemas Solares Fotovoltaicos",
                    "Técnicas Instalador/a de Sistemas Térmicos de Energias Renováveis",
                    "Técnicas Supervisor/a de Redes e Aparelhos a Gás",
                    "Técnicas Especialista em Gestão e Controlo de Energia"
                }));
            await Add(new Aptidao("Eletrónica e Automação",
                new List<string>()
                {
                    "Instalador/a - Reparador/a de Áudio, Rádio, TV e Vídeo",
                    "Instalador/a - Reparador/a de Computadores",
                    "Operação de Eletrónica/Computadores",
                    "Operação de Eletrónica/Domótica",
                    "Operação de Eletrónica/Industrial e Equipamentos",
                    "Operação de Eletrónica/Instrumentação, Controlo e Telemanutenção",
                    "Operação de Eletrónica/Telecomunicações",
                    "Técnicas de Eletrónica e Telecomunicações",
                    "Técnicas de Eletrónica Médica",
                    "Técnicas de Eletrónica, Áudio, Vídeo e TV",
                    "Técnicas de Eletrónica, Automação e Comando",
                    "Técnicas de Eletrónica, Automação e Computadores",
                    "Técnicas de Eletrónica, Automação e Instrumentação",
                    "Técnicas de Mecatrónica",
                    "Técnicas de Relojoaria",
                    "Técnicas Especialista em Automação, Robótica e Controlo Industrial",
                    "Técnicas Especialista em Gestão para a Indústria – Processos e Sistemas Mecatrónicos",
                    "Técnicas Técnicas Especialista em Telecomunicações e Redes"
                }));
            await Add(new Aptidao("Tecnologia dos Processos Químicos",
                new List<string>()
                {
                    "Técnicas de Análise Laboratorial",
                    "Técnicas de Química Industrial"
                }));
            await Add(new Aptidao("Construção e Reparação de Veículos a Motor",
                new List<string>()
                {
                    "Eletricista de Automóveis",
                    "Mecânico/a de Automóveis Ligeiros",
                    "Mecânico/a de Automóveis Pesados de Passageiros e de Mercadorias",
                    "Mecânico/a de Equipamentos de Movimentação de Terras",
                    "Mecânico/a de Serviços Rápidos",
                    "Operação de Construção e Reparação Naval",
                    "Pintor/a de Veículos",
                    "Reparador/a de Carroçarias de Automóveis Ligeiros",
                    "Reparador/a de Motociclos",
                    "Mecânico/a de Aeronaves e de Material de Voo",
                    "Técnicas de Aprovisionamento e Venda de Peças",
                    "Técnicas de Construção Naval/Embarcações de Recreio",
                    "Técnicas de Mecatrónica Automóvel",
                    "Técnicas de Mecatrónica de Motociclos",
                    "Técnicas de Produção Aeronáutica – Montagem de Estruturas",
                    "Técnicas de Produção Automóvel",
                    "Técnicas de Receção/Orçamentação de Oficina",
                    "Técnicas de Reparação e Pintura de Carroçarias",
                    "Técnicas Especialista em Mecatrónica Automóvel, Planeamento e Controlo de Processos"
                }));
            await Add(new Aptidao("Indústrias Alimentares",
                new List<string>()
                {
                    "Operação de Preparação e Transformação de Produtos Cárneos",
                    "Operação de Transformação do Pescado",
                    "Pasteleiro/a - Padeiro/a",
                    "Técnicas de Controlo de Qualidade Alimentar",
                    "Técnicas de Indústrias Alimentares"
                }));
            await Add(new Aptidao("Indústria do Têxtil, Vestuário, Calçado e Couro",
                new List<string>()
                {
                    "Costureiro/a Industrial de Malhas",
                    "Costureiro/a Industrial de Tecidos",
                    "Costureiro/a Modista",
                    "Operação de Fabrico de Calçado",
                    "Operação de Fabrico de Marroquinaria",
                    "Operação de Fiação",
                    "Operação de Tecelagem",
                    "Operação de Tinturaria",
                    "Operação de Tricotagem",
                    "N4 Alfaiate",
                    "Modelista de Vestuário",
                    "Técnicas de Desenho de Vestuário",
                    "Técnicas de Design de Moda",
                    "Técnicas de Enobrecimento Têxtil",
                    "Técnicas de Fabrico Manual de Calçado",
                    "Técnicas de Gestão da Produção de Calçado e de Marroquinaria",
                    "Técnicas de Malhas - Máquinas Retas",
                    "Técnicas de Manutenção de Máquinas de Calçado e de Marroquinaria",
                    "Técnicas de Máquinas de Confeção",
                    "Técnicas de Modelação de Calçado",
                    "Técnicas de Tecelagem",
                    "N5 Técnicas Especialista em Comércio Moda",
                    "Técnicas Especialista em Design de Calçado",
                    "Técnicas Especialista em Design Têxtil para Estamparia",
                    "Técnicas Especialista em Design Têxtil para Malhas",
                    "Técnicas Especialista em Design Têxtil para Tecelagem",
                    "Técnicas Especialista em Gestão do Processo Têxtil",
                    "Técnicas Especialista em Industrialização de Produto Moda",
                    "Técnicas Especialista em Processos de Coloração e Acabamentos Têxteis",
                    "Técnicas Especialista em Têxteis Técnicos e Funcionais"
                }));
            await Add(new Aptidao("Indústrias Materiais",
                new List<string>()
                {
                    "Carpinteiro/a / Carpinteiro/a de Limpos",
                    "Formista/Moldista",
                    "Marcenaria",
                    "Operação de Acabamentos de Madeira e Mobiliário",
                    "Operação de Cerâmica",
                    "Operação de Granulação e Aglomeração de Cortiça",
                    "Operação de Máquinas de Produção de Artigos em Vidro",
                    "Operação de Máquinas de Segunda Transformação da Madeira",
                    "Operação de Transformação de Cortiça",
                    "Pintor/a / Decorador/a",
                    "Preparador/a de Cortiça",
                    "Vidreiro/a",
                    "Técnicas de Acabamento de Madeira e Mobiliário",
                    "Técnicas de Cerâmica",
                    "Técnicas de Cerâmica Criativa",
                    "Técnicas de Desenho de Mobiliário e Construções em Madeira",
                    "Técnicas de Gestão da Produção da Indústria da Cortiça",
                    "Técnicas de Gestão da Produção em Madeira e Mobiliário",
                    "Técnicas de Laboratório Cerâmico",
                    "Técnicas de Modelação Cerâmica",
                    "Técnicas de Pintura Cerâmica",
                    "Técnicas de Preparação de Cortiça",
                    "Técnicas de Programação e Operação em Máquinas de Transformação de Madeira",
                    "Técnicas de Transformação de Polímeros/Processos de Produção",
                    "Técnicas de Vidro",
                    "Técnicas Industrial de Rolhas de Cortiça",
                    "Técnicas Especialista em Conceção e Desenvolvimento do Produto – Cerâmica",
                    "Técnicas Especialista em Ofícios de Arte - Cerâmica e Vidro"
                }));
            await Add(new Aptidao("Indústrias Extrativas",
                new List<string>()
                {
                    "Operação de Salinas Tradicionais",
                    "Operação Mineiro/a",
                    "Técnicas Especialista em Produção Industrial de Rochas Ornamentais e Industriais"
                }));
            await Add(new Aptidao("Construção Civil e Engenharia Civil",
                new List<string>()
                {
                    "Canalizador/a",
                    "Condutor/a / Manobrador/a de Equipamento de Movimentação de Terras",
                    "Condutor/a/Manobrador/a de Equipamentos de Elevação",
                    "Ladrilhador/a / Azulejador/a",
                    "Operação de CAD - Construção Civil",
                    "Pedreiro/a",
                    "Pintor/a de Construção Civil",
                    "Técnicas de Desenho da Construção Civil",
                    "Técnicas de Ensaios da Construção Civil e Obras Públicas",
                    "Técnicas de Medições e Orçamentos",
                    "Técnicas de Obra/Condutor/a de Obra",
                    "Técnicas de Topografia",
                    "Técnicas Especialista em Condução de Obra",
                    "Técnicas Especialista em Reabilitação Energética e Conservação de Infraestruturas - Edificações"
                }));
            await Add(new Aptidao("Produção Agrícola e Animal",
                new List<string>()
                {
                    "Operação Agrícola",
                    "Operação Apícola",
                    "Operação de Máquinas Agrícolas",
                    "Operação de Pecuária",
                    "Tratador/a / Desbastador/a de Equinos",
                    "Tratador/a de Animais em Cativeiro",
                    "Técnicas Apícola",
                    "Técnicas de Gestão Equina",
                    "Técnicas de Produção Agropecuária",
                    "Técnicas Vitivinícola"
                }));
            await Add(new Aptidao("Floricultura e Jardinagem",
                new List<string>()
                {
                    "Operação de Jardinagem",
                    "Operação de Manutenção em Campos de Golfe",
                    "Técnicas de Jardinagem e Espaços Verdes"
                }));
            await Add(new Aptidao("Silvicultura e Caça",
                new List<string>()
                {
                    "Motosserrista",
                    "Operação Florestal",
                    "Sapador/a Florestal",
                    "Técnicas de Gestão Cinegética",
                    "Técnicas de Máquinas Florestais",
                    "Técnicas de Recursos Florestais e Ambientais"
                }));
            await Add(new Aptidao("Pescas",
                new List<string>()
                {
                    "Operação Aquícola",
                    "Técnicas de Aquicultura"
                }));
            await Add(new Aptidao("Ciências Dentária",
                new List<string>()
                {
                    "Técnicas Assistente Dentário"
                }));
            await Add(new Aptidao("Tecnologias de Diagnóstico e Terapêutica",
                new List<string>()
                {
                    "Técnicas de Ótica Ocular"
                }));
            await Add(new Aptidao("Ciências Farmacêuticas",
                new List<string>()
                {
                    "Técnicas Auxiliar de Farmácia"
                }));
            await Add(new Aptidao("Saúde - Programas noutra Área de Formação",
                new List<string>()
                {
                    "Operação de Hidrobalneoterapia ",
                    "Técnicas Auxiliar de Saúde ",
                    "Técnicas de Termalismo"
                }));
            await Add(new Aptidao("Serviços de Apoio a Crianças e Jovens",
                new List<string>()
                {
                    "Cuidador/a de Crianças e Jovens",
                    "Técnicas de Ação Educativa",
                    "Técnicas de Juventude"
                }));
            await Add(new Aptidao("Trabalho Social e Orientação",
                new List<string>()
                {
                    "Agente em Geriatria",
                    "Assistente Familiar e de Apoio à Comunidade ",
                    "N4 Animador/a Sociocultural",
                    "Mediador/a Intercultural",
                    "Técnicas de Apoio Familiar e de Apoio à Comunidade",
                    "Técnicas de Apoio Psicossocial",
                    "Técnicas de Geriatria",
                }));
            await Add(new Aptidao("Hotelaria e Restauração",
                new List<string>()
                {
                    "Cozinheiro/a",
                    "Empregado/a de Andares",
                    "Empregado/a de Restaurante/Bar",
                    "Operação de Manutenção Hoteleira",
                    "Rececionista de Hotel",
                    "Técnicas de Cozinha/Pastelaria",
                    "Técnicas de Manutenção - Hotelaria",
                    "Técnicas de Pastelaria/Padaria",
                    "Técnicas de Restaurante/Bar",
                    "N5 Técnicas Especialista em Gestão de Restauração e Bebidas",
                    "Técnicas Especialista em Gestão e Produção de Cozinha",
                    "Técnicas Especialista em Gestão e Produção de Pastelaria",
                    "Técnicas Especialista em Gestão Hoteleira e Alojamento",
                }));
            await Add(new Aptidao("Turismo e Lazer",
                new List<string>()
                {
                    "Acompanhante de Turismo Equestre",
                    "Técnicas de Agências de Viagens e Transportes",
                    "Técnicas de Informação e Animação Turística",
                    "Técnicas de Turismo Ambiental e Rural",
                    "N5 Técnicas Especialista de Animação em Turismo de Saúde e Bem-estar",
                    "Técnicas Especialista de Gestão de Turismo",
                    "Técnicas Especialista de Turismo Ambiental",
                    "Técnicas Especialista em Turismo Cultural e Património",
                    "Técnicas Especialista em Turismo de Natureza e Aventura",
                }));
            await Add(new Aptidao("Desporto",
                new List<string>()
                {
                    "Técnicas de Apoio à Gestão Desportiva",
                    "Técnicas de Desporto",
                    "Técnicas Especialista em Exercício Físico"
                }));
            await Add(new Aptidao("Serviços Domésticos",
                new List<string>()
                {
                    "Agente Funerário",
                    "Técnicas de Serviços Funerários"
                }));
            await Add(new Aptidao("Cuidados de Beleza",
                new List<string>()
                {
                    "Assistente de Cabeleireiro/a",
                    "Assistente de Cuidados de Beleza",
                    "Manicura – Pedicura",
                    "Cabeleireiro/a",
                    "Esteticista",
                    "Técnicas de Massagem de Estética e Bem-Estar",
                }));
            await Add(new Aptidao("Serviços de Transporte",
                new List<string>()
                {
                    "Maquinista Marítimo/a",
                    "Marinheiro/a",
                    "Técnicas de Condução de Veículos de Transporte Rodoviário",
                    "Técnicas de Gestão de Transportes",
                    "Técnicas de Manutenção e Operação Ferroviária",
                    "Técnicas de Tráfego de Assistência em Escala"
                }));
            await Add(new Aptidao("Proteção do Ambiente - Programas Transversais",
                new List<string>()
                {
                    "Operação de Sistemas de Gestão de Resíduos Sólidos",
                    "Operação de Sistemas de Tratamento de Águas",
                    "Técnicas de Gestão do Ambiente",
                    "Técnicas de Sistemas de Tratamento de Águas"
                }));
            await Add(new Aptidao("Proteção de Pessoas e Bens",
                new List<string>()
                {
                    "Bombeiro/a",
                    "Técnicas de Proteção Civil",
                    "Técnicas de Socorros e Emergências de Aeródromo",
                    "N5 Técnicas Especialista em Administração e Gestão de Organismos de Segurança Interna",
                    "Técnicas Especialista em Sistema de Segurança Interna",
                }));
            await Add(new Aptidao("Segurança e Higiene no Trabalho",
                new List<string>()
                {
                    "Técnicas de Segurança no Trabalho"
                }));
        }

        internal Task GetByKeyAsync(object key)
        {
            throw new NotImplementedException();
        }
    }
}

