using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEBSITE_BANHANG.Controllers
{

    public class PhanQuyenController : Controller
    {
        WEBSITE_BANHANG.Models.WEB_BANHANGEntities db = new Models.WEB_BANHANGEntities();
        // GET: PhanQuyen
        public ActionResult Index()
        {
            return View(db.LoaiThanhViens);

        }
    }
}