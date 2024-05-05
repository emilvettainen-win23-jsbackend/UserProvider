using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserProvider.Data.Entities;

public class UserEntity : IdentityUser
{
    [ProtectedPersonalData]
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; } = null!;

    [ProtectedPersonalData]
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; } = null!;

    public string? Biography { get; set; }
    public string? ProfileImageUrl { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime Created { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime Modified { get; set; }

    public bool IsExternalAccount { get; set; } = false;
}
