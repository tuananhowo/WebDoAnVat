using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Web.Models;

namespace Web.Areas.Admin.Controllers
{
    public class DANHMUCsController : Controller
    {
        private webdoanvat db = new webdoanvat();

        // GET: Admin/DANHMUCs
        public ActionResult Index()
        {
            var m = Convert.ToInt32(Session["PQAdmin"]);
            if (Session["ID_TKAdmin"] != null && m != 3)
            {
                return View(db.DANHMUCs.ToList());
            }
            else
            {
                return Redirect("~/Admin/Home/Login");
            }

        }

        // GET: Admin/DANHMUCs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DANHMUC dANHMUC = db.DANHMUCs.Find(id);
            if (dANHMUC == null)
            {
                return HttpNotFound();
            }
            return View(dANHMUC);
        }

        // GET: Admin/DANHMUCs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/DANHMUCs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_DanhMuc,TenDanhMuc,GhiChu,AnhBiaDM")] DANHMUC dANHMUC)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    dANHMUC.AnhBiaDM = "";
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
                        dANHMUC.AnhBiaDM = FileName;
                    }
                    dANHMUC.KieuDM = Convert.ToInt32(Request["KieuDM"]);
                    db.DANHMUCs.Add(dANHMUC);
                    db.SaveChanges();
                    //SetAlter("Thêm thành công", "success");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;

                return View(dANHMUC);

            }
        }

        // GET: Admin/DANHMUCs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DANHMUC dANHMUC = db.DANHMUCs.Find(id);
            if (dANHMUC == null)
            {
                return HttpNotFound();
            }
            return View(dANHMUC);
        }

        // POST: Admin/DANHMUCs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_DanhMuc,TenDanhMuc,GhiChu,AnhBiaDM")] DANHMUC dANHMUC)
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
                        dANHMUC.AnhBiaDM = FileName;
                    }
                    dANHMUC.KieuDM = Convert.ToInt32(Request["KieuDM"]);
                    db.Entry(dANHMUC).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Nhập thông tin lỗi" + ex.Message;

                return View(dANHMUC);
            }
        }

        // GET: Admin/DANHMUCs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DANHMUC dANHMUC = db.DANHMUCs.Find(id);
            if (dANHMUC == null)
            {
                return HttpNotFound();
            }
            return View(dANHMUC);
        }

        // POST: Admin/DANHMUCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DANHMUC dANHMUC = db.DANHMUCs.Find(id);
            List<SANPHAM> m = db.SANPHAMs.Where(g => g.ID_DanhMuc == id).ToList();
            foreach (var item in m)
            {
                db.SANPHAMs.Remove(item);
            }
            db.DANHMUCs.Remove(dANHMUC);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Thongtin(int? id)
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
        public ActionResult Thongtin([Bind(Include = "ID_TK,TenDangNhap,MatKhau,HoTen,Email,SDT,NgaySinh,GioiTinh,DiaChi,TinhTrang,PhanQuyen,NgayTao,HinhAnh")] TAIKHOAN tAIKHOAN)
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
                        Session["HinhAnhAdmin"] = FileName;
                        f.SaveAs(UploadPath);
                        tAIKHOAN.HinhAnh = FileName;
                    }
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
    }
}
