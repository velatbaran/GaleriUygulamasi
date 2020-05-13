using GaleriUygulamasi.Models;
using GaleriUygulamasi.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GaleriUygulamasi.Controllers
{
    public class HomeController : Controller
    {
        private GaleriEntities context = new GaleriEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Galeri()
        {
            return View();
        }

        public ActionResult AjaxJson()
        {
            return View();
        }

        public ActionResult OgrenciKayit(Ogrenci ogrenci)
        {
            return Json(ogrenci, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Upload()
        {
            if(Session["value"] != null)
            {
                Response.Redirect("/");// anasayfa ya gider 
            }
            return View();
        }

        // dosya adında nokta olmaması gerekiyor.
        public FileContentResult FileView(int? id ,string dosyaAdi)
        {
            var List = context.Dosyas.Where(x => x.Id == id.Value).ToList();
            return new FileContentResult(List[0].Deger, List[0].DosyaTipi);
        }

        //public ActionResult FileDownload(int? id)
        //{
        //    var List = context.Dosyas.Where(x => x.Id == id.Value).ToList();
        //    return File(List[0].Deger, "application/zip", List[0].DosyaAdi);
        //}

        public ActionResult FileUpload()
        {
            HttpPostedFileBase file = HttpContext.Request.Files[0];
            using (BinaryReader reader = new BinaryReader(file.InputStream))
            {
                byte[] value = reader.ReadBytes((Int32)file.ContentLength);
                if (Session["value"] == null)
                    Session["value"] = value;
                else
                    Session["value"] = UtilityManager.ByteBirlestir((byte[])Session["value"], value);

                if (10000 > file.ContentLength)
                {
                    context.Dosyas.Add(new Dosya
                    {
                        Deger = (byte[])Session["value"],
                        //DosyaAdi = file.FileName.Replace(".",""),  // nokta hatası verdiği için noktaları sildik
                        DosyaAdi = file.FileName,
                        DosyaBoyutu = ((byte[])Session["value"]).Length.ToString(),
                        DosyaTipi = file.ContentType,
                        Ikon = UtilityManager.SetIcon(file.ContentType),
                        BoyutKisaltma = UtilityManager.BytesToString(((byte[])Session["value"]).Length),
                        Renk = UtilityManager.SetClass(file.ContentType),
                        KayitTarihi = DateTime.Now

                    });

                    context.SaveChanges();
                    Session["value"] = null;
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetFileDetailById(int? id)
        {
            var file = (from d in context.Dosyas
                        where d.Id == id.Value
                        select new
                        {
                            d.BoyutKisaltma,
                            d.DosyaAdi,
                            d.DosyaTipi,
                            UrlYolu = "/Home/FileView/" + d.Id + "/" + d.Baslik,
                            //UrlYolu = "/Home/FileView/" + d.Id + "/" + d.DosyaAdi,
                            //UrlYolu = "/Home/FileDownoad/" + d.Id + "/" + d.DosyaAdi,
                            d.Baslik,
                            d.Aciklama
                        }).ToList();

            return Json(file[0], JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateFile(UpFile file)
        {
            try
            {
                var entity = context.Dosyas.Where(x => x.Id == file.Id).SingleOrDefault();
                if (entity != null)
                {
                    entity.Baslik = file.Baslik;
                    entity.Aciklama = file.Aciklama;
                    context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return Json(new { hasError = false, Message = "Güncelleme işlemi başarılı" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { hasError = true, Message = "Güncelleme işlemi başarısız!" }, JsonRequestBehavior.AllowGet);
            }

            return View(file);
        }
    }
}