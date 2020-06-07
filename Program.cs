using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ploomes.API;
using System;

namespace Ploomes
{
    class Program
    {
        private static Api api;
        static void Main(string[] args)
        {
            api = new Api();

            var idCliente = CriarCliente();
            var idNegociacao = CriarNegociacao(idCliente);
            var idTarefa = CriarTarefa(idNegociacao);
            AtualizarNegociacao(idNegociacao);
            FecharTarefa(idTarefa);
            GanharNegociacao(idNegociacao);
            EscreverNoHistorico(idCliente);

            Console.ReadLine();
        }

        private static int CriarCliente()
        {
            try
            {
                dynamic body = new JObject();
                body.Name = "Cliente";
                body.Neighborhood = "Vila Guilherme";
                body.ZipCode = 0;
                body.CompanyId = null;
                body.StreetAddressNumber = 123;
                body.TypeId = 0;
                body.Phones = JArray.FromObject(new[] { JObject.Parse(@"{PhoneNumber: '(XX) XXXX - XXXX', TypeId: 0, CountryId: 0}") });

                string createClient = api.CreateClient(body);
                Console.WriteLine("Cliente criado!");

                dynamic obj = JsonConvert.DeserializeObject(createClient);
                return obj.value[0].Id;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro na criação de usuário: \n\n" + e);
            }

            return 0;
        }

        private static int CriarNegociacao(int idCliente = 0)
        {
            try
            {
                dynamic body = new JObject();
                body.Title = "Negócio";
                body.ContactId = idCliente;
                body.Amount = 0;
                body.StageId = 0;

                string createDeal = api.CreateDeal(body);
                Console.WriteLine("Negociação criada!");

                dynamic obj = JsonConvert.DeserializeObject(createDeal);
                return obj.value[0].Id;
            }
            catch(Exception e)
            {
                Console.WriteLine("Erro na criação de negociação: \n\n" + e);
            }

            return 0;
        }

        private static int CriarTarefa(int idNegociacao = 0)
        {
            try
            {
                dynamic body = new JObject();
                body.Title = "Tarefa";
                body.Description = "Descrição da tarefa";
                body.DateTime = DateTime.Now.ToString("s");
                body.EndTime = DateTime.Now.ToString("s");
                body.ContactId = 0;
                body.DealId = idNegociacao;

                string createTask = api.CreateTask(body);
                Console.WriteLine("Tarefa criada!");

                dynamic obj = JsonConvert.DeserializeObject(createTask);
                return obj.value[0].Id;

            }
            catch(Exception e)
            {
                Console.WriteLine("Erro na criação de tarefa: \n\n" + e);
            }

            return 0;
        }

        private static void AtualizarNegociacao(int idNegociacao)
        {
            try
            {
                dynamic body = new JObject();
                body.Amount = 15000;

                api.UpdateDeal(body, idNegociacao);
                Console.WriteLine("Negociação atualizada!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro na atualização da negociação: \n\n" + e);
            }
        }
        
        private static void FecharTarefa(int idTarefa)
        {
            try
            {
                api.FinishTask(idTarefa);
                Console.WriteLine("Tarefa fechada!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro no fechamento da tarefa: \n\n" + e);
            }
        }
        
        private static void GanharNegociacao(int idNegociacao)
        {
            try
            {
                api.WinDeal(idNegociacao);
                Console.WriteLine("Negociação Ganha!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao ganhar negociação: \n\n" + e);
            }
        }

        private static void EscreverNoHistorico(int idCliente)
        {
            try
            {
                dynamic body = new JObject();
                body.ContactId = idCliente;
                body.Content = "Negócio fechado!";
                body.Date = DateTime.Now.ToString("s");

                api.InteractionRecord(body);

                Console.WriteLine("Histórico escrito!");
            }
            catch(Exception e)
            {
                Console.WriteLine("Erro ao escrever no histórico: \n\n" + e);
            }
        }
    }
}
