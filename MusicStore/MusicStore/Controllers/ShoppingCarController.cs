using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Models;

namespace MusicStore.Controllers
{
    [Authorize]

    public class ShoppingCarController : Controller
    {
            // GET: ShoppingCar
        MusicStoreEntity db = new MusicStoreEntity();
        public ActionResult Index()
        {
           
            var cartList= db.Carts.Where(a => a.CartId == User.Identity.Name).ToList();  //登陆的用户，这个User 可以在控制器和视图里使用
            decimal total = 0;
            foreach (var item in cartList)
            {
                total = total + item.Count * item.Albums.Price;
            }
            ViewBag.CartTotal = total;
            return View(cartList);
        }
        public ActionResult AddToCart(int id,int count)
        {
            int i = 0;
            var album = db.Albums.Find(id);

            if(album!=null)  //如果没有判断，就会出现未将对象
            {
                var cartItem=db.Carts.SingleOrDefault(p => p.AlbumId == id && p.CartId == User.Identity.Name);
                if (cartItem != null)      
                {
                    cartItem.Count+=count;
                }
                else
                {
                    cartItem = new Carts()
                    {
                        AlbumId = id,
                        CartId = User.Identity.Name,
                        Count = 1,
                        DateCreated=DateTime.Now
                        
                    };
                    db.Carts.Add(cartItem);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int id)
        {
            var cartItem =db.Carts.SingleOrDefault(p=>p.RecordId==id && p.CartId ==User.Identity.Name);
            if (cartItem != null)
            {
                db.Carts.Remove(cartItem);
                db.SaveChanges();
            }
            var result = new
            {
                ItemID = id,
              CartTotal = ShoppingCart.CartTotal(),
              CartCount = ShoppingCart.GetCartCount()
        };
            return Json(result);
        }
        public ActionResult UpdateItemCart(int id,int count)
        {
            var cartItem = db.Carts.SingleOrDefault(p => p.RecordId == id && p.CartId == User.Identity.Name);
            if (cartItem != null)
            {

                cartItem.Count = count;
               db.SaveChanges();
            }

            var result = new
            {
                ItemID = id,
                CartTotal = ShoppingCart.CartTotal(),
                CartCount = ShoppingCart.GetCartCount()
            };
            return Json(result);
        }

        [AllowAnonymous]  //允许匿名访问

        public ActionResult GetCartSummary()
        {
            ViewBag.Count = ShoppingCart.GetCartCount();
            return PartialView("CartSummary");
        }

    }
}