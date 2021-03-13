using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStorage.Models
{
    public class Cliente: TableEntity
    {
        //CUALQUIER TableEntity NECESITA UN CONSTRUCTOR POR DEFECTO
        public Cliente() { }
        public Cliente(String idcliente, String empresa)
        {
            this.IdCliente = idcliente;
            this.Empresa = empresa;
            //RowKey único
            this.RowKey = idcliente;
            //GRUPO AL QUE PERTENECERÁ
            this.PartitionKey = empresa;
        }

        //COMO SE GUARDA EN JSON, SE GUARDAN COMO STRING
        public String IdCliente { get; set; }
        public String Nombre { get; set; }
        public String Edad { get; set; }
        public String Empresa { get; set; }
    }
}
