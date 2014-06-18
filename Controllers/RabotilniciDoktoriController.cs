using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ZplrmApp.Models;

namespace ZplrmApp.Controllers
{
    [Authorize]
    public class RabotilniciDoktoriController : Controller
    {
        private readonly ZplrmDbEntities db = new ZplrmDbEntities();

        //
        // GET: /RabotilniciDoktori/
        public ActionResult Index()
        {
            Session["CurrentUrlDoktorEdit"] = null;
            Session[""] = null;
            var rabotilnicidoktori = db.RabotilniciDoktori.Include(r => r.Doktori).Include(r => r.Rabotilnici)
                .Where(w => w.Rabotilnici.Datum >= DateTime.Today).ToList().Select(s =>
                new RabotilniciDoktoriViewModel
                {
                    RabotilnicaDoktorId = s.RabotilnicaDoktorId,
                    RabotilnicaTema = s.Rabotilnici.Temi.TemaIme,
                    RabotilnicaDatum = s.Rabotilnici.Datum,
                    RabotilnicaOdDo = string.Format("{0} - {1}",
                        string.Format("{0:hh\\:mm}", s.Rabotilnici.Pocetok),
                        string.Format("{0:hh\\:mm}", s.Rabotilnici.Kraj)),
                    DoktorFaksimil = s.DoktorFaksimil,
                    DoktorImePrezime = s.Doktori.DoktorImePrezime
                });
            
            return View(rabotilnicidoktori);
        }

        public ActionResult BackToList()
        {
            if (Session["CurrentUrlDoktorEdit"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect(Session["CurrentUrlDoktorEdit"].ToString());
            }
        }

        //
        // GET: /RabotilniciDoktori/Delete/5
        public ActionResult Delete(int id = 0)
        {
            RabotilniciDoktori rabotilnicidoktori = db.RabotilniciDoktori.Find(id);
            if (rabotilnicidoktori == null)
            {
                return HttpNotFound();
            }
            return View(rabotilnicidoktori);
        }

        //
        // POST: /RabotilniciDoktori/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            RabotilniciDoktori rabotilnicidoktori = db.RabotilniciDoktori.Find(id);
            var rabotilnica = db.Rabotilnici.First(w => w.RabotilnicaId == rabotilnicidoktori.RabotilnicaId);
            if(rabotilnica.OptimumPosetiteli!=null)
                rabotilnica.OptimumPosetiteli++;
            
            db.RabotilniciDoktori.Remove(rabotilnicidoktori);
            db.SaveChanges();
            if (Session["CurrentUrlDoktorEdit"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect(Session["CurrentUrlDoktorEdit"].ToString());
            }
            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}