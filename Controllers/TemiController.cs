using System.Data;
using System.Linq;
using System.Web.Mvc;
using ZplrmApp.Models;

namespace ZplrmApp.Controllers
{
    [Authorize]
    public class TemiController : Controller
    {
        private readonly ZplrmDbEntities db = new ZplrmDbEntities();

        //
        // GET: /Temi/

        public ActionResult Index()
        {
            return View(db.Temi.ToList());
        }

        //
        // GET: /Temi/Details/5

        public ActionResult Details(int id = 0)
        {
            Temi temi = db.Temi.Find(id);
            if (temi == null)
            {
                return HttpNotFound();
            }
            return View(temi);
        }

        //
        // GET: /Temi/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Temi/Create

        [HttpPost]
        public ActionResult Create(Temi temi)
        {
            if (ModelState.IsValid)
            {
                db.Temi.Add(temi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(temi);
        }

        //
        // GET: /Temi/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Temi temi = db.Temi.Find(id);
            if (temi == null)
            {
                return HttpNotFound();
            }
            return View(temi);
        }

        //
        // POST: /Temi/Edit/5

        [HttpPost]
        public ActionResult Edit(Temi temi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(temi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(temi);
        }

        //
        // GET: /Temi/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Temi temi = db.Temi.Find(id);
            if (temi == null)
            {
                return HttpNotFound();
            }
            return View(temi);
        }

        //
        // POST: /Temi/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Temi temi = db.Temi.Find(id);
            db.Temi.Remove(temi);
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