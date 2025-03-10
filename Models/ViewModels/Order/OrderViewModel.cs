namespace Models.ViewModels.Order
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public string OrderCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }

        public decimal TotalAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
    }
}
