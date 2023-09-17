namespace WebAppForGame.ViewModels;

public class PaymentsViewModel
{
    public Guid Id { get; set; }

    public string? UserID { get; set; }
    public ProductsViewModel? Product { get; set; }
    public string? PaymentId { get; set; }
    public string? PaymentStatus { get; set; }
    public DateTime Date { get; set; }
}