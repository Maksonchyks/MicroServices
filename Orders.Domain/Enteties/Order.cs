using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Domain.Enteties
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public int? PromotionId { get; set; }
        public Promotion? Promotion { get; set; }

        public List<OrderItem> Items { get; set; } = new();
        public List<OrderStatusHistory> StatusHistory { get; set; } = new();
        public List<PaymentRecord> Payments { get; set; } = new();
    }
}
