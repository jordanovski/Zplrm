using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ZplrmApp.Models;

namespace ZplrmApp.Controllers
{
    [Authorize]
    public class PrezenteriController : Controller
    {
        private readonly ZplrmDbEntities db = new ZplrmDbEntities();

        //
        // GET: /Prezenteri/
        public ActionResult Index()
        {
            var prezenteri = db.Prezenteri.Include(p => p.Gradovi);
            return View(prezenteri.ToList());
        }



        //
        // GET: /Prezenteri/Details/5
        public ActionResult Details(string faksimil = null)
        {
            Prezenteri prezenteri = db.Prezenteri.Find(faksimil);
            if (prezenteri == null)
            {
                return HttpNotFound();
            }
            return View(prezenteri);
        }



        //
        // GET: /Prezenteri/Create
        public ActionResult Create()
        {
            ViewBag.GradId = new SelectList(db.Gradovi, "GradId", "GradIme");
            return View();
        }

        //
        // POST: /Prezenteri/Create
        [HttpPost]
        public ActionResult Create(Prezenteri prezenteri)
        {
            if (ModelState.IsValid)
            {
                db.Prezenteri.Add(prezenteri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GradId = new SelectList(db.Gradovi, "GradId", "GradIme", prezenteri.GradId);
            return View(prezenteri);
        }



        //
        // GET: /Prezenteri/Edit/5
        public ActionResult Edit(string faksimil = null)
        {
            Prezenteri prezenteri = db.Prezenteri.Find(faksimil);
            if (prezenteri == null)
            {
                return HttpNotFound();
            }
            ViewBag.GradId = new SelectList(db.Gradovi, "GradId", "GradIme", prezenteri.GradId);
            return View(prezenteri);
        }

        //
        // POST: /Prezenteri/Edit/5
        [HttpPost]
        public ActionResult Edit(Prezenteri prezenteri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prezenteri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GradId = new SelectList(db.Gradovi, "GradId", "GradIme", prezenteri.GradId);
            return View(prezenteri);
        }



        //
        // GET: /Prezenteri/Delete/5
        public ActionResult Delete(string faksimil = null)
        {
            Prezenteri prezenteri = db.Prezenteri.Find(faksimil);
            if (prezenteri == null)
            {
                return HttpNotFound();
            }
            return View(prezenteri);
        }

        //
        // POST: /Prezenteri/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string faksimil)
        {
            Prezenteri prezenteri = db.Prezenteri.Find(faksimil);
            db.Prezenteri.Remove(prezenteri);
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