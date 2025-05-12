namespace HOA.Models;

public partial class Payment
{
    public int Id { get; set; }

    public string ResidentName { get; set; }

    public int Apartment { get; set; }

    public DateOnly PaymentDate { get; set; }

    public float Amount { get; set; }

    public string Status { get; set; }
}