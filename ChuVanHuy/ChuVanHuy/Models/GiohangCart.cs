using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChuVanHuy.Models
{
    public class GiohangCart
    {
        public List<GiohangCartItem> Items { get; set; }
        public GiohangCart()
        {
            this.Items = new List<GiohangCartItem>();
        }

    public void AddToCard(GiohangCartItem item,int Quantity)
        {
            var checkExits = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (checkExits != null)
            {
                checkExits.Quantity += Quantity;
                checkExits.TotalPrice = checkExits.Prite * checkExits.Quantity;
            }
            else 
            {
                Items.Add(item);
            }
        }


        public void Remove(int id)
            {
            var checkExits = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                Items.Remove(checkExits);
            }
        }

        public void UpdataQuantity(int id, int quantity)
        {
            var checkExits = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                checkExits.Quantity = quantity;
                checkExits.TotalPrice = checkExits.Prite * checkExits.Quantity;
            }  
                
         }

        public decimal GetTotalPrice()
        {
            return Items.Sum(x => x.TotalPrice);
        }


        public int GetTotalQuantity()
        {
            return Items.Sum(x => x.Quantity);
        }

        public void ClearCart()
        {
            Items.Clear();
        }

    }



    public class GiohangCartItem 
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Alias { get; set; }
        public string CategoryName { get; set; }
        public string ProductImg { get; set; }
        public int Quantity { get; set; }
        public decimal Prite { get; set; }
        public decimal TotalPrice { get; set; }

    }
}