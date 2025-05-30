using System.ComponentModel.DataAnnotations;

namespace WebApi.Entity;

public class ProfileEntity
{
    [Key]
    public string Id { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = string.Empty;

    // Address
    public string Street { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}