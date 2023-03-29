using KeyBoardStore.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace KeyBoardStore.Controllers
{
    public class TheLoaiController : Controller
    {
        dbBanPhimDataContext data = new dbBanPhimDataContext();
        // GET: TheLoai
        public ActionResult Index(int? page, string searchString)
        {
            ViewBag.Keyword = searchString;
            if (page == null) page = 1;
            var all_phim = (from BanPhim in data.BanPhims select BanPhim).OrderBy(m => m.maphim);
            if (!string.IsNullOrEmpty(searchString)) all_phim = (IOrderedQueryable<BanPhim>)all_phim.Where(a => a.tenphim.Contains(searchString));
            int pageSize = 8;
            int pageNum = page ?? 1;
            return View(all_phim.ToPagedList(pageNum, pageSize));
        }
        //        public ActionResult Detail(int id)
        //        {
        //            var D_phim = data.BanPhims.Where(m => m.maphim == id).First();
        //            return View(D_phim);
        //        }
        //        public ActionResult Create()
        //        {
        //            return View();
        //        }
        //        [HttpPost]
        //        public ActionResult Create(FormCollection collection, BanPhim s)
        //        {
        //            var E_tenphim = collection["tenphim"];
        //            var E_hinh = collection["hinh"];
        //            var E_giaban = Convert.ToDecimal(collection["giaban"]);
        //            var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
        //            var E_soluongton = Convert.ToInt32(collection["soluongton"]);
        //            if (string.IsNullOrEmpty(E_tenphim))
        //            {
        //                ViewData["Error"] = "Don't empty!";
        //            }
        //            else
        //            {
        //                s.tenphim = E_tenphim.ToString();
        //                s.hinh = E_hinh.ToString();
        //                s.giaban = E_giaban;
        //                s.ngaycapnhat = E_ngaycapnhat;
        //                s.soluongton = E_soluongton;
        //                data.BanPhims.InsertOnSubmit(s);
        //                data.SubmitChanges();
        //                return RedirectToAction("Index");
        //            }
        //            return this.Create();
        //        }
        //        public ActionResult Edit(int id)
        //        {
        //            var E_phimm = data.BanPhims.First(m => m.maphim == id);
        //            return View(E_phimm);
        //        }
        //        public string ProcessUpload(HttpPostedFileBase file)
        //        {
        //            if (file == null)
        //            {
        //                return "";
        //            }
        //            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
        //            return "/Content/images/" + file.FileName;
        //        }
        //        [HttpPost]
        //        public ActionResult Edit(int maloai, FormCollection collection)
        //        {
        //            var E_phim = data.BanPhims.First(m => m.maphim == maloai);
        //            var E_tenphim = collection["tenphim"];
        //            var E_hinh = collection["hinh"];
        //            var E_giaban = Convert.ToDecimal(collection["giaban"]);
        //            var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
        //            var E_soluongton = Convert.ToInt32(collection["soluongton"]);
        //            if (string.IsNullOrEmpty(E_tenphim))
        //            {
        //                ViewData["Error"] = "Don't Empty";
        //            }
        //            else
        //            {
        //                E_phim.tenphim = E_tenphim;
        //                E_phim.hinh = E_hinh;
        //                E_phim.giaban = E_giaban;
        //                E_phim.ngaycapnhat = E_ngaycapnhat;
        //                E_phim.soluongton = E_soluongton;
        //                UpdateModel(E_phim);
        //                data.SubmitChanges();
        //                return RedirectToAction("Index");
        //            }
        //            return this.Edit(maloai);
        //        }
        //        public ActionResult Delete(int id)
        //        {
        //            var all_phim = data.BanPhims.First(m => m.maloai == id);
        //            return View(all_phim);
        //        }
        //        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var all_phim = data.BanPhims.Where(m => m.maloai == id).First();
            data.BanPhims.DeleteOnSubmit(all_phim);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult TheLoai()
        {
            return View();
        }
    }
}