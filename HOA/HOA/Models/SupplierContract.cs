namespace HOA.Models
{
    public class SupplierContract
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public string Category { get; set; } // e.g., Water, Electricity, etc.
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
    }
}