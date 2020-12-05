using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace WEBSITE_BANHANG.Controllers
{
    public class DemoAjaxController : Controller
    {
        // GET: DemoAjax
        WEBSITE_BANHANG.Models.WEB_BANHANGEntities db = new Models.WEB_BANHANGEntities();

        public ActionResult DemoAjax()
        {
            return View();
        }
        public ActionResult LoadAjaxActionlink()
        {
            System.Threading.Thread.Sleep(2000);
            return Content("Hello");
        }


        public ActionResult LoadAjaxbeginForm(FormCollection f)
        {
            string kq = f["txt1"].ToString();
            return Content(kq);
        }
        // acction sử lý ajax jquery
        public ActionResult AjaxJquery(int a, int b)
        {
            System.Threading.Thread.Sleep(2000);
            return Content((a + b).ToString());
        }


        public ActionResult LoadSanPhamPartial()
        {
            var listsp = db.SanPhams;
            ////Lấy dữ liệu nạp vào model
            //var lstSPMOI = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
            ////return PartialView(lstSPMOI);
            return PartialView(listsp);
        }
    }
}