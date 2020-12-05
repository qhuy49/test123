using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WEBSITE_BANHANG.Models;
using PagedList;

namespace WEBSITE_BANHANG.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        WEB_BANHANGEntities db = new WEB_BANHANGEntities();
        //public ActionResult SanPham1()
        //{
        //    var lstSPMOI = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
        //    ViewBag.LstSP = lstSPMOI;
        //    var lstDT = db.SanPhams.Where(n => n.MaLoaiSP ==1);
        //    ViewBag.LstDT = lstDT;
        //    return View();
        //}

        //public ActionResult SanPham2()
        //{
        //    var lstSPMOI = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
        //    ViewBag.LstSP = lstSPMOI;
        //    return View();
        //}

        //[ChildActionOnly]
        public ActionResult SanPhamPartial()
        {
            //Lấy dữ liệu nạp vào model
            var lstSPMOI = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
            return PartialView(lstSPMOI);
            // return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SanPhamStyle1Patial()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SanPhamStyle2Patial()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult SanPhamStyle3Patial()
        {
            return PartialView();
        }

        public ActionResult ChiTietSanPham(int? id, string tensp)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
                var lstSP = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa==false);
                if (lstSP==null)
                {
                    return HttpNotFound();
                }
            
            
            return View(lstSP);
        }
        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX, int? page)
        {
            if (MaLoaiSP == null || MaNSX==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if (lstSP.Count()==0)
            {
                return HttpNotFound();
            }
            //thực hiện phân trang
            if (Request.HttpMethod!="GET")
            {
                page = 1;
            }
            // số sp trên 1 trang
            int PageSize = 6;
            //Số trang hiện tại
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;

            return View(lstSP.OrderBy(n =>n.MaSP).ToPagedList(PageNumber,PageSize));
        }
    }
}