using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using MvcStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStorage.Services
{
    public class ServiceStorageTables
    {
        private CloudTable table;

        public ServiceStorageTables(String keys)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(keys);
            CloudTableClient client = account.CreateCloudTableClient();
            this.table = client.GetTableReference("clientes");
            this.table.CreateIfNotExistsAsync();
        }

        public async Task CreateClientAsync(String idcliente, String nombre, String edad, String empresa)
        {
            Cliente cliente = new Cliente(idcliente, empresa);
            cliente.Nombre = nombre;
            cliente.Edad = edad;
            TableOperation insert = TableOperation.Insert(cliente);
            await this.table.ExecuteAsync(insert);
        }

        public async Task<List<Cliente>> GetClientsAsync()
        {
            TableQuery<Cliente> query = new TableQuery<Cliente>();
            TableQuerySegment<Cliente> segment = await this.table.ExecuteQuerySegmentedAsync(query, null);
            return segment.Results;
        }

        public async Task<List<String>> GetEmpresasAsync()
        {
            TableQuery<Cliente> query = new TableQuery<Cliente>();
            TableQuerySegment<Cliente> segment = await this.table.ExecuteQuerySegmentedAsync(query, null);
            return segment.Select(x => x.Empresa).Distinct().ToList();
        }

        public async Task<List<Cliente>> GetClientsEmpresaAsync(String empresa)
        {
            TableQuery<Cliente> query = new TableQuery<Cliente>();
            TableQuerySegment<Cliente> segment = await this.table.ExecuteQuerySegmentedAsync(query, null);
            return segment.Where(x => x.Empresa == empresa).ToList();
        }
    }
}
