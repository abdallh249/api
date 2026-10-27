namespace MyApi.Models;

public class Donation
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string DonorName { get; set; } = "";
    public bool IsAnonymous { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}