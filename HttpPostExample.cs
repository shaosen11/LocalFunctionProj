using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

// When the trigger parameter is an HttpRequestData or an HttpRequest, custom types can also
// be bound to additional parameters using Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute.
// Use of this attribute requires Microsoft.Azure.Functions.Worker.Extensions.Http version 3.1.0
// or later.
// dotnet add package Microsoft.Azure.Functions.Worker.Extensions.Http --version 3.1.0

using System.Text.Json.Serialization;

// using Newtonsoft.Json;

namespace LocalFunctionProj
{
    public class PetDetails
    {
        [JsonConstructor]
        public PetDetails() 
        {
            PetName = string.Empty;
            PetAge = 0;
        }

        [JsonPropertyName("name")]
        public string PetName { get; set; }
        [JsonPropertyName("age")]
        public int PetAge { get; set; }
    }

    public class HttpPostExample
    {
        private readonly ILogger _logger;

        public HttpPostExample(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpPostExample>();
        }

        [Function("HttpPostExample")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "post",
            Route = "pet/{species:alpha}")] HttpRequestData req, [FromBody] PetDetails details, 
            string species)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString($"Added a new pet: species {species} name {details.PetName} age {details.PetAge}.");

            return response;
        }
    }
}
