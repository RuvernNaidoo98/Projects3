using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Sprint33.Models;

namespace Sprint33.Controllers
{
    public class PrescriptionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Prescriptions
        public ActionResult Index(string search)
        {
            return View(db.Prescriptions.Where(x => x.PatientName.StartsWith(search) || search == null).ToList());
        }

        // GET: Prescriptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            return View(prescription);
        }

        // GET: Prescriptions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Prescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrescriptionId,PrescriptionDetails,DateIssued,PrescriptionValid,PatientName")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                if (prescription.PrescriptionValid <= DateTime.Now)
                {
                    return View(prescription);
                }
                else
                {
                    db.Prescriptions.Add(prescription);
                    prescription.DateIssued = DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

            return View(prescription);
        }

        // GET: Prescriptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            return View(prescription);
        }

        // POST: Prescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PrescriptionId,PrescriptionDetails,DateIssued,PrescriptionValid,PatientName")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prescription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prescription);
        }

        // GET: Prescriptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            return View(prescription);
        }

        // POST: Prescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prescription prescription = db.Prescriptions.Find(id);
            db.Prescriptions.Remove(prescription);
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

        [HttpGet]
        public ActionResult AddPrescription(int patientId)
        {
            if (patientId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //if (prescription == null)
            //{
            //    return HttpNotFound();
            //}
            var model = (from a in db.Appointments
                         join p in db.Patients on a.PatientID equals p.UserID
                         //join ps in db.Prescriptions on p.UserID equals ps.PatientID
                         where a.PatientID == patientId
                         select new PatientPrescriptionViewModel
                         {
                             PatientId = p.UserID,
                             AppointmentId = a.AppointmentID,
                             PatientName = a.PatientName,
                             Age = p.Age,
                             DateIssued = DateTime.Now,
                             DoctorName = a.DoctorName
                         }).FirstOrDefault();

            var debugPatientName = model.PatientName;
            var debugDoctorName = model.DoctorName;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPrescription(PatientPrescriptionViewModel prescriptionViewModel)
        {
            if (ModelState.IsValid)
            {

                    db.Prescriptions.Add(new Prescription
                    {
                        DoctorName = prescriptionViewModel.DoctorName,
                        PatientName = prescriptionViewModel.PatientName,
                        DateIssued = prescriptionViewModel.DateIssued,
                        PrescriptionDetails = prescriptionViewModel.PrescriptionDetails,
                        PrescriptionValid = prescriptionViewModel.PrescriptionValid
                    });
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }
            return View(prescriptionViewModel);
        }

        public ActionResult PrescriptionHistory()
        {
            var prescriptions = db.Prescriptions.Include(p => p.Patient);
            return View(prescriptions.ToList());
        }

        public ActionResult ViewPrescription(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            return View(prescription);
        }
    }
}
