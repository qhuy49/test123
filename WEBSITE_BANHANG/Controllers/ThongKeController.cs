using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEBSITE_BANHANG.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: ThongKe
        public ActionResult ThongKe()
        {

            ViewBag.Online = HttpContext.Application["Online"].ToString();
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString(); // Lấy số lg người truy cấp từ application đã tạo
            return View();
        }
    }
}