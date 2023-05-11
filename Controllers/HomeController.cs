using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private webdoanvat db = new webdoanvat();
        // GET: Home
        public ActionResult Index()
        {
            var m1 = db.Banners.FirstOrDefault(g => g.ThuTu == 1).LinkBanner;
            var m2 = db.Banners.FirstOrDefault(g => g.ThuTu == 2).LinkBanner;
            var m3 = db.Banners.FirstOrDefault(g => g.ThuTu == 3).LinkBanner;
            if (m1 != null || m1 != null || m2 != null)
            {
                Session["Banner1"] = m1;
                Session["Banner2"] = m2;
                Session["Banner3"] = m3;
            }
            ViewData["hinhanh"] = db.Banners.AsNoTracking().Where(g => g.ThuTu == 1).ToList();
            return View();
        }
        public PartialViewResult ListDanhMuc()
        {
            var danhmuc = db.DANHMUCs.Select(n => n);
            return PartialView(danhmuc);
        }
        public PartialViewResult SanPhamMoi()
        {
            List<SANPHAM> sanphams = new List<SANPHAM>();

            //sanphams = db.SANPHAMs.Select(h => h).ToList();
            var sANPHAMs = db.SANPHAMs.OrderByDescending(h => h.ID_SP).Select(h => h).Take(6);

            return PartialView(sANPHAMs);
        }
        public PartialViewResult TopGiamGia(string id)
        {
            List<SANPHAM> sanphams = new List<SANPHAM>();

            var sANPHAMs = db.SANPHAMs.OrderByDescending(h => h.GiamGia).Select(h => h).Take(6);

            return PartialView(sANPHAMs);
        }
        public ActionResult ListSanPham()
        {
            List<SANPHAM> sp = Session["SP"] as List<SANPHAM>;
            var so = 0;
            if (sp == null)
            {
                so = 0;
            }
            else
            {
                so = sp.Count;
            }
            ViewBag.sosp = so;
            return View();
        }
        public PartialViewResult AllSanPham(string id, string keysearch = "")
        {
            List<SANPHAM> sanphams = new List<SANPHAM>();
            if (id == null)
            {
                sanphams = db.SANPHAMs.Select(h => h).ToList();
            }

            else
            {
                sanphams = db.SANPHAMs.Where(h => h.ID_DanhMuc.ToString().Equals(id)).Select(h => h).ToList();
                Session["SP"] = sanphams;
            }
            if (keysearch != "")
            {
                sanphams = db.SANPHAMs.Where(h => h.TenSanPham.ToLower().Contains(keysearch.ToLower())).ToList();
            }
            return PartialView(sanphams);
        }
        public PartialViewResult ShowListDanhMuc()
        {
            var danhmuc = db.DANHMUCs.Select(n => n);
            return PartialView(danhmuc);
        }
        //chi tiết sản phẩm
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sanphams = db.SANPHAMs.SingleOrDefault(m => m.ID_SP == id);
            if (sanphams == null)
            {
                return HttpNotFound();
            }
            return View(sanphams);
        }
        public ActionResult XemChiTiet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(m => m.ID_SP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
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
                    var newCookie = new HttpCookie("myCookie", user1.ID_TK.ToString());
                    newCookie.Expires = DateTime.Now.AddDays(10);
                    Response.AppendCookie(newCookie);
                    if (user1.PhanQuyen == 3)
                    {
                        Session["HoTen"] = user1.HoTen;
                        Session["Email"] = user1.Email;
                        Session["ID_TK"] = user1.ID_TK;
                        Session["HinhAnh"] = user1.HinhAnh;
                        Session["PQ"] = user1.PhanQuyen;
                        return Redirect("~/Home/Index");
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
            if (Request.Cookies["myCookie"] != null)
            {
                //Fetch the Cookie using its Key.
                HttpCookie nameCookie = Request.Cookies["myCookie"];

                //Set the Expiry date to past date.
                nameCookie.Expires = DateTime.Now.AddDays(-1);

                //Update the Cookie in Browser.
                Response.Cookies.Add(nameCookie);

                //Set Message in TempData.
                TempData["Message"] = "Cookie deleted.";
            }
            return RedirectToAction("Login");
        }
        public ActionResult BanDo()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "ID_TK,TenDangNhap,MatKhau,HoTen,Email,NgaySinh,GioiTinh,DiaChi,TinhTrang,PhanQuyen,NgayTao,HinhAnh,SDT")] TAIKHOAN tAIKHOAN)
        {
            var m = Request["NgaySinh"];
            tAIKHOAN.NgaySinh = DateTime.ParseExact(Request["NgaySinh"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            try
            {


                tAIKHOAN.HinhAnh = "";
                tAIKHOAN.NgayTao = DateTime.Now;
                var f = Request.Files["ImageFile"];
                if (f != null && f.ContentLength > 0)
                {
                    //Use  Namespace  called  :	System.IO
                    string FileName = System.IO.Path.GetFileName(f.FileName);
                    //Lấy  tên  file  upload
                    string UploadPath = Server.MapPath("~/wwwoot/dataimg/" + FileName);
                    //Copy  Và  lưu  file  vào  server.
                    f.SaveAs(UploadPath);
                    //Lưu  tên  file  vào  trường
                    tAIKHOAN.HinhAnh = FileName;
                }
                tAIKHOAN.TinhTrang = 1;
                tAIKHOAN.GioiTinh = Request["GioiTinh"];
                tAIKHOAN.PhanQuyen = 3;
                db.TAIKHOANs.Add(tAIKHOAN);
                db.SaveChanges();
                //SetAlter("Thêm thành công", "success");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;

                return View(tAIKHOAN);

            }
        }
        public ActionResult ThongTinCN(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOAN objTaiKhoan = db.TAIKHOANs.Find(id);
            if (objTaiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(objTaiKhoan);
        }
        public ActionResult SuaTT(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOAN objTaiKhoan = db.TAIKHOANs.Find(id);
            if (objTaiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(objTaiKhoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaTT([Bind(Include = "ID_TK,TenDangNhap,MatKhau,HoTen,Email,SDT,NgaySinh,GioiTinh,DiaChi,TinhTrang,PhanQuyen,NgayTao,HinhAnh")] TAIKHOAN tAIKHOAN)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/wwwoot/dataimg/" + FileName);
                        Session["HinhAnh"] = FileName;
                        f.SaveAs(UploadPath);
                        tAIKHOAN.HinhAnh = FileName;
                    }
                    tAIKHOAN.TinhTrang = 1;
                    tAIKHOAN.PhanQuyen = 3;
                    tAIKHOAN.GioiTinh = Request["GioiTinh"];
                    tAIKHOAN.NgaySinh = Convert.ToDateTime(Request["NgaySinh"]);
                    db.TAIKHOANs.AddOrUpdate(tAIKHOAN);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Nhập thông tin lỗi" + ex.Message;

                return View(tAIKHOAN);
            }
        }


    }
}