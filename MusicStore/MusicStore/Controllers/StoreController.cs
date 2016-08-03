using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Models;
using X.PagedList;


namespace MusicStore.Controllers
{

    //[Authorize]
    public class StoreController : Controller
    {
        MusicStoreEntity DB = new MusicStoreEntity();
        // GET: Store
       // [Authorize(Roles ="admin.user")]
        public ActionResult Index(string search,int page=1)
        {
            var pageSize = 16;
            IPagedList<Albums> list = null;
            if (string.IsNullOrEmpty(search))
            {
             list = DB.Albums.OrderByDescending(a => a.AlbumId).ToPagedList(page, pageSize);
            }
            else
            {
                ViewBag.search = search;//分页数据
             list = DB.Albums.Where(a=>a.Title.Contains(search)).OrderByDescending(a => a.AlbumId).ToPagedList(page, pageSize);
            }
                       
            return View(list);
        }
        public ActionResult Details(int id)
        {

            Albums a = DB.Albums.Find(id);
            return View(a);
        }
    }
}