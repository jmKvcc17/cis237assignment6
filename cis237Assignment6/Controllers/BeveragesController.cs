using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237Assignment6.Models;

namespace cis237Assignment6.Controllers
{
    [Authorize]
    public class BeveragesController : Controller
    {
        //private BeverageJMeachumEntities db = new BeverageJMeachumEntities();
        private BeverageJMeachumEntities1 db = new BeverageJMeachumEntities1();
        
        // GET: Beverages
        public ActionResult Index()
        {
            DbSet<Beverage> BeveragesToFilter = db.Beverages;

            string filterName = "";
            string filterPack = "";
            string filterMin = "";
            string filterMax = "";

            decimal min = 0;
            decimal max = 100; // **********************************

            if (!string.IsNullOrWhiteSpace((string)Session["name"]))
            {
                filterName = (string)Session["name"];
            }

            if (!string.IsNullOrWhiteSpace((string)Session["pack"]))
            {
                filterPack = (string)Session["pack"];
            }

            if (!string.IsNullOrWhiteSpace((string)Session["min"]))
            {
                filterMin = (string)Session["min"];
                min = decimal.Parse(filterMin);
            }

            if (!string.IsNullOrWhiteSpace((string)Session["max"]))
            {
                filterMax = (string)Session["max"];
                max = decimal.Parse(filterMax);
            }

            IEnumerable<Beverage> filtered = BeveragesToFilter.Where(beverage => beverage.price >= min
                && beverage.price <= max && beverage.name.Contains(filterName)
                && beverage.pack.Contains(filterPack));

            IEnumerable<Beverage> finalFiltered = filtered.ToList();

            ViewBag.filterName = filterName;
            ViewBag.filterPack = filterPack;
            ViewBag.filterMin = filterMin;
            ViewBag.filterMax = filterMax;

            return View(finalFiltered);

            //return View(db.Beverages.ToList());
        }

        // GET: Beverages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // GET: Beverages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beverages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                if (!SearchDB(beverage))
                {
                    db.Beverages.Add(beverage);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    return View("Error"); // *****************Redirect to new view (error page)
                
            }

            return View(beverage);
        }

        private bool SearchDB(Beverage SearchBev)
        {

            if (db.Beverages.Find(SearchBev.id) == null)
                return false;
            else
                return true;
        }

        // GET: Beverages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beverage);
        }

        // GET: Beverages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Beverage beverage = db.Beverages.Find(id);
            db.Beverages.Remove(beverage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter()
        {
            string name = Request.Form.Get("name");
            string pack = Request.Form.Get("pack");
            string min = Request.Form.Get("min");
            string max = Request.Form.Get("max");

            Session["name"] = name;
            Session["pack"] = pack;
            Session["min"] = min;
            Session["max"] = max;

            return RedirectToAction("Index");
        }
    }
}
