using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZplrmApp.Models;
using System.Data.Entity;
using PagedList;

namespace ZplrmApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ZplrmDbEntities db = new ZplrmDbEntities();
        private int selectedRabotilnicaId = 0;

        public ActionResult Index(int page=1)
        {
            var rabotilnici = db.Rabotilnici.Include(r => r.Gradovi).Include(r => r.Temi).Include(r => r.RabotilniciPrezenteri).Where(w=>w.Datum>=DateTime.Today);
            return View(rabotilnici.OrderBy(o => o.Datum).ToPagedList(page, 10));
        }


        public ActionResult Prijavuvaje(int id = 0)
        {
            Session["CurrentUrl"] = Request.Url.ToString();
            Rabotilnici rabotilnici = db.Rabotilnici.Find(id);
            if (rabotilnici == null)
            {
                return HttpNotFound();
            }
            selectedRabotilnicaId = id;
            ViewBag.DoktorFaksimil = new SelectList(db.Prezenteri, "DoktorFaksimil", "PrezenterIme");
            ViewBag.Tema = rabotilnici.Temi.TemaIme;
            ViewBag.Lokacija = rabotilnici.Gradovi.GradIme + ", " + rabotilnici.Mesto + ", " + rabotilnici.Lokacija;
            ViewBag.Datum = rabotilnici.Datum.ToShortDateString();
            ViewBag.Vreme = string.Format("{0:hh\\:mm}", rabotilnici.Pocetok) + " - " + string.Format("{0:hh\\:mm}", rabotilnici.Kraj);
            ViewBag.SlobodniMesta = rabotilnici.OptimumPosetiteli;

            ViewBag.Uspesno = false;
            ViewBag.FaksimilNePostoi = false;
            ViewBag.VekeRegistriran = false;
            ViewBag.NemaSlobodniMesta = false;

            var doktori = db.RabotilniciDoktori.Where(w => w.RabotilnicaId == id).Select(s => s.DoktorFaksimil);
            ViewBag.Doktori=db.Doktori.Where(w => doktori.Contains(w.DoktorFaksimil));

            return View();
        }
        [HttpPost]
        public ActionResult Prijavuvaje(int id, RabotilniciDoktori rabotilnicaDoktor)
        {
            ViewBag.Uspesno = false;
            ViewBag.FaksimilNePostoi = false;
            ViewBag.VekeRegistriran = false;
            ViewBag.NemaSlobodniMesta = false;

            Rabotilnici rabotilnica = db.Rabotilnici.Find(id);
            if (rabotilnica == null)
            {
                return HttpNotFound();
            }

            Doktori doktor = db.Doktori.Find(rabotilnicaDoktor.DoktorFaksimil);
            if (doktor == null)
            {
                ViewBag.FaksimilNePostoi = true;
            }
            else
            {
                if (db.RabotilniciDoktori.Any(w => (w.DoktorFaksimil == rabotilnicaDoktor.DoktorFaksimil && w.RabotilnicaId == id)))
                {
                    ViewBag.VekeRegistriran = true;
                }
                else
                {
                    if (rabotilnica.OptimumPosetiteli == 0)
                    {
                        ViewBag.NemaSlobodniMesta = true;
                    }
                    else
                    {
                        if (rabotilnica.OptimumPosetiteli > 0)
                        {
                            rabotilnica.OptimumPosetiteli--;
                        }
                        rabotilnicaDoktor.RabotilnicaId = id;
                        rabotilnicaDoktor.VremePrijava = DateTime.Now;
                        if (ModelState.IsValid)
                        {
                            db.RabotilniciDoktori.Add(rabotilnicaDoktor);
                            db.SaveChanges();

                            PopulateViewBag(rabotilnica, id);

                            ViewBag.Uspesno = true;

                            ModelState.Clear();
                            return View();
                        }
                    }
                }
            }

            PopulateViewBag(rabotilnica, id);
            return View(rabotilnicaDoktor);
        }


        #region Регистрирање со факсимил

            public ActionResult SoFaksimil()
            {
                ViewBag.GradId = new SelectList(db.Gradovi, "GradId", "GradIme");
                return View();
            }

            [HttpPost]
            public ActionResult SoFaksimil(Doktori doktori)
            {
                if (doktori.DoktorFaksimil == null)
                {
                    ModelState.AddModelError("DoktorFaksimil", "Ова поле е задолжително.");
                }
                if (doktori.DoktorImePrezime == null)
                {
                    ModelState.AddModelError("DoktorImePrezime", "Ова поле е задолжително.");
                }

                if (ModelState.IsValid)
                {
                    db.Doktori.Add(doktori);
                    db.SaveChanges();
                    return Session["CurrentUrl"] == null ? Index() : Redirect(Session["CurrentUrl"].ToString());
                }

                ViewBag.GradId = new SelectList(db.Gradovi, "GradId", "GradIme", doktori.GradId);
                return View(doktori);
            }

        #endregion


        #region Регистрирање без факсимил

            public ActionResult BezFaksimil()
            {                
                Doktori d = new Doktori();
                d.DoktorFaksimil = GetFaksimil();
                ViewBag.GradId = new SelectList(db.Gradovi, "GradId", "GradIme");
                
                return View(d);
            }  

            [HttpPost]
            public ActionResult BezFaksimil(Doktori doktori)
            {
                doktori.DoktorFaksimil = GetFaksimil();
                if (doktori.DoktorImePrezime == null)
                {
                    ModelState.AddModelError("DoktorImePrezime", "Ова поле е задолжително.");
                }

                if (ModelState.IsValid)
                {
                    db.Doktori.Add(doktori);
                    db.SaveChanges(); 
                    return Session["CurrentUrl"] == null ? Index() : Redirect(Session["CurrentUrl"].ToString());
                }

                ViewBag.GradId = new SelectList(db.Gradovi, "GradId", "GradIme", doktori.GradId);
                return View(doktori);
            }

            private string GetFaksimil()
            {
                var maxFaksimilDB = db.Doktori.Where(w => w.DoktorFaksimil.StartsWith("x")).Select(s => s.DoktorFaksimil.Substring(1, 5)).Max();
                var maxFaksimil=(int.Parse(maxFaksimilDB)+1);
                var faksimil = "x";

                for (var i = maxFaksimil.ToString().Length; i < 5; i++)
                {
                    faksimil = faksimil + "0";
                }
                faksimil = faksimil + "{0}";
                string result = string.Format(faksimil, maxFaksimil.ToString());

                return result;
            }

        #endregion

       

        public ActionResult Details(int id)
        {
            Rabotilnici rabotilnici = db.Rabotilnici.Find(id);
            if (rabotilnici == null)
            {
                return HttpNotFound();
            }
            selectedRabotilnicaId = id;
            ViewBag.DoktorFaksimil = new SelectList(db.Prezenteri, "DoktorFaksimil", "PrezenterIme");
            ViewBag.Tema = rabotilnici.Temi.TemaIme;
            ViewBag.Lokacija = rabotilnici.Gradovi.GradIme + ", " + rabotilnici.Mesto + ", " + rabotilnici.Lokacija;
            ViewBag.Datum = rabotilnici.Datum.ToShortDateString();
            ViewBag.Vreme = string.Format("{0:hh\\:mm}", rabotilnici.Pocetok) + " - " + string.Format("{0:hh\\:mm}", rabotilnici.Kraj);
            ViewBag.SlobodniMesta = rabotilnici.OptimumPosetiteli;
            ViewBag.Uspesno = false;

            var doktori = db.RabotilniciDoktori.Where(w => w.RabotilnicaId == id).Select(s => s.DoktorFaksimil);
            ViewBag.Doktori = db.Doktori.Where(w => doktori.Contains(w.DoktorFaksimil));

            return View();
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        private void PopulateViewBag(Rabotilnici rabotilnica, int id)
        {
            ViewBag.DoktorFaksimil = new SelectList(db.Prezenteri, "DoktorFaksimil", "PrezenterIme");

            ViewBag.Tema = rabotilnica.Temi.TemaIme;
            ViewBag.Lokacija = rabotilnica.Gradovi.GradIme + ", " + rabotilnica.Mesto + ", " + rabotilnica.Lokacija;
            ViewBag.Datum = rabotilnica.Datum.ToShortDateString();
            ViewBag.Vreme = string.Format("{0:hh\\:mm}", rabotilnica.Pocetok) + " - " + string.Format("{0:hh\\:mm}", rabotilnica.Kraj);
            ViewBag.SlobodniMesta = rabotilnica.OptimumPosetiteli;

            var doktori1 = db.RabotilniciDoktori.Where(w => w.RabotilnicaId == id).Select(s => s.DoktorFaksimil);
            ViewBag.Doktori = db.Doktori.Where(w => doktori1.Contains(w.DoktorFaksimil));
        }
    }
}
