using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using UserProvider.Infrastructure.Helpers.Responses;
using UserProvider.Infrastructure.Services;

namespace UserProvider.Functions.Http;

public class GetUsers(ILogger<GetUsers> logger, UserService userService)
{
    private readonly ILogger<GetUsers> _logger = logger;
    private readonly UserService _userService = userService;

    [Function("GetUsers")]
    public async Task <IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route ="users/all")] HttpRequest req)
    {
        try
        {
            var result = await _userService.GetAllUsersAsync();
            return result.StatusCode switch
            {
                ResultStatus.OK => new OkObjectResult(result.ContentResult),
                ResultStatus.NOT_FOUND => new NotFoundResult(),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : GetUsers.Run() :: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}