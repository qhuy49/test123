using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBSITE_BANHANG.Models;

namespace WEBSITE_BANHANG.Controllers
{
    public class GioHangController : Controller
    {
        WEB_BANHANGEntities db = new WEB_BANHANGEntities();
        public List<ItemGioHang> LayGioHang()
        {
            // giỏ hàng đã tồn tại
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                // nếu session giở hàng chưa tồn tại
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
                
            }
            return lstGioHang;
        }
        //Thêm giỏ hàng bth (load lại trang)
        public ActionResult ThemGioHang(int MaSP , string strUrl)
        {
            //kiểm tra sp tồn tại trong csdl
            SanPham sp = db.SanPhams.SingleOrDefault(n=>n.MaSP == MaSP);
            if (sp == null)
            {
                //  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Response.StatusCode = 404;
                return null;
            }
            // lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            // trường hợp 1 : sp đã tồn tại trong giỏ hàng
            ItemGioHang spcheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spcheck != null)
            {
                if (sp.SoLuongTon < spcheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spcheck.SoLuong++;
                spcheck.ThanhTien = spcheck.SoLuong * spcheck.DonGia;
                return Redirect(strUrl);
            }

            
            ItemGioHang itemgiohang = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemgiohang.SoLuong)
            {
                return View("ThongBao");
            }
            lstGioHang.Add(itemgiohang);
            return Redirect(strUrl);
        }
        //tính tổng số lượng
        public double TinhTongSoLuong()
        {
            List<ItemGioHang> lstgiohang = Session["GioHang"] as List<ItemGioHang>;
            if (lstgiohang ==null)
            {
                return 0;

            }
            return lstgiohang.Sum(n=>n.SoLuong);
        }
        // tính tổng tiền 
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstgiohang = Session["GioHang"] as List<ItemGioHang>;
            if (lstgiohang == null)
            {
                return 0;

            }
            return lstgiohang.Sum(n => n.ThanhTien);
        }

        // GET: GioHang
        public ActionResult XemGioHang()
        {
            List<ItemGioHang> lstgiohang = LayGioHang();
            return View(lstgiohang);
        }
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            //KIỂM TRA SESSION GIỎ HÀNG TỒN TẠI CHƯA
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index","Home");
            }
            //kiểm tra sp có tồn tại trong CSDL k
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Response.StatusCode = 404;
                return null;
            }
            // Lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //ktra sp có tồn tại trong giỏ hàng không
            ItemGioHang spcheck = lstGioHang.SingleOrDefault(n=>n.MaSP == MaSP);
            if (spcheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //lấy list giỏ hàng
            ViewBag.lstgiohang = lstGioHang;
            // nếu đã tồn tại
            return View(spcheck);
        }

        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong()==0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongThanhTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongThanhTien = TinhTongTien();
            return PartialView();
        }
        [HttpPost]
        public ActionResult SuaGioHang(int soluong, int MaSP)
        {
            //ktra số lượng tồn
            SanPham spcheck = db.SanPhams.SingleOrDefault(n=>n.MaSP==MaSP);
            if (spcheck.SoLuongTon < soluong)
            {
                return View("ThongBao");
            }
            // cập nhật số lượng trong session giỏ hàng
            List<ItemGioHang> lstGH = LayGioHang();
            //b1: lấy sản phẩm hiện tại cần sửa trong giỏ hàng ra
            ItemGioHang itemupdate = lstGH.Find(n=>n.MaSP == MaSP);
            //b2: cập nhật số lượng, thành tiền bằng với số lượng truyền vào
            itemupdate.SoLuong = soluong;
            itemupdate.ThanhTien = itemupdate.SoLuong *itemupdate.DonGia;
            return RedirectToAction("XemGioHang");
        }
        //[HttpPost]
        //public ActionResult SuaGioHang1(ItemGioHang itemgh)
        //{
        //    //ktra số lượng tồn
        //    SanPham spcheck = db.SanPhams.SingleOrDefault(n => n.MaSP == itemgh.MaSP);
        //    if (spcheck.SoLuongTon < itemgh.SoLuong)
        //    {
        //        return View("ThongBao");
        //    }
        //    // cập nhật số lượng trong session giỏ hàng
        //    List<ItemGioHang> lstGH = LayGioHang();
        //    //b1: lấy sản phẩm hiện tại cần sửa trong giỏ hàng ra
        //    ItemGioHang itemupdate = lstGH.Find(n => n.MaSP == itemgh.MaSP);
        //    //b2: cập nhật số lượng, thành tiền bằng với số lượng truyền vào
        //    itemupdate.SoLuong = itemgh.SoLuong;
        //    itemupdate.ThanhTien = itemupdate.SoLuong * itemupdate.DonGia;
        //    return RedirectToAction("XemGioHang");
        //}
        //[HttpGet]
        //public ActionResult SuaGioHang1(int MaSP)
        //{
        //    //KIỂM TRA SESSION GIỎ HÀNG TỒN TẠI CHƯA
        //    if (Session["GioHang"] == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    //kiểm tra sp có tồn tại trong CSDL k
        //    SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
        //    if (sp == null)
        //    {
        //        //  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        Response.StatusCode = 404;
        //        return null;
        //    }
        //    // Lấy list giỏ hàng từ session
        //    List<ItemGioHang> lstGioHang = LayGioHang();
        //    //ktra sp có tồn tại trong giỏ hàng không
        //    ItemGioHang spcheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
        //    if (spcheck == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    //lấy list giỏ hàng
        //    ViewBag.lstgiohang = lstGioHang;
        //    // nếu đã tồn tại
        //    return View(spcheck);
        //}
        public ActionResult XoaGioHang(int MaSP)
        {

            //KIỂM TRA SESSION GIỎ HÀNG TỒN TẠI CHƯA
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //kiểm tra sp có tồn tại trong CSDL k
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Response.StatusCode = 404;
                return null;
            }
            // Lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //ktra sp có tồn tại trong giỏ hàng không
            ItemGioHang spcheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spcheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Xóa item trong giỏ hàng
            lstGioHang.Remove(spcheck);
            return RedirectToAction("XemGioHang");
        }
        // chức năng đặt hàng
        public ActionResult DatHang()
        {
            //KIỂM TRA SESSION GIỎ HÀNG TỒN TẠI CHƯA
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Thêm đơn hàng
            DonDatHang ddh = new DonDatHang();
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            //thêm chi tiết đơn đặt hàng
            List<ItemGioHang> lstGH = LayGioHang();
            foreach ( var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");

        }
        [HttpPost]
        public ActionResult DatHang(KhachHang kh)
        {
            //KIỂM TRA SESSION GIỎ HÀNG TỒN TẠI CHƯA
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang khachhang = new KhachHang();
            //thêm đơn hàng của khách vãng lai (k có tài khoản)
            if (Session["TaiKhoan"] == null)
            {
                
                khachhang = kh;
                db.KhachHangs.Add(khachhang);
            }
            else
            {
                //đối với khách hàng là thành viên(có tài khoản)
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khachhang.TenKH = tv.HoTen;
                khachhang.DiaChi = tv.DiaChi;
                khachhang.Email = tv.Email;
                khachhang.SoDienThoai = tv.SoDienThoai;
                khachhang.MaThanhVien = tv.MaLoaiTV;
                db.KhachHangs.Add(khachhang);
                db.SaveChanges();
            }
            //Thêm đơn hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khachhang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            //thêm chi tiết đơn đặt hàng
            List<ItemGioHang> lstGH = LayGioHang();
            foreach (var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");

        }
        // Ajax thêm giỏ hàng
        [HttpPost]
        public ActionResult ThemGioHangAjax(int MaSP, string strUrl)
        {
            //kiểm tra sp tồn tại trong csdl
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Response.StatusCode = 404;
                return null;
            }
            // lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            // trường hợp 1 : sp đã tồn tại trong giỏ hàng
            ItemGioHang spcheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spcheck != null)
            {
                if (sp.SoLuongTon < spcheck.SoLuong)
                {
                    return Content("<script>alert(\"Sản phẩm đã hết hàng !!!\");</script>");
                }
                spcheck.SoLuong++;
                spcheck.ThanhTien = spcheck.SoLuong * spcheck.DonGia;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongThanhTien = TinhTongTien();
                return PartialView("GioHangPartial");
            }


            ItemGioHang itemgiohang = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemgiohang.SoLuong)
            {
                return Content("<script>alert(\"Sản phẩm đã hết hàng !!!\");</script>");
            }
            lstGioHang.Add(itemgiohang);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongThanhTien = TinhTongTien();
            return PartialView("GioHangPartial");
        }
        public ActionResult SuaGioHangAjax(int MaSP)
        {
            //KIỂM TRA SESSION GIỎ HÀNG TỒN TẠI CHƯA
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //kiểm tra sp có tồn tại trong CSDL k
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Response.StatusCode = 404;
                return null;
            }
            // Lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //ktra sp có tồn tại trong giỏ hàng không
            ItemGioHang spcheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spcheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //lấy list giỏ hàng
            ViewBag.lstgiohang = lstGioHang;
            // nếu đã tồn tại
            return RedirectToAction("SuaGioHang",spcheck);
        }
    }
}