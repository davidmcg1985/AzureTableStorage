using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Entities;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnection"));

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("customers");

            table.CreateIfNotExists();

            //CreateCustomer(table, new CustomerUS("David", "david@test.com"));
            GetCustomer(table, "US", "david@test.com");

            Console.ReadKey();
        }

        static void CreateCustomer(CloudTable table, CustomerUS customer)
        {
            TableOperation insert = TableOperation.Insert(customer);

            table.Execute(insert);
        }

        static void GetCustomer(CloudTable table, string partitionKey, string rowKey)
        {
            TableOperation retrieve = TableOperation.Retrieve<CustomerUS>(partitionKey, rowKey);

            var result = table.Execute(retrieve);

            Console.WriteLine(((CustomerUS) result.Result).Name);
        }
    }
}
