using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebSockets;
using System.Xml.Linq;

namespace KeyBoardStore.Models
{
    public class GioHang
    {
        dbBanPhimDataContext data = new dbBanPhimDataContext();
        public int maphim { get; set; }

        [Display(Name = "Tên Phím")]
        public string tenphim { get; set; }
        [Display(Name = "")]
        public string hinh { get; set; }
        [Display(Name = "Giá Bán")]
        public double giaban { get; set; }

        [Display(Name = "Số Luợng")]
        public int isoluong { get; set; }
        [Display(Name = "Tổng Tiền")]
        public double dThanhtien
        {
            get { return isoluong * giaban; }
        }
        public GioHang(int id)
        {
            maphim = id;
            BanPhim phim = data.BanPhims.Single(n => n.maphim == maphim);
            tenphim = phim.tenphim;
            hinh = phim.hinh;
            giaban = double.Parse(phim.giaban.ToString());
            isoluong = 1;
        }
    }
}