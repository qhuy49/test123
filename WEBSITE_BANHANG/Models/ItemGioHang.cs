using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBSITE_BANHANG.Models
{
    public class ItemGioHang
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string HinhAnh { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        public decimal ThanhTien { get; set; }
      


        public ItemGioHang(int iMaSP )
        {
            using (WEB_BANHANGEntities db = new WEB_BANHANGEntities())
            {
                this.MaSP = iMaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == iMaSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.SoLuong = 1;
                this.DonGia = sp.DonGia.Value;
                this.ThanhTien = DonGia * SoLuong;
            }
        }

        public ItemGioHang(int iMaSP, int sl)
        {
            using (WEB_BANHANGEntities db = new WEB_BANHANGEntities())
            {
                this.MaSP = iMaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == iMaSP);
                this.TenSP = sp.TenSP ;
                this.HinhAnh = sp.HinhAnh;
                this.SoLuong = sl;
                this.DonGia = sp.DonGia.Value;
               this.ThanhTien = DonGia * SoLuong;
            }
        }


    }
}