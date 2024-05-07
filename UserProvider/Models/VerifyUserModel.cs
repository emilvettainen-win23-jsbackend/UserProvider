namespace UserProvider.Models;

public class VerifyUserModel
{
    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;
}
