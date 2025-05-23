﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChuVanHuy
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "ChuVanHuy.Controllers" }
            );

            routes.MapRoute(
                name: "CheckOut",
                url: "thanh-toan",
                defaults: new { controller = "GiohangCart", action = "CheckOut", alias = UrlParameter.Optional },
                namespaces: new[] { "ChuVanHuy.Controllers" }
            );

            routes.MapRoute(
                name: "GiohangCart",
                url: "gio-hang",
                defaults: new { controller = "GiohangCart", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "ChuVanHuy.Controllers" }
            );

            routes.MapRoute(
                name: "CategoryProduct",
                url: "danh-muc-game/{alias}-{id}",
                defaults: new { controller = "Products", action = "ProductCategory", id = UrlParameter.Optional },
                namespaces: new[] { "ChuVanHuy.Controllers" }
            );

            routes.MapRoute(
                name: "detailProduct",
                url: "chi-tiet/{alias}-p{id}",
                defaults: new { controller = "Products", action = "Detail", alias = UrlParameter.Optional },
                namespaces: new[] { "ChuVanHuy.Controllers" }
            );

            routes.MapRoute(
                name: "Products",
                url: "game",
                defaults: new { controller = "Products", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "ChuVanHuy.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "ChuVanHuy.Controllers" }
            );
        }
    }
}
