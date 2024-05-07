using System.ComponentModel.DataAnnotations;

namespace UserProvider.Models;

public class CreateUserModel
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [RegularExpression(@"^(([^<>()\]\\.,;:\s@\""]+(\.[^<>()\]\\.,;:\s@\""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$", ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = null!;

    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, numeric, and special characters.")]
    public string Password { get; set; } = null!;
}