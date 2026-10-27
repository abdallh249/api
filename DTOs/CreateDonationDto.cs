using System.ComponentModel.DataAnnotations;

namespace MyApi.DTOs;

public class CreateDonationDto
{
    [Range(typeof(decimal), "0.01", "9999999999999999.99")]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(200)]
    public string DonorName { get; set; } = "";

    public bool IsAnonymous { get; set; }
}