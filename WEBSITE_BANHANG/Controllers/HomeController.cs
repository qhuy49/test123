using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBSITE_BANHANG.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace WEBSITE_BANHANG.Controllers
{
    
    public class HomeController : Controller
    {
        // GET: Home
      
        WEBSITE_BANHANG.Models.WEB_BANHANGEntities db = new Models.WEB_BANHANGEntities();
        public ActionResult Index()
        {

            //if (Session["expired"] != null)
            //{
            //    if(DateTime.Parse(Session["expired"].ToString()) >= DateTime.Now)
            //    {
            //        // van con hieu luc
            //        // hien thi thong tin dang nhap
            //        var name = Session["name"];
                    
            //    }
            //    else
            //    {
            //        // het 
            //        Session["expired"] = null;
            //        Session["id_token"] = null;
            //        Session["orgid"] = null;
            //        // xoa het thong tin 
            //    }
            //}
            // tạo viewbag lấy list sp từ db
            //list dt
            //  var lstDT = from KH in db.SanPhams select KH;
            var LSTDTM = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.LSTDTM = LSTDTM;
            //LIST MÁY TÍNH BẢNG MỚI NHẤT
            var LSTMTB = db.SanPhams.Where(n => n.MaLoaiSP == 3 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.LSTMTB = LSTMTB;
            //LIST LAPTOP MỚI NHẤT
            var LSTLT = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.LSTDLT = LSTLT;


            return View();
        }
        public ActionResult MenuPartial()
        {
            var lstSP = db.SanPhams;
            return PartialView(lstSP);
        }

        [HttpGet]
        public ActionResult MenuLogin()
        {
            ViewBag.CauHoi = new SelectList(Loadcauhoi());
            return PartialView();
        }
        [HttpPost]
        public ActionResult MenuLogin(ThanhVien tv )
        {

            ViewBag.CauHoi = new SelectList(Loadcauhoi());

            //kiểm tra captcha hợp lệ

            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                if (ModelState.IsValid)
                {
                    Response.Write(@"<script language='javascript'>alert('Message: \n" + "Đăng ký thành công!!!" + " .');</script>");
                    db.ThanhViens.Add(tv);
                    db.SaveChanges();
                    return PartialView();
                }
                else
                {
                    Response.Write(@"<script language='javascript'>alert('Message: \n" + "Đăng ký thất bại!!!" + " .');</script>");
                    return PartialView();
                }

            }
            // thêm khách vào csdl
            //ViewBag.ThongBao = "Sai mã captcha";
            
            
             //   Response.Write(@"<script language='javascript'>alert('Message: \n" + "Sai mã captcha" + " .');</script>");
                return PartialView();
            
           
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            return View();
        }
        public List<string> Loadcauhoi()
        {
                List<string>  lstcauhoi = new List<string>();
            lstcauhoi.Add("Bạn thích con gì nhất ?");
            lstcauhoi.Add("Bạn sống ở đâu ?");
            lstcauhoi.Add("Trường bạn học là gì ?");
            lstcauhoi.Add("Ba mẹ bạn ở đâu ?");
            lstcauhoi.Add("Bạn thích ăn gì ?");
            lstcauhoi.Add("Con vật mà bạn ghét nhất ?");
            lstcauhoi.Add("Con vật mà bạn thích nhất ?");
            return lstcauhoi;
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f, string strUrl)
        {
            //ktra tên đn và mk
            string taikhoan = f["Name"].ToString();
            string matkhau = f["Password"].ToString();
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == taikhoan && n.MatKhau == matkhau);
            if (tv != null)
            {
                IEnumerable<LoaiThanhVien_Quyen> lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                string Quyen = "";
                foreach (var item in lstQuyen)
                {
                    Quyen += item.Quyen.MaQuyen + ",";
                }
                Quyen = Quyen.Substring(0, Quyen.Length - 1);
                //    
                //    //return Json(new { url = strUrl });
                //    //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Redirect, strUrl);
                //    return Content("<script>window.location.reload();</script>");
                //}
                //return Content("Tài khoản hoặc mật khẩu không đúng");
                PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
                Session["TaiKhoan"] = tv;
                return Content("<script>window.location.reload();</script>");

            }
            return View();
        }
        public void PhanQuyen(string taikhoan , string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1, taikhoan, DateTime.Now, DateTime.Now.AddHours(3),false,Quyen, FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));

            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
                
            }
            Response.Cookies.Add(cookie);
        }
        public ActionResult LoiPhanQuyen()
        {
            return View();
        }
        public ActionResult DangXuat()
        {
            
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}