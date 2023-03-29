using KeyBoardStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace KeyBoardStore.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        dbBanPhimDataContext data = new dbBanPhimDataContext();
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["hoten"];
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            var MatKhauXacNhan = collection["MatKhauXacNhan"];
            var email = collection["email"];
            var diachi = collection["diachi"];
            var dienthoai = collection["dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["ngaysinh"]);
            if (string.IsNullOrEmpty(MatKhauXacNhan))
            {
                ViewData["NhapMKXN"] = "Phải nhập mật khẩu xác nhận!";
            }
            else
            {
                if (!matkhau.Equals(MatKhauXacNhan))
                {
                    ViewData["MatKhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận phải giống nhau";
                }
                else
                {
                    kh.hoten = hoten;
                    kh.tendangnhap = tendangnhap;
                    kh.matkhau = matkhau;
                    kh.email = email;
                    kh.diachi = diachi;
                    kh.dienthoai = dienthoai;
                    kh.ngaysinh = DateTime.Parse(ngaysinh);
                    data.KhachHangs.InsertOnSubmit(kh);
                    data.SubmitChanges();
                    return RedirectToAction("DangNhap");
                }
            }
            return this.DangKy();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            KhachHang kh = data.KhachHangs.SingleOrDefault(n => n.tendangnhap == tendangnhap && n.matkhau == matkhau);
            if (kh != null)
            {
                if (kh.tendangnhap == "supermouse0985")
                {
                    return RedirectToAction("Index", "BanPhim");
                }

                //ViewBag.ThongBao = "Dang nhap thanh cong!";
                ViewData["Thông báo"] = "Abưadwadwđ";
                Session["TaiKhoan"] = kh;
                return RedirectToAction("Home", "Home");
            }
            else
            {
                ViewBag.ThongBao = String.Format("ten dang nhap hoac mat khau khong dung");
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            //if (kh.tendangnhap == "")
            //{
            //    ViewData["Thông bao"] = "Abưadwadwđ";
            //    return RedirectToAction("DangNhap", "NguoiDung");
            //}
        }
        public ActionResult DangXuat()
        {
            Session.Abandon();
            return RedirectToAction("DangNhap", "NguoiDung");

        }
    }
}