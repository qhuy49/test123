using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBSITE_BANHANG.Models;

namespace WEBSITE_BANHANG.Controllers
{
    [Authorize(Roles = "DangKy")]
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        WEB_BANHANGEntities db = new WEB_BANHANGEntities();
        public ActionResult Index()
        {
            //var lstKH = from KH in db.KhachHangs select KH;
            var lstKH = db.KhachHangs.ToList();
            return View(lstKH);
        }


        public ActionResult TruyVan1DoiTuong()
        {
            //cách 1: truy vấn 1 đối tượng bằng câu lệnh truy vấn
            //buocws1:
            //  var lstKH = from kh in db.KhachHangs where kh.MaKH==1 select kh;
            //bước 2:
            //   KhachHang khach = lstKH.FirstOrDefault();


            //cách 2: dựa trên phương thức hỗ trợ sẵn
            KhachHang khach = db.KhachHangs.SingleOrDefault(n => n.MaKH == 1);
            return View(khach);
        }

        public ActionResult SortDuLieu()
        {
            //cách 1: truy vấn 1 đối tượng bằng câu lệnh truy vấn và sắp xếp


            List<KhachHang> lstKH = db.KhachHangs.OrderByDescending(n=> n.TenKH).ToList();
            //buocws1:
            //  var lstKH = from kh in db.KhachHangs where kh.MaKH==1 select kh;
            //bước 2:
            //   KhachHang khach = lstKH.FirstOrDefault();


            //cách 2: dựa trên phương thức hỗ trợ sẵn
          //  KhachHang khach = db.KhachHangs.SingleOrDefault(n => n.MaKH == 1);
            return View(lstKH);
        }

        public ActionResult GroupDuLieu()
        {
            //cách 1: truy vấn 1 đối tượng bằng câu lệnh truy vấn và sắp xếp


            List<ThanhVien> lstKH = db.ThanhViens.OrderByDescending(n => n.TaiKhoan).ToList();
            //buocws1:
            //  var lstKH = from kh in db.KhachHangs where kh.MaKH==1 select kh;
            //bước 2:
            //   KhachHang khach = lstKH.FirstOrDefault();


            //cách 2: dựa trên phương thức hỗ trợ sẵn
            //  KhachHang khach = db.KhachHangs.SingleOrDefault(n => n.MaKH == 1);
            return View(lstKH);
        }
    }
}