using KeyBoardStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyBoardStore.Controllers
{
    public class TongSLDonHangController : Controller
    {
        dbBanPhimDataContext db = new dbBanPhimDataContext();
        public ActionResult TongSLBanPhim()
        {
            var result = from BanPhim in db.BanPhims
                         join ChiTietDonHang in db.ChiTietDonHangs on BanPhim.maphim equals ChiTietDonHang.maphim
                         join dh in db.DonHangs on ChiTietDonHang.madon equals dh.madon
                         group ChiTietDonHang by BanPhim.tenphim into a
                         select new TongSL
                         {
                             TenSP = a.First().BanPhim.tenphim,
                             TongSLuong = (int)a.Sum(x => x.soluong),
                             TongTien = (int)a.Sum(x => x.gia)
                         };
            return View(result.ToList());
        }
    }
}