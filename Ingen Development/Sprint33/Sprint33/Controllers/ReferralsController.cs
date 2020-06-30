using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sprint33.Models;

namespace Sprint33.Controllers
{
    public class ReferralsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Referrals
        public ActionResult Index(string search)
        {
            // return View(db.referrals.ToList());
            return View(db.referrals.Where(x => x.referral_patient_Name.Contains(search) || search == null).ToList());
        }

        // GET: Referrals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referral referral = db.referrals.Find(id);
            if (referral == null)
            {
                return HttpNotFound();
            }
            return View(referral);
        }

        // GET: Referrals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Referrals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "refferal_ID,referral_Doctors_Name,referral_doctor_Add,referral_doctor_num,referral_doctor_Email,referral_patient_Name,referral_patient_Surname,referral_patient_DOB,referral_patient_Gender,referral_ValidDate,refferal_Location,referral_Reasoning")] Referral referral)
        {
            if (ModelState.IsValid)
            {
                referral.referral_Doctors_Name = referral.dName();
                referral.referral_doctor_Add = referral.dAdd();
                referral.referral_doctor_Email = referral.dEmail();
                referral.referral_doctor_num = referral.dCon();
                db.referrals.Add(referral);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(referral);
        }

        // GET: Referrals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referral referral = db.referrals.Find(id);
            if (referral == null)
            {
                return HttpNotFound();
            }
            return View(referral);
        }

        // POST: Referrals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "refferal_ID,referral_Doctors_Name,referral_doctor_Add,referral_doctor_num,referral_doctor_Email,referral_patient_Name,referral_patient_Surname,referral_patient_DOB,referral_patient_Gender,referral_ValidDate,refferal_Location,referral_Reasoning")] Referral referral)
        {
            if (ModelState.IsValid)
            {
                db.Entry(referral).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(referral);
        }

        // GET: Referrals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referral referral = db.referrals.Find(id);
            if (referral == null)
            {
                return HttpNotFound();
            }
            return View(referral);
        }

        // POST: Referrals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Referral referral = db.referrals.Find(id);
            db.referrals.Remove(referral);
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
    }
}
