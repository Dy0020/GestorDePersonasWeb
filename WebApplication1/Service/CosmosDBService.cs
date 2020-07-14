using WebApplication1.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioAzureCosmos.Service
{
    public class CosmosDBService : ICosmosDbService
    {
        private Container _container;

        public CosmosDBService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(Persona persona)
        {
            await this._container.CreateItemAsync<Persona>(persona, new PartitionKey(persona.id));
        }


        public async Task<Persona> GetItemAsync(int id)
        {
            try
            {
                ItemResponse<Persona> response = await this._container.ReadItemAsync<Persona>(id.ToString(), new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Persona>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Persona>(new QueryDefinition(queryString));
            List<Persona> results = new List<Persona>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }


    }
}
