using KeyBoardStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyBoardStore.Controllers
{
    public class GioHangController : Controller
    {
        dbBanPhimDataContext db = new dbBanPhimDataContext();
        public List<GioHang> Laygiohang()
        {
            List<GioHang> listGiohang = Session["Giohang"] as List<GioHang>;
            if (listGiohang == null)
            {
                listGiohang = new List<GioHang>();
                Session["GioHang"] = listGiohang;
            }
            return listGiohang;
        }
        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<GioHang> listGiohang = Laygiohang();
            GioHang sanpham = listGiohang.Find(n => n.maphim == id);
            if (sanpham == null)
            {
                sanpham = new GioHang(id);
                listGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.isoluong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang == null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }
        private int TongsoluongSanPham()
        {
            int tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }
        private double TongTien()
        {
            double tt = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tt = lstGioHang.Sum(n => n.dThanhtien);
            }
            return tt;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = Laygiohang();
            ViewBag.TongsoLuong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongsoluongSanPham();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.Tongsoluongsanpham = TongsoluongSanPham();
            return PartialView();
        }
        public ActionResult XoaGioHang(int id)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.maphim == id);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.maphim == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGiohang(int id, FormCollection collection)
        {
            List<GioHang> lstGioHang = Laygiohang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.maphim == id);
            if (sanpham != null)
            {
                sanpham.isoluong = int.Parse(collection["txtSoLg"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Home", "Home");
                //return RedirectToAction("GioHangTrong", "GioHang");
            }
            List<GioHang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0) 
            {
                //ViewData["ErrorMessage"] = "Có lỗi xảy ra";
                return RedirectToAction("GioHangTrong", "GioHang");
            }
            ViewBag.TongsoLuong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.TongsoLuongsanpham = TongsoluongSanPham();
            return View(lstGiohang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            BanPhim s = new BanPhim();
            List<GioHang> gh = Laygiohang();
            //var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);

            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            //dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;
            db.DonHangs.InsertOnSubmit(dh);
            db.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.maphim = item.maphim;
                ctdh.soluong = item.isoluong;
                ctdh.gia = (decimal)item.giaban;
                s = db.BanPhims.Single(n => n.maphim == item.maphim);
                s.soluongton -= ctdh.soluong;
                db.SubmitChanges();
                db.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            db.SubmitChanges();
            Session["GioHang"] = null;
            
            //ViewData["ErrorMessage"] = "Có lỗi xảy ra";
           
            return RedirectToAction("XacnhanDonhang", "GioHang");
        }
        public ActionResult XacnhanDonhang()
        {
            return View();
        }
        //GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(int id)
        {
            var D_phim = db.BanPhims.Where(m => m.maphim == id).First();
            return View(D_phim);
        }
        public ActionResult GioHangTrong()
        {
            return View();
        }
    }
}