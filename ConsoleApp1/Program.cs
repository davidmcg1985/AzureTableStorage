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

            //CreateCustomer(table, new CustomerUS("Mike", "mike@test.com"));
            //GetCustomer(table, "US", "david@test.com");
            //GetAllCustomers(table);

            //var updateMike = GetCustomer(table, "US", "mike@test.com");
            //updateMike.Name = "Michael";
            //UpdateCustomer(table, updateMike);
            //DeleteCustomer(table, updateMike);

            //GetAllCustomers(table);

            TableBatchOperation batch = new TableBatchOperation();

            var customer1 = new CustomerUS("Frank", "frank@test.com");
            var customer2 = new CustomerUS("Steve", "steve@test.com");
            var customer3 = new CustomerUS("Joe", "joe@test.com");

            batch.Insert(customer1);
            batch.Insert(customer2);
            batch.Insert(customer3);

            table.ExecuteBatch(batch);

            GetAllCustomers(table);

            Console.ReadKey();
        }

        static void CreateCustomer(CloudTable table, CustomerUS customer)
        {
            TableOperation insert = TableOperation.Insert(customer);

            table.Execute(insert);
        }

        static CustomerUS GetCustomer(CloudTable table, string partitionKey, string rowKey)
        {
            TableOperation retrieve = TableOperation.Retrieve<CustomerUS>(partitionKey, rowKey);

            var result = table.Execute(retrieve);

            return (CustomerUS) result.Result;
        }

        static void GetAllCustomers(CloudTable table)
        {
            TableQuery<CustomerUS> query = new TableQuery<CustomerUS>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "US"));

            foreach (CustomerUS customer in table.ExecuteQuery(query))
            {
                Console.WriteLine(customer.Name);
            }

        }

        static void UpdateCustomer(CloudTable table, CustomerUS customer)
        {
            TableOperation update = TableOperation.Replace(customer);

            table.Execute(update);
        }

        static void DeleteCustomer(CloudTable table, CustomerUS customer)
        {
            TableOperation delete = TableOperation.Delete(customer);

            table.Execute(delete);
        }
    }
}
