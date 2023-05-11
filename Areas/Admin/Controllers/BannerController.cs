using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Web.Models;

namespace Web.Areas.Admin.Controllers
{
    public class BannerController : Controller
    {
        private webdoanvat db = new webdoanvat();


        public ActionResult Index()
        {
            var m = Convert.ToInt32(Session["PQAdmin"]);
            if (Session["ID_TKAdmin"] != null && m != 3)
            {
                var list = db.Banners.AsQueryable().ToList();
                return View(list);
            }
            else
            {
                return Redirect("~/Admin/Home/Login");
            }
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LinkBanner,ThuTu")] Banner banner)
        {
            try
            {
                banner.LinkBanner = "";
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
                    banner.LinkBanner = FileName;
                }
                banner.ThuTu = Convert.ToInt16(Request["ThuTu"]);
                db.Banners.Add(banner);
                db.SaveChanges();
                //SetAlter("Thêm thành công", "success");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;

                return View(banner);

            }
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner sANPHAM = db.Banners.FirstOrDefault(g => g.Id == id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            return View(sANPHAM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LinkBanner,ThuTu")] Banner sANPHAM)
        {

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
                sANPHAM.LinkBanner = FileName; ;
            }
            sANPHAM.ThuTu = Convert.ToInt16(Request["ThuTu"]);
            db.Entry(sANPHAM).State = EntityState.Modified;
            db.SaveChanges();
            //SetAlter("Cập nhật thành công", "success");
            return RedirectToAction("Index");

        }
    }
}