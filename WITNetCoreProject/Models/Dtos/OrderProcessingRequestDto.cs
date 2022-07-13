using System;
namespace WITNetCoreProject.Models.Dtos
{
    public class OrderProcessingRequestDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
}
