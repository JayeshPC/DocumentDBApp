using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;


namespace DocumentDBApp
{
    class Program
    {

        private const string EndpointUrl = "https://jaycosmosdb.documents.azure.com:443/";

        private const string AuthorizationKey = "9zryibbuseFEFvsYT4kPCfSahjV4BqgNjYM3AeT6s9RYXMKQzIz47zBNrY5hHSh0fzd1xrPVgjPxWNzZgjEAmA==";
			
        static void Main(string[] args)
        {
            try
            {
                CreateDocumentClient().Wait();
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            Console.ReadKey();

        }


        private static async Task CreateDocumentClient()
        {
            // Create a new instance of the DocumentClient
            var client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
            {
                // Create a Database for DocumentDB Using .Net SDK
                await CreateDatabase(client);

                // TO LIST DATABASES 
                //GetDatabases(client);

                // TO DELETE DATABASE
                //GetDatabases(client);
                //await DeleteDatabase(client);
                //GetDatabases(client);

            }
        }


        private async static Task CreateDatabase(DocumentClient client)
        {
            Console.WriteLine();
            Console.WriteLine("******** Create Database *******");

            var databaseDefinition = new Database { Id = "mynewdb" };
            var result = await client.CreateDatabaseAsync(databaseDefinition);
            var database = result.Resource;

            Console.WriteLine(" Database Id: {0}; Rid: {1}", database.Id, database.ResourceId);
            Console.WriteLine("******** Database Created *******");
        }


        private static void GetDatabases(DocumentClient client)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("******** Get Databases List ********");

            var databases = client.CreateDatabaseQuery().ToList();

            foreach (var database in databases)
            {
                Console.WriteLine(" Database Id: {0}; Rid: {1}",
                   database.Id, database.ResourceId);
            }

            Console.WriteLine();
            Console.WriteLine("Total databases: {0}", databases.Count);
        }

        private async static Task DeleteDatabase(DocumentClient client)
        {
            Console.WriteLine();
            Console.WriteLine("******** Delete Database ********");

            Database database = client
               .CreateDatabaseQuery("SELECT * FROM c WHERE c.id = 'tempdb1'")
               .AsEnumerable()
               .First();
            await client.DeleteDatabaseAsync(database.SelfLink);
        }

    }
}
