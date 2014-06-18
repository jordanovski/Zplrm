using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ZplrmApp.Models;

namespace ZplrmApp.Controllers
{
    [Authorize]
    public partial class RabotilniciController : Controller
    {
        private readonly ZplrmDbEntities db = new ZplrmDbEntities();

        //
        // GET: /Rabotilnici/
        public ActionResult Index()
        {
            return View(db.Rabotilnici.Include(r => r.Gradovi).Include(r => r.Temi).OrderBy(o => o.Datum).ToList());
        }



        //public ActionResult Products_Read([DataSourceRequest] DataSourceRequest request)
        //{
        //    var pom = db.Rabotilnici.Include(r => r.Gradovi).Include(r => r.Temi).ToList().Select(s => new RabotilniciViewModel
        //    {
        //        RabotilnicaId = s.RabotilnicaId,
        //        Tema = s.Temi.TemaIme,
        //        Grad = s.Gradovi.GradIme,
        //        Mesto = s.Mesto,
        //        Lokacija = s.Lokacija,
        //        Datum = s.Datum,
        //        OdDo = string.Format("{0} - {1}", string.Format("{0:hh\\:mm}", s.Pocetok), string.Format("{0:hh\\:mm}", s.Kraj)),
        //        Pocetok = string.Format("{0:hh\\:mm}", s.Pocetok),
        //        Kraj = string.Format("{0:hh\\:mm}", s.Kraj),
        //        Bodovi = s.Bodovi,
        //        OptimumPosetiteli = s.OptimumPosetiteli
        //    });
        //    return Json(pom.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}

        //
        // GET: /Doktori/Details/5


        public ActionResult Details(int id = 0)
        {
            Session["CurrentUrlDoktorEdit"] = Request.Url.ToString();
            Rabotilnici rabotilnica = db.Rabotilnici.Find(id);
            if (rabotilnica == null)
            {
                return HttpNotFound();
            }

            ViewBag.PrijaveniDoktori = db.RabotilniciDoktori.Include(r => r.Doktori).Include(r => r.Rabotilnici)
                .Where(w => w.RabotilnicaId == id).ToList().Select(s =>
                    new RabotilniciDoktoriViewModel
                    {
                        RabotilnicaDoktorId = s.RabotilnicaDoktorId,
                        RabotilnicaTema = s.Rabotilnici.Temi.TemaIme,
                        RabotilnicaDatum = s.Rabotilnici.Datum,
                        RabotilnicaOdDo = string.Format("{0} - {1}", string.Format("{0:hh\\:mm}", s.Rabotilnici.Pocetok), string.Format("{0:hh\\:mm}", s.Rabotilnici.Kraj)),
                        DoktorFaksimil = s.DoktorFaksimil,
                        DoktorImePrezime = s.Doktori.DoktorImePrezime
                    });
            
            return View(rabotilnica);
        }
        
        
        //
        // GET: /Rabotilnici/Create
        public ActionResult Create()
        {
            ViewBag.GradId = new SelectList(db.Gradovi, "GradId", "GradIme");
            ViewBag.TemaId = new SelectList(db.Temi, "TemaId", "TemaIme");
            return View();
        }

        //
        // POST: /Rabotilnici/Create
        [HttpPost]
        public ActionResult Create(Rabotilnici rabotilnici)
        {
            if (ModelState.IsValid)
            {
                db.Rabotilnici.Add(rabotilnici);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GradId = new SelectList(db.Gradovi, "GradId", "GradIme", rabotilnici.GradId);
            ViewBag.TemaId = new SelectList(db.Temi, "TemaId", "TemaIme", rabotilnici.TemaId);
            return View(rabotilnici);
        }



        //
        // GET: /Rabotilnici/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Rabotilnici rabotilnici = db.Rabotilnici.Find(id);
            if (rabotilnici == null)
            {
                return HttpNotFound();
            }
            ViewBag.GradId = new SelectList(db.Gradovi, "GradId", "GradIme", rabotilnici.GradId);
            ViewBag.TemaId = new SelectList(db.Temi, "TemaId", "TemaIme", rabotilnici.TemaId);
            return View(rabotilnici);
        }

        //
        // POST: /Rabotilnici/Edit/5
        [HttpPost]
        public ActionResult Edit(Rabotilnici rabotilnici)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rabotilnici).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GradId = new SelectList(db.Gradovi, "GradId", "GradIme", rabotilnici.GradId);
            ViewBag.TemaId = new SelectList(db.Temi, "TemaId", "TemaIme", rabotilnici.TemaId);
            return View(rabotilnici);
        }



        //
        // GET: /Rabotilnici/Delete/5
        public ActionResult Delete(int id = 0)
        {
            Rabotilnici rabotilnici = db.Rabotilnici.Find(id);
            if (rabotilnici == null)
            {
                return HttpNotFound();
            }
            return View(rabotilnici);
        }

        //
        // POST: /Rabotilnici/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Rabotilnici rabotilnici = db.Rabotilnici.Find(id);
            db.Rabotilnici.Remove(rabotilnici);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}