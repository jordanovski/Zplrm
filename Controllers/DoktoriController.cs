using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ZplrmApp.Models;

namespace ZplrmApp.Controllers
{
    [Authorize]
    public class DoktoriController : Controller
    {
        private readonly ZplrmDbEntities db = new ZplrmDbEntities();

        //
        // GET: /Doktori/
        public ActionResult Index()
        {
            Session["CurrentUrlDoktorEdit"] = null;
            var doktori = db.Doktori.Include(d => d.Gradovi);
            return View(doktori.ToList());
        }

        //
        // GET: /Doktori/Details/5
        public ActionResult Details(string faksimil = null)
        {
            Session["CurrentUrlDoktorEdit"] = Request.Url.ToString();
            Doktori rabotilnici = db.Doktori.Find(faksimil);
            if (rabotilnici == null)
            {
                return HttpNotFound();
            }

            ViewBag.RabotilniciAktuelni = db.RabotilniciDoktori.Include(r => r.Doktori).Include(r => r.Rabotilnici)
                .Where(w => w.Rabotilnici.Datum >= DateTime.Today && w.DoktorFaksimil == faksimil).ToList().Select(s =>
                    new RabotilniciDoktoriViewModel
                    {
                        RabotilnicaDoktorId = s.RabotilnicaDoktorId,
                        RabotilnicaTema = s.Rabotilnici.Temi.TemaIme,
                        RabotilnicaDatum = s.Rabotilnici.Datum,
                        RabotilnicaOdDo = string.Format("{0} - {1}", string.Format("{0:hh\\:mm}", s.Rabotilnici.Pocetok), string.Format("{0:hh\\:mm}", s.Rabotilnici.Kraj)),
                        DoktorFaksimil = s.DoktorFaksimil,
                        DoktorImePrezime = s.Doktori.DoktorImePrezime
                    });
            ViewBag.RabotilniciPominati = db.RabotilniciDoktori.Include(r => r.Doktori).Include(r => r.Rabotilnici)
                .Where(w => w.Rabotilnici.Datum < DateTime.Today && w.DoktorFaksimil == faksimil).ToList().Select(s =>
                    new RabotilniciDoktoriViewModel
                    {
                        RabotilnicaDoktorId = s.RabotilnicaDoktorId,
                        RabotilnicaTema = s.Rabotilnici.Temi.TemaIme,
                        RabotilnicaDatum = s.Rabotilnici.Datum,
                        RabotilnicaOdDo = string.Format("{0} - {1}", string.Format("{0:hh\\:mm}", s.Rabotilnici.Pocetok), string.Format("{0:hh\\:mm}", s.Rabotilnici.Kraj)),
                        DoktorFaksimil = s.DoktorFaksimil,
                        DoktorImePrezime = s.Doktori.DoktorImePrezime
                    });
            
            return View(rabotilnici);
        }

        //
        // GET: /Doktori/Create
        public ActionResult Create()
        {
            ViewBag.GradId = new SelectList(db.Gradovi, "GradId", "GradIme");
            return View();
        }

        //
        // POST: /Doktori/Create
        [HttpPost]
        public ActionResult Create(Doktori doktori)
        {
            if (ModelState.IsValid)
            {
                db.Doktori.Add(doktori);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GradId = new SelectList(db.Gradovi, "GradId", "GradIme", doktori.GradId);
            return View(doktori);
        }

        //
        // GET: /Doktori/Edit/5
        public ActionResult Edit(string faksimil = null)
        {
            
            Doktori doktori = db.Doktori.Find(faksimil);
            if (doktori == null)
            {
                return HttpNotFound();
            }
            ViewBag.GradId = new SelectList(db.Gradovi.OrderBy(o => o.GradIme), "GradId", "GradIme", doktori.GradId);
            return View(doktori);
        }

        //
        // POST: /Doktori/Edit/5
        [HttpPost]
        public ActionResult Edit(Doktori doktori)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doktori).State = EntityState.Modified;
                db.SaveChanges();

                if (Session["CurrentUrlDoktorEdit"] == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Redirect(Session["CurrentUrlDoktorEdit"].ToString());
                }
            }
            ViewBag.GradId = new SelectList(db.Gradovi.OrderBy(o => o.GradIme), "GradId", "GradIme", doktori.GradId);
            return View(doktori);
        }


        //
        // GET: /Doktori/Delete/5
        public ActionResult Delete(string faksimil = null)
        {
            Doktori doktori = db.Doktori.Find(faksimil);
            if (doktori == null)
            {
                return HttpNotFound();
            }
            return View(doktori);
        }

        //
        // POST: /Doktori/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string faksimil)
        {
            Doktori doktori = db.Doktori.Find(faksimil);
            db.Doktori.Remove(doktori);
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