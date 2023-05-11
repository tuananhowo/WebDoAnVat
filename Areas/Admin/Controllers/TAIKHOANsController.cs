using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Web.Models;

namespace Web.Areas.Admin.Controllers
{
    public class TAIKHOANsController : Controller
    {
        private webdoanvat db = new webdoanvat();

        // GET: Admin/TAIKHOANs
        public ActionResult Index()
        {
            var m = Convert.ToInt32(Session["PQAdmin"]);
            if (Session["ID_TKAdmin"] != null && m != 3)
            {
                return View(db.TAIKHOANs.Where(g => g.TinhTrang == 1).ToList());
            }
            else
            {
                return Redirect("~/Admin/Home/Login");
            }

        }

        // GET: Admin/TAIKHOANs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
            if (tAIKHOAN == null)
            {
                return HttpNotFound();
            }
            return View(tAIKHOAN);
        }

        // GET: Admin/TAIKHOANs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TAIKHOANs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_TK,TenDangNhap,MatKhau,HoTen,Email,SDT,NgaySinh,GioiTinh,DiaChi,TinhTrang,PhanQuyen,NgayTao,HinhAnh")] TAIKHOAN tAIKHOAN)
        {
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
                tAIKHOAN.PhanQuyen = Convert.ToInt32(Request["phanquyen"]);
                tAIKHOAN.GioiTinh = Request["GioiTinh"];
                tAIKHOAN.NgayTao = DateTime.Now;
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

        // GET: Admin/TAIKHOANs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
            if (tAIKHOAN == null)
            {
                return HttpNotFound();
            }
            return View(tAIKHOAN);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_TK,TenDangNhap,MatKhau,HoTen,Email,SDT,NgaySinh,GioiTinh,DiaChi,TinhTrang,PhanQuyen,NgayTao,HinhAnh")] TAIKHOAN tAIKHOAN)
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
                        f.SaveAs(UploadPath);
                        tAIKHOAN.HinhAnh = FileName;
                    }
                    tAIKHOAN.TinhTrang = 1;
                    tAIKHOAN.NgaySinh = Convert.ToDateTime(Request["NgaySinh"]);
                    tAIKHOAN.GioiTinh = Request["GioiTinh"];
                    db.Entry(tAIKHOAN).State = EntityState.Modified;
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

        // GET: Admin/TAIKHOANs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
            if (tAIKHOAN == null)
            {
                return HttpNotFound();
            }
            return View(tAIKHOAN);
        }

        // POST: Admin/TAIKHOANs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
            tAIKHOAN.TinhTrang = 2;
            db.TAIKHOANs.AddOrUpdate(tAIKHOAN);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //in


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
