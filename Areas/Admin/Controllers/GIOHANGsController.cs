using NReco.PdfGenerator;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Web.Models;

namespace Web.Areas.Admin.Controllers
{
    public class GIOHANGsController : Controller
    {
        private webdoanvat db = new webdoanvat();

        // GET: Admin/GIOHANGs
        public ActionResult Index(string searchString, int? ma = -1)
        {
            //var gIOHANGs = db.GIOHANGs.Include(g => g.SANPHAM).Include(g => g.TAIKHOAN);
            //return View(gIOHANGs.ToList());
            var list = db.THANHTOANs.AsNoTracking().ToList();
            ViewData["thanhtoan"] = list;
            var gIOHANGs = db.GIOHANGs.Include(g => g.SANPHAM).Include(g => g.TAIKHOAN);
            if (!String.IsNullOrEmpty(searchString))
            {
                gIOHANGs = gIOHANGs.Where(b => b.ID_GioHang.ToString().Contains(searchString));
            }
            if (ma >= 0)
            {
                gIOHANGs = gIOHANGs.Where(b => b.TinhTrang == ma);
            }
            var m = Convert.ToInt32(Session["PQAdmin"]);
            if (Session["ID_TKAdmin"] != null && m != 3)
            {
                return View(gIOHANGs.ToList());
            }
            else
            {
                return Redirect("~/Admin/Home/Login");
            }

        }

        // GET: Admin/GIOHANGs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list = db.THANHTOANs.AsNoTracking().ToList();
            ViewData["thanhtoan"] = list;
            GIOHANG gIOHANG = db.GIOHANGs.FirstOrDefault(g => g.ID_GioHang == id);
            ViewData["tt"] = db.GIOHANGs.AsQueryable().Where(g => g.ID_ThanhToan == gIOHANG.ID_ThanhToan).ToList();
            if (gIOHANG.ID_ThanhToan == 0)
            {
                ViewData["tt"] = db.GIOHANGs.AsQueryable().Where(g => g.ID_GioHang == id).ToList(); ;
            }
            if (gIOHANG == null)
            {
                return HttpNotFound();
            }
            return View(gIOHANG);
        }
        [HttpPost]
        public void DaTT(int? id, int? idtt)
        {

            var tk = Convert.ToInt32(Request.Cookies["myCookieAdmin"].Value);
            var k = db.GIOHANGs.Where(g => g.ID_ThanhToan == idtt).ToList();
            foreach (var item in k)
            {
                item.TinhTrang = 3;
                db.GIOHANGs.AddOrUpdate(item);
            }
            var t = db.THANHTOANs.FirstOrDefault(g => g.ID_ThanhToan == idtt);
            t.TinhTrang = 2;
            t.NgayTao = DateTime.Now;
            db.THANHTOANs.AddOrUpdate(t);
            db.SaveChanges();
        }

        // GET: Admin/GIOHANGs/Create
        public ActionResult Create()
        {
            ViewBag.ID_SP = new SelectList(db.SANPHAMs, "ID_SP", "TenSanPham");
            ViewBag.ID_TK = new SelectList(db.TAIKHOANs, "ID_TKAdmin", "TenDangNhap");
            return View();
        }

