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
            // Holds the entire database
            DbSet<Beverage> BeveragesToFilter = db.Beverages;

            // used to hold the values from the session
            string filterName = "";
            string filterPack = "";
            string filterMin = "";
            string filterMax = "";

            // Default max/min values to filter price
            decimal min = 0;
            decimal max = 100;

            // Check to see if there is a value in the session, and if there is, assign it
            // to the variable that we setup to hold the value
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

            // Use BeveragesToFilter to filter the database based on the price range, name, and pack.
            // Once it has found the beverages that meet the criteria, store them in the 
            // filtered variable.
            IEnumerable<Beverage> filtered = BeveragesToFilter.Where(beverage => beverage.price >= min
                && beverage.price <= max && beverage.name.Contains(filterName)
                && beverage.pack.Contains(filterPack));

            // Convert filtered to a list and store it in finalFiltered.
            IEnumerable<Beverage> finalFiltered = filtered.ToList();

            return View(finalFiltered);
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
                // if the same id isn't found, add the 
                // beverage to the database
                if (!SearchDB(beverage))
                {
                    db.Beverages.Add(beverage);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                // if there is a duplicate, send the user to
                // an error page.
                else
                    return View("Error");
                
            }

            return View(beverage);
        }


        // Accepts the beverage to be added, and searches
        // the database to see if the same id exists
        private bool SearchDB(Beverage SearchBev)
        {
            // If the same id is not found, return false
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

        // Mark the method as Post since it is reached from a form submit
        // Make sure to validate the antiforgeryToken too since we include it in the form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter()
        {
            // Get the form data that we sent out of the request object.
            string name = Request.Form.Get("name");
            string pack = Request.Form.Get("pack");
            string min = Request.Form.Get("min");
            string max = Request.Form.Get("max");

            // Now that we have the data pulled out from the request object,
            // let's put it into the session so that other methods can have access to it
            Session["name"] = name;
            Session["pack"] = pack;
            Session["min"] = min;
            Session["max"] = max;

            // Redirect back to the index page
            return RedirectToAction("Index");
        }
    }
}
