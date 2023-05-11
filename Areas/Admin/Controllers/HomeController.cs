using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private webdoanvat db = new webdoanvat();
        // GET: Admin/Home
        public ActionResult Index(string TuNgay = "", string DenNgay = "")
        {
            var m = Convert.ToInt32(Session["PQAdmin"]);
            if (Session["ID_TKAdmin"] != null && m != 3)
            {
                if (String.IsNullOrEmpty(TuNgay) || String.IsNullOrEmpty(DenNgay))
                {
                    var emp = (from s in db.SANPHAMs
                               join g in db.GIOHANGs on s.ID_SP equals g.ID_SP
                               where g.NgayDatHang.Value.Month == DateTime.Now.Month
                               select new { g.TinhTrang, s.GiaTien, g.SoLuong, g.NgayDatHang }).GroupBy(g => g.TinhTrang).Select(g => new BieuDo
                               {
                                   TinhTrangBD = g.Key,
                                   SoTien = g.Sum(x => x.GiaTien * x.SoLuong),
                               }).ToList();
                    return View(emp);
                }
                else
                {
                    var tn = Convert.ToDateTime(TuNgay);
                    var dn = Convert.ToDateTime(DenNgay);
                    var emp = (from s in db.SANPHAMs
                               join g in db.GIOHANGs on s.ID_SP equals g.ID_SP
                               where (g.NgayDatHang >= tn && g.NgayDatHang <= dn)
                               select new { g.TinhTrang, s.GiaTien, g.SoLuong, g.NgayDatHang }).GroupBy(g => g.TinhTrang).Select(g => new BieuDo
                               {
                                   TinhTrangBD = g.Key,
                                   SoTien = g.Sum(x => x.GiaTien * x.SoLuong),
                               }).ToList();
                    return View(emp);
                }


            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        //đăng nhập, đăng xuất
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string TenDangNhap, string MatKhau)
        {
            if (ModelState.IsValid)
            {
                var user1 = db.TAIKHOANs.FirstOrDefault(u => u.TenDangNhap.Equals(TenDangNhap) && u.MatKhau.Equals(MatKhau));
                if (user1 != null)
                {
                    var newCookie = new HttpCookie("myCookieAdmin", user1.ID_TK.ToString());
                    newCookie.Expires = DateTime.Now.AddDays(10);
                    Response.AppendCookie(newCookie);
                    if (user1.PhanQuyen == 1 || user1.PhanQuyen == 2)
                    {
                        Session["HoTenAdmin"] = user1.HoTen;
                        Session["EmailAdmin"] = user1.Email;
                        Session["ID_TKAdmin"] = user1.ID_TK;
                        Session["HinhAnhAdmin"] = user1.HinhAnh;
                        Session["PQAdmin"] = user1.PhanQuyen;
                        ViewBag.PQ = user1.PhanQuyen;
                        return Redirect("~/Admin/Home/Index");
                    }
                    else
                    {
                        ViewBag.error = "Thông tin đăng nhập không hợp lệ!!!";
                    }
                }

                else
                {
                    ViewBag.error = "Thông tin đăng nhập không hợp lệ!!!";
                }

            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            if (Request.Cookies["myCookieAdmin"] != null)
            {
                //Fetch the Cookie using its Key.
                HttpCookie nameCookie = Request.Cookies["myCookieAdmin"];

                //Set the Expiry date to past date.
                nameCookie.Expires = DateTime.Now.AddDays(-1);

                //Update the Cookie in Browser.
                Response.Cookies.Add(nameCookie);

                //Set Message in TempData.
                TempData["Message"] = "Cookie deleted.";
            }
            return RedirectToAction("Login");
        }
        public ActionResult QlDanhMuc()
        {
            if (Session["ID_TKAdmin"] != null)
            {
                var danhmuc = db.DANHMUCs.Select(h => h);

                return View(danhmuc);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

    }
}