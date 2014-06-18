using System.Data;
using System.Linq;
using System.Web.Mvc;
using ZplrmApp.Models;

namespace ZplrmApp.Controllers
{
    [Authorize]
    public class GradoviController : Controller
    {
        private readonly ZplrmDbEntities db = new ZplrmDbEntities();

        //
        // GET: /Gradovi/

        public ActionResult Index()
        {
            return View(db.Gradovi.OrderBy(o=>o.GradIme).ToList());
        }

        //
        // GET: /Gradovi/Details/5

        public ActionResult Details(int id = 0)
        {
            Gradovi gradovi = db.Gradovi.Find(id);
            if (gradovi == null)
            {
                return HttpNotFound();
            }
            return View(gradovi);
        }

        //
        // GET: /Gradovi/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Gradovi/Create

        [HttpPost]
        public ActionResult Create(Gradovi gradovi)
        {
            if (ModelState.IsValid)
            {
                db.Gradovi.Add(gradovi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gradovi);
        }

        //
        // GET: /Gradovi/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Gradovi gradovi = db.Gradovi.Find(id);
            if (gradovi == null)
            {
                return HttpNotFound();
            }
            return View(gradovi);
        }

        //
        // POST: /Gradovi/Edit/5

        [HttpPost]
        public ActionResult Edit(Gradovi gradovi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gradovi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gradovi);
        }

        //
        // GET: /Gradovi/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Gradovi gradovi = db.Gradovi.Find(id);
            if (gradovi == null)
            {
                return HttpNotFound();
            }
            return View(gradovi);
        }

        //
        // POST: /Gradovi/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Gradovi gradovi = db.Gradovi.Find(id);
            db.Gradovi.Remove(gradovi);
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