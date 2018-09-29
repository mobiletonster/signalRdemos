using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using BrainBuffet.Models;
using Microsoft.Extensions.Configuration;

namespace BrainBuffet.Services
{
    public class CosmosQuestionService
    {
        private string EndpointUri;
        private string PrimaryKey;
        private DocumentClient client;

        public CosmosQuestionService(IConfiguration configuration)
        {
            EndpointUri = configuration.GetSection("Cosmos").GetValue<string>("EndpointUri");
            PrimaryKey = configuration.GetSection("Cosmos").GetValue<string>("PrimaryKey");
            this.client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
            Init();
        }

        private async void Init()
        {
            await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "BrainBuffet" });
            await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("BrainBuffet"), new DocumentCollection { Id = "Jeopardy" });
        }

        public ActionResult<Jeopardy> GetQuestionById(int id)
        {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            var url = UriFactory.CreateDocumentCollectionUri("BrainBuffet","Jeopardy");


            // this.client.CreateDocumentQuery<Jeopardy>();
            // Now execute the same query via direct SQL
            var queryResult = this.client.CreateDocumentQuery<Jeopardy>(
                    url,
                    $"SELECT * FROM Jeopardy J WHERE J.id = '{id}'",
                    queryOptions).AsEnumerable();

            return queryResult.First();
        }

        public int GetQuestionCount()
        {
            return 30000;
            // return _context.Questions.Max(m => m.Id);
        }
    }
}
