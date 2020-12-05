using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WEBSITE_BANHANG.Models
{
    [MetadataTypeAttribute(typeof(ThanhVienMetadata))]
    public partial class ThanhVien
    {
        
        internal sealed class ThanhVienMetadata
        {
            public int MaThanhVien { get; set; }
            public string TaiKhoan { get; set; }
            public string MatKhau { get; set; }
            public string HoTen { get; set; }
            public string DiaChi { get; set; }
            [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
            ErrorMessage = "Please enter correct email address")]

            public string Email { get; set; }
            public string SoDienThoai { get; set; }
            public string CauHoi { get; set; }
            public Nullable<int> MaLoaiTV { get; set; }

  
        }
    }
}