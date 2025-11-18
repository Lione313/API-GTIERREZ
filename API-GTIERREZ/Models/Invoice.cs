namespace API_GTIERREZ.Models
{
    public class Invoice
    {

        public int InvoiceID { get; set; }
        public int CustomerID { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNumber { get; set; }
        public float Total { get; set; }
        public bool Active { get; set; }

      
        public Customer Customer { get; set; }
        public ICollection<Detail> Details { get; set; }
    }
}
