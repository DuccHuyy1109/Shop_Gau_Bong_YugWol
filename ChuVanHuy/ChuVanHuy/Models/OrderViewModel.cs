using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChuVanHuy.Models
{
    public class OrderViewModel
    {
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Không được để trống thông tin")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Không được để trống thông tin")]
        public string Address { get; set; }
        public string Email { get; set; }
        public int TypePayment { get; set; }

    }
}