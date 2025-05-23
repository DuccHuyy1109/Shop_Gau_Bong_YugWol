﻿using ChuVanHuy.Models;
using ChuVanHuy.Models.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuVanHuy.Controllers
{
    public class GiohangCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: GiohangCart
        public ActionResult Index()
        {
            GiohangCart cart = (GiohangCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        public ActionResult CheckOut()
        {
            GiohangCart cart = (GiohangCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }

        public ActionResult CheckOutSuccess()
        {

            return View();
        }

        public ActionResult Partial_Item_ThanhToan()
        {
            GiohangCart cart = (GiohangCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }

        public ActionResult Partial_Item_Cart()
        {
            GiohangCart cart = (GiohangCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }




        public ActionResult ShowCount()
        {
            GiohangCart cart = (GiohangCart)Session["Cart"];
            if(cart != null)
            {
                return Json(new { Count = cart.Items.Count },JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var code = new { Success = false, Code = -1 };
            if (ModelState.IsValid)
            {
                GiohangCart cart = (GiohangCart)Session["Cart"];
                if (cart != null)
                {
                    Order order = new Order();
                    order.CustomerName = req.CustomerName; 
                    order.Phone = req.Phone;
                    order.Address = req.Address;
                    //order.Email = req.CustomerName;
                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = x.Prite
                    }));
                    order.TotalAmount = cart.Items.Sum(x => (x.Prite * x.Quantity));
                    order.TypePayment = req.TypePayment;
                    order.CreatedDate = DateTime.Now;
                    order.ModifiedDate = DateTime.Now;
                    order.CreatedBy = req.Phone;
                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    db.Orders.Add(order);
                    db.SaveChanges();
                    //send mail cho kach hang
                    var strSanPham= "";
                    var thanhtien = decimal.Zero;
                    var TongTien = decimal.Zero;
                    foreach (var sp in cart.Items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>"+sp.ProductName+"</td>";
                        strSanPham += "<td>" + "https://play.google.com/store/games?hl=vi&gl=US" + "</td>";
                        strSanPham += "<td>"+ChuVanHuy.Common.Common.FormatNumber(sp.TotalPrice,0)+"</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Prite * sp.Quantity;
                    }

                    TongTien = thanhtien;
                    string contentCustomer =System.IO.File.ReadAllText( Server.MapPath("~/Content/templates/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
                    contentCustomer = contentCustomer.Replace("{{DiaChi}}", order.Address); 
                     contentCustomer = contentCustomer.Replace("{{ThanhTien}}", ChuVanHuy.Common.Common.FormatNumber(thanhtien,0));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", ChuVanHuy.Common.Common.FormatNumber(TongTien, 0));
                    ChuVanHuy.Common.Common.SendMail("YugWolGaming", "Đơn Hàng #" + order.Code, contentCustomer.ToString(), req.Email);
                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", order.Code);
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", order.Phone);
                    contentAdmin = contentAdmin.Replace("{{Email}}", req.Email);
                    contentAdmin = contentAdmin.Replace("{{DiaChi}}", order.Address);
                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", ChuVanHuy.Common.Common.FormatNumber(thanhtien, 0));
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", ChuVanHuy.Common.Common.FormatNumber(TongTien, 0));
                    ChuVanHuy.Common.Common.SendMail("YugWolGaming", "Đơn Hàng Mới #" + order.Code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);


                    cart.ClearCart();
                    return RedirectToAction("CheckOutSuccess");

                }
            }
            return Json(code);
        }


        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            var db = new ApplicationDbContext();
            var checkProduct = db.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                GiohangCart cart = (GiohangCart)Session["Cart"];
                if(cart == null)
                {
                    cart = new GiohangCart();
                }
                GiohangCartItem item = new GiohangCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Alias=checkProduct.Alias,
                    Quantity = quantity
                };
                if (checkProduct.ProductImages.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImages.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Prite = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Prite = (decimal)checkProduct.PriceSale;
                }
                item.TotalPrice = item.Quantity * item.Prite;
                cart.AddToCard(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = true, msg = "Game đã được thêm vào giỏi hàng 🥊 ", code = 1, Count = cart.Items.Count };
            }
            return Json(code);
        }


        

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 }; 
            GiohangCart cart = (GiohangCart)Session["Cart"];
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
                }
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult DeleteAll()
        {
            GiohangCart cart = (GiohangCart)Session["Cart"];
            if(cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
    }
}