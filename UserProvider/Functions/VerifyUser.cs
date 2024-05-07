using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using UserProvider.Helpers.Responses;
using UserProvider.Helpers.Validations;
using UserProvider.Models;
using UserProvider.Services;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace UserProvider.Functions
{
    public class VerifyUser
    {
        private readonly ILogger<VerifyUser> _logger;
        private readonly UserService _userService;

        public VerifyUser(ILogger<VerifyUser> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [Function("VerifyUser")]
        public async Task <IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req, [FromBody] VerifyUserModel model)
        {
            try
            {
                var modelState = CustomValidation.ValidateModel(model);
                if (!modelState.IsValid)
                {
                    return new BadRequestResult();
                }
                var result = await _userService.VerifyUserAsync(model);
                return result.StatusCode switch
                {
                    ResultStatus.OK => new OkResult(),
                    ResultStatus.UNAUTHORIZED => new UnauthorizedResult(),
                    _=> new BadRequestResult()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR : VerifyUser.Run() :: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
}
