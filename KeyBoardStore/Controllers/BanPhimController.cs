using KeyBoardStore.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace KeyBoardStore.Controllers
{
    public class BanPhimController : Controller
    {
        dbBanPhimDataContext data = new dbBanPhimDataContext();
        // GET: BanPhim
        public ActionResult Index(int? page, string searchString)
        {
        //    var all_key = from r in data.BanPhims select r;
        //    return View(all_key);
        //}
        ViewBag.Keyword = searchString;
            if (page == null) page = 1;
            var all_phim = (from BanPhim in data.BanPhims select BanPhim).OrderBy(m => m.maphim);
            if (!string.IsNullOrEmpty(searchString)) all_phim = (IOrderedQueryable<BanPhim>) all_phim.Where(a => a.tenphim.Contains(searchString));
        int pageSize = 8;
        int pageNum = page ?? 1;
                return View(all_phim.ToPagedList(pageNum, pageSize));
    }
        //public ActionResult Index(string sortProperty, string sortOrder)
        //{
        //    var BanPhim = from r in data.BanPhims select r;
        //    // 1. Tạo biến ViewBag SortOrder để giữ trạng thái sắp tăng hay giảm
        //    ViewBag.SortOrder = String.IsNullOrEmpty(sortOrder) ? "desc" : "";

        //    //2.Tạo câu truy vấn kết 2 bảng Book, Author, Category

        //    // 4. Tạo thuộc tính sắp xếp mặc định là "Title"
        //    if (String.IsNullOrEmpty(sortProperty)) sortProperty = "Title";

        //    // 5. Sắp xếp tăng/giảm bằng phương thức OrderBy sử dụng trong thư viện Dynamic LINQ
        //    if (sortOrder == "desc")
        //        BanPhim = BanPhim.OrderBy(sortProperty + " desc");
        //    else
        //        BanPhim = BanPhim.OrderBy(sortProperty);

        //    // 4. Trả kết quả về Views
        //    return View(BanPhim.ToList());
        //}
        public ActionResult Detail(int id)
    {
        var D_phim = data.BanPhims.Where(m => m.maphim == id).First();
        return View(D_phim);
    }
    public ActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Create(FormCollection collection, BanPhim s)
    {
            var E_maphim = Convert.ToInt32(collection["maphim"]);
            var E_tenphim = collection["tenphim"];
        var E_hinh = collection["hinh"];
        var E_giaban = Convert.ToDecimal(collection["giaban"]);
        var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
        var E_soluongton = Convert.ToInt32(collection["soluongton"]);
        if (string.IsNullOrEmpty(E_tenphim))
        {
            ViewData["Error"] = "Don't empty!";
        }
        else
        {

            s.maphim =E_maphim;
            s.tenphim = E_tenphim.ToString();
            s.hinh = E_hinh.ToString();
            s.giaban = E_giaban;
            s.ngaycapnhat = E_ngaycapnhat;
            s.soluongton = E_soluongton;
            data.BanPhims.InsertOnSubmit(s);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
        return this.Create();
    }
        public ActionResult Edit(int id)
    {
        var E_phimm = data.BanPhims.First(m => m.maphim == id);
        return View(E_phimm);
    }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }
        [HttpPost]
    public ActionResult Edit(int id, FormCollection collection)
    {
            //var E_maphim = collection["maphim"];
            var E_phim = data.BanPhims.First(m => m.maphim == id);
        var E_tenphim = collection["tenphim"];
        var E_hinh = collection["hinh"];
        var E_giaban = Convert.ToDecimal(collection["giaban"]);
        var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
        var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            var E_Hang = collection["Hang"];
            E_phim.maphim = id;
            if (string.IsNullOrEmpty(E_tenphim))
        {
            ViewData["Error"] = "Don't Empty";
        }
        else
        {
                E_phim.tenphim = E_tenphim;
                E_phim.hinh = E_hinh;
                E_phim.giaban = E_giaban;
                E_phim.ngaycapnhat = E_ngaycapnhat;
                E_phim.soluongton = E_soluongton;
            UpdateModel(E_phim);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
        return this.Edit(id);
    }
    public ActionResult Delete(int id)
    {
        var all_phim = data.BanPhims.First(m => m.maphim == id);
        return View(all_phim);
    }
    [HttpPost]
    public ActionResult Delete(int id, FormCollection collection)
    {
        var all_phim = data.BanPhims.Where(m => m.maphim == id).First();
        data.BanPhims.DeleteOnSubmit(all_phim);
        data.SubmitChanges();
        return RedirectToAction("Index");
    }
    public ActionResult TheLoai()
    {
        return View();
    }
        public ActionResult TongSoDonHangNguoiDung()
        {
            var all_sv = from ChiTietDonHang in data.ChiTietDonHangs select ChiTietDonHang;
            return View(all_sv);
        }
        public ActionResult DeleteDonHang(int id)
        {
            var all_phim = data.ChiTietDonHangs.First(m => m.madon == id);
            return View(all_phim);
        }
        [HttpPost]
        public ActionResult DeleteDonHang(int id, FormCollection collection)
        {
            var all_phim = data.ChiTietDonHangs.Where(m => m.madon == id).First();
            data.ChiTietDonHangs.DeleteOnSubmit(all_phim);
            data.SubmitChanges();
            return RedirectToAction("TongSoDonHangNguoiDung");
        }
        public ActionResult QL_KhachHang()
        {
            var all_kh = from KhachHang in data.KhachHangs select KhachHang;
            return View(all_kh);
        }
        public ActionResult DeleteKhachHang(int id)
        {
            var all_kh = data.KhachHangs.First(m => m.makh == id);
            return View(all_kh);
        }
        [HttpPost]
        public ActionResult DeleteKhachHang(int id, FormCollection collection)
        {
            var all_kh = data.KhachHangs.Where(m => m.makh == id).First();
            data.KhachHangs.DeleteOnSubmit(all_kh);
            data.SubmitChanges();
            return RedirectToAction("QL_KhachHang");
        }
        public ActionResult EditKhachHang(int id)
        {
            var E_kh = data.KhachHangs.First(m => m.makh == id);
            return View(E_kh);
        }
        [HttpPost]
        public ActionResult EditKhachHang(int makhh, FormCollection collection)
        {
            var E_makh = collection["makh"];
            var E_kh = data.KhachHangs.First(m => m.makh == makhh);
            var E_hoten = collection["hoten"];
            var E_tdn = collection["tendangnhap"];
            var E_matkhau = collection["matkhau"];
            var E_email = collection["email"];
            var E_diachi = collection["diachi"];
            var E_dienthoai = collection["dienthoai"];
            var E_ngaysinh = Convert.ToDateTime(collection["ngaysinh"]);
            E_kh.makh = makhh;
            if (string.IsNullOrEmpty(E_hoten))
            {
                ViewData["Error"] = "Don't Empty";
            }
            else
            {
                E_kh.hoten = E_hoten;
                E_kh.tendangnhap = E_tdn;
                E_kh.matkhau = E_matkhau;
                E_kh.email = E_email;
                E_kh.diachi = E_diachi;
                E_kh.dienthoai = E_dienthoai;
                E_kh.ngaysinh = E_ngaysinh;
                UpdateModel(E_kh);
                data.SubmitChanges();
                return RedirectToAction("QL_KhachHang");
            }
            return this.Edit(makhh);
        }
    }
}