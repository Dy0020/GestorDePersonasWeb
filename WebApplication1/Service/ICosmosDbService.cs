﻿using WebApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioAzureCosmos.Service
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<Persona>> GetItemsAsync(string query);
        Task<Persona> GetItemAsync(int id);
        Task AddItemAsync(Persona item);



    }
}
