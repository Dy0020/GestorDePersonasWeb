using WebApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioAzureCosmos.Service
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<Persona>> GetItemsAsync(string query);
        Task<Persona> GetItemAsync(string id);
        Task AddItemAsync(Persona item);
        Task UpdateItemAsync(string id, Persona libro);
        Task DeleteItemAsync(string id);


    }
}