        // POST: Admin/GIOHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_GioHang,ID_SP,ID_DanhMuc,ID_TK,SoTien,DuLieu,TinhTrang")] GIOHANG gIOHANG)
        {
            if (ModelState.IsValid)
            {
                db.GIOHANGs.Add(gIOHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_SP = new SelectList(db.SANPHAMs, "ID_SP", "TenSanPham", gIOHANG.ID_SP);
            ViewBag.ID_TK = new SelectList(db.TAIKHOANs, "ID_TKAdmin", "TenDangNhap", gIOHANG.ID_TK);
            return View(gIOHANG);
        }

        // GET: Admin/GIOHANGs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIOHANG gIOHANG = db.GIOHANGs.Find(id);
            if (gIOHANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_SP = new SelectList(db.SANPHAMs, "ID_SP", "TenSanPham", gIOHANG.ID_SP);
            ViewBag.ID_TK = new SelectList(db.TAIKHOANs, "ID_TKAdmin", "TenDangNhap", gIOHANG.ID_TK);
            return View(gIOHANG);
        }

        // POST: Admin/GIOHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_GioHang,ID_SP,ID_DanhMuc,ID_TK,SoTien,DuLieu,TinhTrang")] GIOHANG gIOHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gIOHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_SP = new SelectList(db.SANPHAMs, "ID_SP", "TenSanPham", gIOHANG.ID_SP);
            ViewBag.ID_TK = new SelectList(db.TAIKHOANs, "ID_TKAdmin", "TenDangNhap", gIOHANG.ID_TK);
            return View(gIOHANG);
        }

        // GET: Admin/GIOHANGs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIOHANG gIOHANG = db.GIOHANGs.Find(id);
            if (gIOHANG == null)
            {
                return HttpNotFound();
            }
            return View(gIOHANG);
        }

        // POST: Admin/GIOHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GIOHANG gIOHANG = db.GIOHANGs.Find(id);
            db.GIOHANGs.Remove(gIOHANG);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //
        // GET: Admin/GIOHANGs/Edit/5
        public ActionResult XNThanhToan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIOHANG gIOHANG = db.GIOHANGs.Find(id);
            if (gIOHANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_SP = new SelectList(db.SANPHAMs, "ID_SP", "TenSanPham", gIOHANG.ID_SP);
            ViewBag.ID_TK = new SelectList(db.TAIKHOANs, "ID_TKAdmin", "TenDangNhap", gIOHANG.ID_TK);
            return View(gIOHANG);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XNThanhToan([Bind(Include = "ID_GioHang,ID_SP,ID_DanhMuc,ID_TK,SoTien,DuLieu,TinhTrang")] GIOHANG gIOHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gIOHANG).State = EntityState.Modified;
                gIOHANG.TinhTrang = 1;
                db.SaveChanges();
                return RedirectToAction("Index", "XNThanhToan");
            }
            ViewBag.ID_SP = new SelectList(db.SANPHAMs, "ID_SP", "TenSanPham", gIOHANG.ID_SP);
            ViewBag.ID_TK = new SelectList(db.TAIKHOANs, "ID_TKAdmin", "TenDangNhap", gIOHANG.ID_TK);
            return View(gIOHANG);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExportPDF(FormCollection collection)
        {
            string htmlString = collection["GridHtml"];
            string tenFile = collection["tenFilePDF"];
            string widthtable = collection["widthTable"];
            string heightTable = collection["heightTable"];
            string oritentTable = collection["oritentTable"];
            string baseUrl = "";
            int webPageWidth = !string.IsNullOrEmpty(widthtable) ? Convert.ToInt32(widthtable) : 1024;
            int webPageHeight = !string.IsNullOrEmpty(heightTable) ? Convert.ToInt32(heightTable) : 650;
            var webPageOrient = NReco.PdfGenerator.PageOrientation.Portrait;
            var htmlToPdf = new HtmlToPdfConverter();
            htmlToPdf.PageWidth = webPageWidth;
            htmlToPdf.Orientation = webPageOrient; // Portrait - Landscape
            htmlToPdf.Size = PageSize.A4;
            htmlToPdf.PageFooterHtml = "";
            htmlToPdf.Margins.Top = 10;
            htmlToPdf.Margins.Left = 10;
            htmlToPdf.Margins.Right = 10;
            htmlToPdf.Margins.Bottom = 10;
            var pdfBytes = htmlToPdf.GeneratePdf(htmlString);
            FileResult fileResult = new FileContentResult(pdfBytes, "application/pdf");
            fileResult.FileDownloadName = tenFile + ".pdf";
            return fileResult;
        }
    }
}
