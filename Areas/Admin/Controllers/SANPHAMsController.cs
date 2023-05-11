using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Web.Models;

namespace Web.Areas.Admin.Controllers
{
    public class SANPHAMsController : Controller
    {
        private webdoanvat db = new webdoanvat();

        // GET: Admin/SANPHAMs
        public ActionResult Index()
        {
            var m = Convert.ToInt32(Session["PQAdmin"]);
            if (Session["ID_TKAdmin"] != null && m != 3)
            {
                var sANPHAMs = db.SANPHAMs.Include(s => s.DANHMUC);
                return View(sANPHAMs.ToList());
            }
            else
            {
                return Redirect("~/Admin/Home/Login");
            }

        }

        // GET: Admin/SANPHAMs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.FirstOrDefault(g => g.ID_SP == id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            return View(sANPHAM);
        }

        // GET: Admin/SANPHAMs/Create
        public ActionResult Create()
        {
            ViewData["ID_DanhMuc"] = db.DANHMUCs.AsNoTracking().ToList();
            return View();
        }

        // POST: Admin/SANPHAMs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_SP,ID_DanhMuc,TenSanPham,AnhBia,Anh1,Anh2,GiaTien,GiamGia,TinhTrang,SoLuong,MoTa")] SANPHAM sANPHAM)
        {
            try
            {
                sANPHAM.AnhBia = "";
                sANPHAM.Anh1 = "";
                sANPHAM.Anh2 = "";
                var f = Request.Files["ImageFile"];
                var f1 = Request.Files["ImageFile1"];
                var f2 = Request.Files["ImageFile2"];
                if (f != null && f.ContentLength > 0 && f1 != null && f1.ContentLength > 0 && f2 != null && f2.ContentLength > 0)
                {
                    //Use  Namespace  called  :	System.IO
                    string FileName = System.IO.Path.GetFileName(f.FileName);
                    string FileName1 = System.IO.Path.GetFileName(f1.FileName);
                    string FileName2 = System.IO.Path.GetFileName(f2.FileName);
                    //Lấy  tên  file  upload
                    string UploadPath = Server.MapPath("~/wwwoot/dataimg/" + FileName);
                    string UploadPath1 = Server.MapPath("~/wwwoot/dataimg/" + FileName1);
                    string UploadPath2 = Server.MapPath("~/wwwoot/dataimg/" + FileName2);
                    //Copy  Và  lưu  file  vào  server.
                    f.SaveAs(UploadPath);
                    f1.SaveAs(UploadPath1);
                    f2.SaveAs(UploadPath2);
                    //Lưu  tên  file  vào  trường
                    sANPHAM.AnhBia = FileName;
                    sANPHAM.Anh1 = FileName1;
                    sANPHAM.Anh2 = FileName2;
                }
                var soluong = Request["ID_DanhMuc"];
                if (soluong != null)
                {
                    sANPHAM.ID_DanhMuc = Convert.ToInt32(Request["ID_DanhMuc"]);
                }
                else
                {
                    sANPHAM.ID_DanhMuc = 0;
                }

                db.SANPHAMs.Add(sANPHAM);
                db.SaveChanges();
                //SetAlter("Thêm thành công", "success");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;

                return View(sANPHAM);

            }
        }

        // GET: Admin/SANPHAMs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.FirstOrDefault(g => g.ID_SP == id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_DanhMuc = new SelectList(db.DANHMUCs, "ID_DanhMuc", "TenDanhMuc", sANPHAM.ID_DanhMuc);
            return View(sANPHAM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_SP,ID_DanhMuc,TenSanPham,AnhBia,Anh1,Anh2,GiaTien,GiamGia,TinhTrang,SoLuong,MoTa")] SANPHAM sANPHAM)
        {

            var f = Request.Files["ImageFile"];
            var f1 = Request.Files["ImageFile1"];
            var f2 = Request.Files["ImageFile2"];
            if (f != null && f.ContentLength > 0 && f1 != null && f1.ContentLength > 0 && f2 != null && f2.ContentLength > 0)
            {
                //Use  Namespace  called  :	System.IO
                string FileName = System.IO.Path.GetFileName(f.FileName);
                string FileName1 = System.IO.Path.GetFileName(f1.FileName);
                string FileName2 = System.IO.Path.GetFileName(f2.FileName);
                //Lấy  tên  file  upload
                string UploadPath = Server.MapPath("~/wwwoot/dataimg/" + FileName);
                string UploadPath1 = Server.MapPath("~/wwwoot/dataimg/" + FileName1);
                string UploadPath2 = Server.MapPath("~/wwwoot/dataimg/" + FileName2);
                //Copy  Và  lưu  file  vào  server.
                f.SaveAs(UploadPath);
                f1.SaveAs(UploadPath1);
                f2.SaveAs(UploadPath2);
                //Lưu  tên  file  vào  trường
                sANPHAM.AnhBia = FileName;
                sANPHAM.Anh1 = FileName1;
                sANPHAM.Anh2 = FileName2;
            }
            var soluong = Request["ID_DanhMuc"];
            if (soluong != null)
            {
                sANPHAM.ID_DanhMuc = Convert.ToInt32(Request["ID_DanhMuc"]);
            }
            else
            {
                sANPHAM.ID_DanhMuc = 0;
            }

            var GiaTien = !string.IsNullOrEmpty(Request["GiaTien"]) ? Convert.ToInt32(Request["GiaTien"].Replace(".", "")) : 0;
            sANPHAM.GiaTien = Convert.ToDecimal(GiaTien);
            db.Entry(sANPHAM).State = EntityState.Modified;
            db.SaveChanges();
            //SetAlter("Cập nhật thành công", "success");
            return RedirectToAction("Index");

        }

        // GET: Admin/SANPHAMs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.FirstOrDefault(g => g.ID_SP == id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            return View(sANPHAM);
        }

        // POST: Admin/SANPHAMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SANPHAM sANPHAM = db.SANPHAMs.FirstOrDefault(g => g.ID_SP == id);
            db.SANPHAMs.Remove(sANPHAM);
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
    }
}
