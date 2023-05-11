using System;
using System.Linq;
using System.Web.Mvc;
using Web.Models;

namespace Web.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: Admin/ThongKe
        private webdoanvat db = new webdoanvat();
        public ActionResult Index()
        {
            var m = Convert.ToInt32(Session["PQAdmin"]);
            if (Session["ID_TKAdmin"] != null && m != 3)
            {
                var list = db.SANPHAMs.AsQueryable().ToList();
                return View(list);
            }
            else
            {
                return Redirect("~/Admin/Home/Login");
            }

        }
    }
}