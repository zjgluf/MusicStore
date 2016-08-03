using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Models;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Models.MusicStoreEntity DB = new Models.MusicStoreEntity();
            List<Albums> list=DB.Albums.OrderByDescending(a=>a.OrderDetails.Count).ToList();//按照字段AlbumID降序排列
            return View(list);
        }

        private int Order(Albums b)
        {
            return b.AlbumId;
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            ViewData["Message"] = "aaaa";
            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}