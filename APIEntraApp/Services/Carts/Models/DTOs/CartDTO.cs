using System.Collections.Generic;

namespace APIEntraApp.Services.Carts.Models.DTOs
{
    public class CartDTO
    {
        public List<ProductCartDTO> ProductCartList { get; set; }
        public decimal              CartTotal       { get; set; }
    }
}
