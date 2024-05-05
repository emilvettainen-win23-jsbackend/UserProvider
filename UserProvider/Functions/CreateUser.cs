using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using UserProvider.Models;

namespace UserProvider.Functions
{
    public class CreateUser
    {
        private readonly ILogger<CreateUser> _logger;

        public CreateUser(ILogger<CreateUser> logger)
        {
            _logger = logger;
        }

        [Function("CreateUser")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "user")] HttpRequest req, [FromBody] CreateUserModel model)
        {
            
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
