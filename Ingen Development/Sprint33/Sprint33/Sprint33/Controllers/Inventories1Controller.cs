using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sprint33.Models;
using Newtonsoft.Json;

namespace Sprint33.Controllers
{
    public class Inventories1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Inventories1
        public ActionResult Index()
        {
            return View(db.Inventorys.ToList());
        }

        // GET: Inventories1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventorys.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // GET: Inventories1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inventories1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemID,itemName,itemQuantity,itemPrice,Item_Image,Item_ImagePath")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Inventorys.Add(inventory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inventory);
        }

        public ActionResult LowStock()
        {
            var items = db.Inventorys.ToList();


            List<DataPoint> dataPoints = new List<DataPoint>();
            List<DataPoint2> dataPoints2 = new List<DataPoint2>();
            var selectedItems = (from a in db.Inventorys
                                 orderby a.itemQuantity ascending
                                 select new
                                 { name = a.itemName, uis = a.itemQuantity }).Take(10);
            var selectedItems1 = (from a in db.Inventorys
                                  orderby a.itemQuantity descending
                                  select new
                                  { name = a.itemName, uis = a.itemQuantity }).Take(10);

            foreach (var s in selectedItems)
            {
                dataPoints.Add(new DataPoint(Convert.ToString(s.name), Convert.ToDouble(s.uis)));
                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            }

            foreach (var s in selectedItems1)
            {
                dataPoints2.Add(new DataPoint2(Convert.ToString(s.name), Convert.ToDouble(s.uis)));
                ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);
            }

            return View(items.ToList());
        }
        public ActionResult HighStock()
        {
            var items = db.Inventorys.ToList();


            List<DataPoint> dataPoints = new List<DataPoint>();
            List<DataPoint2> dataPoints2 = new List<DataPoint2>();
            var selectedItems = (from a in db.Inventorys
                                 orderby a.itemQuantity descending
                                 select new
                                 { name = a.itemName, uis = a.itemQuantity }).Take(10);
            var selectedItems1 = (from a in db.Inventorys
                                  orderby a.itemQuantity ascending
                                  select new
                                  { name = a.itemName, uis = a.itemQuantity }).Take(10);

            foreach (var s in selectedItems)
            {
                dataPoints.Add(new DataPoint(Convert.ToString(s.name), Convert.ToDouble(s.uis)));
                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            }

            foreach (var s in selectedItems1)
            {
                dataPoints2.Add(new DataPoint2(Convert.ToString(s.name), Convert.ToDouble(s.uis)));
                ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);
            }

            return View(items.ToList());
        }
        // GET: Inventories1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventorys.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventories1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemID,itemName,itemQuantity,itemPrice,Item_Image,Item_ImagePath")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inventory);
        }

        // GET: Inventories1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventorys.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventories1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inventory inventory = db.Inventorys.Find(id);
            db.Inventorys.Remove(inventory);
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
        //Code
        //Admin Side
        public ActionResult AdminIndex(string searching)
        {
            var item = from s in db.Inventorys
                       select s;

            if (!String.IsNullOrEmpty(searching))
            {
                item = item.Where(s => s.itemName.Contains(searching));
            }
            return View(item.ToList());
        }
        public ActionResult AdminEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventorys.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminEdit(Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }
            return View(inventory);
        }
        public ActionResult AdminDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventorys.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }
        public ActionResult AdminCreate()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminCreate([Bind(Include = "ItemID,itemName,itemQuantity,itemPrice,Item_Image,Item_ImagePath")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Inventorys.Add(inventory);
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }

            return View(inventory);
        }
        //End of Admin side//

        
    }
}