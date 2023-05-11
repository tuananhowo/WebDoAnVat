using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class CartItem
    {
        public SANPHAM product { get; set; }
        public int Quantity { get; set; }
    }
    public class Cart
    {
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }
        public void Add(SANPHAM _sanpham, int _soluong = 1)
        {
            var item = items.FirstOrDefault(s => s.product.ID_SP == _sanpham.ID_SP);
            if (item == null)
            {
                items.Add(new CartItem
                {
                    product = _sanpham,
                    Quantity = _soluong
                });

            }
            else
            {
                item.Quantity += _soluong;
            }
        }
        public void UpdateSoluong(int id, int _soluong)
        {
            var item = items.Find(s => s.product.ID_SP == id);
            if (item != null)
            {
                item.Quantity = _soluong;
            }
        }
        public void Xoa(int id)
        {
            var item = items.Find(s => s.product.ID_SP == id);
            items.Remove(item);
        }
        public double total_money()
        {
            var total = items.Sum(s => (double) s.product.GiaTien * (100 - s.product.GiamGia) * 0.01*s.Quantity);
            return (double)total;
        }
        
    }
}