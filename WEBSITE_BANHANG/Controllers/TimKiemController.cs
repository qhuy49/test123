using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBSITE_BANHANG.Models;
using PagedList;

namespace WEBSITE_BANHANG.Controllers
{
    public class TimKiemController : Controller
    {
        WEB_BANHANGEntities db = new WEB_BANHANGEntities();
        // GET: TimKiem
        [HttpGet]
        public ActionResult KQTimKiem(string TuKhoa, int? page)
        {
            //thực hiện phân trang

            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            // số sp trên 1 trang
            int PageSize = 6;
            //Số trang hiện tại
            int PageNumber = (page ?? 1);
            ViewBag.TuKhoa = TuKhoa;
            //tìm kiếm theo tên sp
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(TuKhoa));
            return View(lstSP.OrderBy(n=>n.TenSP).ToPagedList(PageNumber,PageSize));
        }
        [HttpPost]
        public ActionResult LayTuKhoaTimKiem(string TuKhoa)
        {
            //thực hiện phân trang

            return RedirectToAction("KQTimKiem", new {@TuKhoa=TuKhoa });
        }
        public ActionResult KQTimKiemPartial(string TuKhoa)
        {
            ViewBag.TuKhoa = TuKhoa;
            //tìm kiếm theo tên sp
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(TuKhoa));
            return PartialView(lstSP.OrderBy(n=>n.DonGia));
        }
    }
}