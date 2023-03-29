using KeyBoardStore.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Data.SqlClient;

namespace KeyBoardStore.Controllers
{
    public class HomeController : Controller
    {
        dbBanPhimDataContext data = new dbBanPhimDataContext(); //datadb context
        public ActionResult Home(int? page, string searchString, string sortProperty, string sortOrder)
        {
            //var all_phim = from BanPhim in db.BanPhims select BanPhim;
            //return View(all_phim);
            ViewBag.Keyword = searchString;
            if (page == null) page = 1;
            var BanPhims = (from BanPhim in data.BanPhims select BanPhim).OrderBy(m => m.maphim);
            if (!string.IsNullOrEmpty(searchString)) BanPhims = (IOrderedQueryable<BanPhim>)BanPhims.Where(a => a.tenphim.Contains(searchString));
            int pageSize = 6;
            int pageNum = page ?? 1;
            ViewBag.SortOrder = String.IsNullOrEmpty(sortOrder) ? "desc" : "";

            //2.Tạo câu truy vấn kết 2 bảng Book, Author, Category

            // 4. Tạo thuộc tính sắp xếp mặc định là "Title"
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "maphim";
            
            // 5. Sắp xếp tăng/giảm bằng phương thức OrderBy sử dụng trong thư viện Dynamic LINQ
            if (sortOrder == "desc")
                BanPhims = (IOrderedQueryable<BanPhim>)BanPhims.OrderBy(sortProperty + " desc");
            else
                BanPhims = (IOrderedQueryable<BanPhim>)BanPhims.OrderBy(sortProperty);

            return View(BanPhims.ToPagedList(pageNum, pageSize));
        }
        //public ActionResult Home(string sortProperty, string sortOrder)
        //{
        //    var BanPhim =data.BanPhims.OrderBy(x => x.tenphim);
        //    // 1. Tạo biến ViewBag SortOrder để giữ trạng thái sắp tăng hay giảm
        //    ViewBag.SortOrder = String.IsNullOrEmpty(sortOrder) ? "desc" : "";

        //    //2.Tạo câu truy vấn kết 2 bảng Book, Author, Category

        //    // 4. Tạo thuộc tính sắp xếp mặc định là "Title"
        //    if (String.IsNullOrEmpty(sortProperty)) sortProperty = "tenphim";

        //    // 5. Sắp xếp tăng/giảm bằng phương thức OrderBy sử dụng trong thư viện Dynamic LINQ
        //    if (sortOrder == "desc")
        //        BanPhim = (IOrderedQueryable<BanPhim>)BanPhim.OrderBy(sortProperty + "desc");
        //    else
        //        BanPhim = (IOrderedQueryable<BanPhim>)BanPhim.OrderBy(sortProperty);

        //    // 4. Trả kết quả về Views
        //    return View(BanPhim);
        //}
        public ActionResult Detail(int id)
        {
            var D_phim = data.BanPhims.Where(m => m.maphim == id).First();
            return View(D_phim);
        }
        //public ActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Create(FormCollection collection, BanPhim s)
        //{
        //    //var E_MaPhim = collection["MaPhim"];
        //    var E_tenphim = collection["tenphim"];
        //    var E_hinh = collection["hinh"];
        //    var E_giaban = Convert.ToDecimal(collection["giaban"]);
        //    var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
        //    var E_soluongton = Convert.ToInt32(collection["soluongton"]);
        //    if (string.IsNullOrEmpty(E_tenphim))
        //    {
        //        ViewData["Error"] = "Don't empty!";
        //    }
        //    else
        //    {
        //        //s.maphim = E_MaPhim.();
        //        s.tenphim = E_tenphim.ToString();
        //        s.hinh = E_hinh.ToString();
        //        s.giaban = E_giaban;
        //        s.ngaycapnhat = E_ngaycapnhat;
        //        s.soluongton = E_soluongton;
        //        data.BanPhims.InsertOnSubmit(s);
        //        data.SubmitChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return this.Create();
        //}
        //public ActionResult Edit(int id)
        //{
        //    var E_phimm = data.BanPhims.First(m => m.maphim == id);
        //    return View(E_phimm);
        //}
        //public string ProcessUpload(HttpPostedFileBase file)
        //{
        //    if (file == null)
        //    {
        //        return "";
        //    }
        //    file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
        //    return "/Content/images/" + file.FileName;
        //}
        //[HttpPost]
        //public ActionResult Edit(int maloai, FormCollection collection)
        //{
        //    var E_phim = data.BanPhims.First(m => m.maphim == maloai);
        //    var E_tenphim = collection["tenphim"];
        //    var E_hinh = collection["hinh"];
        //    var E_giaban = Convert.ToDecimal(collection["giaban"]);
        //    var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
        //    var E_soluongton = Convert.ToInt32(collection["soluongton"]);
        //    if (string.IsNullOrEmpty(E_tenphim))
        //    {
        //        ViewData["Error"] = "Don't Empty";
        //    }
        //    else
        //    {
        //        E_phim.tenphim = E_tenphim;
        //        E_phim.hinh = E_hinh;
        //        E_phim.giaban = E_giaban;
        //        E_phim.ngaycapnhat = E_ngaycapnhat;
        //        E_phim.soluongton = E_soluongton;
        //        UpdateModel(E_phim);
        //        data.SubmitChanges();
        //        return RedirectToAction("Home");
        //    }
        //    return this.Edit(maloai);
        //}
        //public ActionResult Delete(int id)
        //{
        //    var all_phim = data.BanPhims.First(m => m.maloai == id);
        //    return View(all_phim);
        //}
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection formCollection)
        //{
        //    var all_phim = data.BanPhims.Where(m => m.maloai == id).First();
        //    data.BanPhims.DeleteOnSubmit(all_phim);
        //    data.SubmitChanges();
        //    return RedirectToAction("Home");
        //}
        public ActionResult TheLoai()
        {
            return View();
        }
    }
}