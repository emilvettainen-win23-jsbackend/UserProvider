using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserProvider.Helpers.Responses;
using UserProvider.Helpers.Validations;
using UserProvider.Models;
using UserProvider.Services;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace UserProvider.Functions
{
    public class CreateUser
    {
        private readonly ILogger<CreateUser> _logger;
        private readonly UserService _userService;
        private readonly ServiceBusClient _client;
       

        public CreateUser(ILogger<CreateUser> logger, UserService userService, ServiceBusClient client)
        {
            _logger = logger;
            _userService = userService;
            _client = client;
        }

        [Function("CreateUser")]
        public async Task <IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "user")] HttpRequest req, [FromBody] CreateUserModel model)
        {
            try
            {
                var modelState = CustomValidation.ValidateModel(model);
                if (!modelState.IsValid)
                {
                    return new BadRequestResult();
                }

                var result = await _userService.CreateUserAsync(model);
                switch (result.StatusCode)
                {
                    case ResultStatus.OK:
                        var sender = _client.CreateSender("verification_request");
                        var messages = new ServiceBusMessage(JsonConvert.SerializeObject(new RequestModel { Email = model.Email }));
                        await sender.SendMessageAsync(messages);
                        return new OkResult();

                    case ResultStatus.EXISTS:
                        return new ConflictResult();

                    default:
                        return new BadRequestResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR : CreateUser.CreateUser() :: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
}