using MoMo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class ShoppingcartController : Controller
    {
        // GET: Shoppingcart
        webdoanvat _db = new webdoanvat();
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        //phuong thuc add item vao gio hang
        public ActionResult AddtoCart(int id)
        {
            var pro = _db.SANPHAMs.SingleOrDefault(s => s.ID_SP == id);
            GIOHANG a = new GIOHANG();
            if (pro != null)
            {
                GetCart().Add(pro);
                Cart cart = Session["Cart"] as Cart;
                CartItem c = new CartItem();
                a.ID_TK = int.Parse(Request.Cookies["myCookie"].Value);
                a.SoTien = pro.GiaTien * 1;
                a.DuLieu = pro.MoTa;
                a.TinhTrang = 0;
                a.ID_SP = pro.ID_SP;
                a.ID_DanhMuc = pro.ID_DanhMuc;
                a.SoLuong = 1;
                _db.GIOHANGs.Add(a);
                _db.SaveChanges();

            }
            var check = (string)Session["check"] ?? "";
            check += a.ID_GioHang + ",";
            Session["check"] = check;
            return RedirectToAction("Index", "Home");

        }
        //trang gio hang
        public ActionResult ShowToCart()
        {
            var r = Request.Cookies["myCookie"];
            if (r != null)
            {
                ViewBag.coo = r.Value;
            }
            else
            {
                ViewBag.coo = "";
            }
            int tk = int.Parse(r.Value.ToString());
            Session["ID_TK"] = r.Value;
            var hoten = _db.TAIKHOANs.FirstOrDefault(g => g.ID_TK == tk).HoTen;
            var hinhanh = _db.TAIKHOANs.FirstOrDefault(g => g.ID_TK == tk).HinhAnh;
            ViewBag.HoTen = hoten;
            ViewBag.HinhAnh = hinhanh;
            if (Session["Cart"] == null)
            {

                return RedirectToAction("Index", "Home");

            }
            Cart cart = Session["Cart"] as Cart;
            return View(cart);
        }
        public ActionResult Update_soluong(FormCollection form, int? ID_GioHang = 0)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["ID_SP"]);
            int Sluong = int.Parse(form["So_luong"]);
            var tk = Convert.ToInt32(Request.Cookies["myCookie"].Value);
            var list = _db.GIOHANGs.Where(g => g.ID_SP == id_pro && g.ID_TK == tk && g.TinhTrang == 0 && g.ID_GioHang == ID_GioHang).ToList();
            foreach (var item in list)
            {
                var objSP = _db.SANPHAMs.FirstOrDefault(g => g.ID_SP == item.ID_SP);
                if (objSP.SoLuong < Sluong && objSP.DANHMUC.KieuDM == 2)
                {
                    return RedirectToAction("ShowToCart", "ShoppingCart", new { sc = 1 });
                }
                else
                {
                    item.SoLuong = Sluong;
                    _db.SaveChanges();
                }
            }
            cart.UpdateSoluong(id_pro, Sluong);
            return RedirectToAction("ShowToCart", "ShoppingCart");

        }
        //checkout
        public ActionResult CheckOut()
        {
            var tk = Convert.ToInt32(Request.Cookies["myCookie"].Value);
            var m = Session["check"].ToString();
            var l = m.Trim(',').Split(',').ToList();
            var List = _db.GIOHANGs.Where(g => g.TinhTrang == 0 && g.ID_TK == tk && l.Contains(g.ID_GioHang.ToString())).ToList();
            Session["tt"] = List;
            ViewBag.TK = _db.TAIKHOANs.FirstOrDefault(g => g.ID_TK == tk);

            foreach (var item in List)
            {
                item.TinhTrang = 1;
                _db.SaveChanges();
            }
            if (Session["Cart"] == null)
            {

                return RedirectToAction("CheckOut", "ShoppingCart");

            }
            Cart cart = Session["Cart"] as Cart;
            return View(cart);
        }
        [HttpPost]
        public ActionResult ThanhToan()
        {

            var tk = Convert.ToInt32(Request.Cookies["myCookie"].Value);
            var k = _db.TAIKHOANs.FirstOrDefault(g => g.ID_TK == tk).MatKhau;
            THANHTOAN ttt = new THANHTOAN();
            ttt.ID_TK = int.Parse(Request.Cookies["myCookie"].Value);
            ttt.PhuongThucTT = Request["tt"];
            ttt.ThongTinTT = Request["tttt"];
            ttt.BaoMat = k;
            ttt.NgayTao = DateTime.Now;
            ttt.TinhTrang = 1;
            _db.THANHTOANs.Add(ttt);
            _db.SaveChanges();
            var m = Session["check"].ToString();
            var l = m.Trim(',').Split(',').ToList();
            var i = 0;
            var list = Session["tt"] as List<GIOHANG> ?? new List<GIOHANG>();

            Cart cart = Session["Cart"] as Cart;
            Session["cart"] = new List<Cart>();
            //return RedirectToAction("Index", "Home", new { sc = 1 });
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "Thanh toán hóa đơn";
            string returnUrl = "https://localhost:44394/Home/ConfirmPaymentClient";
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test
            var tien = ttt.ThongTinTT.Replace("VND", "").Replace(".", "").Trim();
            string amount = tien;
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

        
            if (ttt.PhuongThucTT == "1")
            {
                foreach (var item in list)
                {
                    //var id_gh = Convert.ToInt16(l[i]);
                    item.TinhTrang = 2;
                    item.ID_ThanhToan = ttt.ID_ThanhToan;
                    item.NgayDatHang = DateTime.Now;
                    _db.GIOHANGs.AddOrUpdate(item);
                    // tru trong bang sp
                    var objSP = _db.SANPHAMs.FirstOrDefault(g => g.ID_SP == item.ID_SP && g.DANHMUC.KieuDM == 2);
                    if (objSP != null)
                    {
                        objSP.SoLuong -= item.SoLuong;
                        _db.SANPHAMs.AddOrUpdate(objSP);
                    }
                    i++;

                }
                _db.SaveChanges();
                return RedirectToAction("Index", "Home", new { sc = 1 });
            }
            else
            {
                foreach (var item in list)
                {
                    //var id_gh = Convert.ToInt16(l[i]);
                    item.TinhTrang = 3;
                    item.ID_ThanhToan = ttt.ID_ThanhToan;
                    item.NgayDatHang = DateTime.Now;
                    _db.GIOHANGs.AddOrUpdate(item);
                    // tru trong bang sp
                    var objSP = _db.SANPHAMs.FirstOrDefault(g => g.ID_SP == item.ID_SP && g.DANHMUC.KieuDM == 2);
                    if (objSP != null)
                    {
                        objSP.SoLuong -= item.SoLuong;
                        _db.SANPHAMs.AddOrUpdate(objSP);
                    }
                    i++;

                }
                JObject jmessage = JObject.Parse(responseFromMomo);
                _db.SaveChanges();
                return Redirect(jmessage.GetValue("payUrl").ToString());
            }

        }
        public ActionResult Xoa(int ID_SP = 0)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Xoa(ID_SP);
            return RedirectToAction("ShowToCart", "ShoppingCart");

        }
        public ActionResult ShowCart(int TinhTrang = -1)
        {
            var r = Request.Cookies["myCookie"];
            if (r != null)
            {
                ViewBag.coo = r.Value;
            }
            else
            {
                ViewBag.coo = "";
            }
            int tk = int.Parse(r.Value.ToString());
            Session["ID_TK"] = r.Value;
            var hoten = _db.TAIKHOANs.FirstOrDefault(g => g.ID_TK == tk).HoTen;
            var hinhanh = _db.TAIKHOANs.FirstOrDefault(g => g.ID_TK == tk).HinhAnh;
            ViewBag.HoTen = hoten;
            ViewBag.HinhAnh = hinhanh;
            var query = _db.GIOHANGs.AsNoTracking().Where(g => g.ID_TK == tk);
            if (TinhTrang > -1)
            {
                query = query.Where(g => g.TinhTrang == TinhTrang);
            }
            var list = query.ToList();
            ViewData["sp"] = _db.SANPHAMs.AsNoTracking().ToList();
            return View(list);
        }
        public ActionResult HuyDon(int ID_GioHang)
        {
            GIOHANG m = _db.GIOHANGs.FirstOrDefault(g => g.ID_GioHang == ID_GioHang);
            m.TinhTrang = 4;
            _db.GIOHANGs.AddOrUpdate(m);
            _db.SaveChanges();
            return RedirectToAction("ShowCart", "ShoppingCart");
        }

    }

}
