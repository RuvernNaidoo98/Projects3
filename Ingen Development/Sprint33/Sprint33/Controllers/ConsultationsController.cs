using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sprint33.Models;

namespace Sprint33.Controllers
{
    public class ConsultationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Consultations
        public ActionResult Index(string search)
        {
            //var consultations = db.Consultations.Include(c => c.appointment);
            //return View(await consultations.ToListAsync());

            return View(db.Consultations.Where(x => x.appointment.PatientName.Contains(search) || search == null).ToList());

        }
        public async Task<ActionResult> PatientHistory()
        {
            var consultations = db.Consultations.Include(c => c.appointment);
            return View(await consultations.ToListAsync());
        }

        // GET: Consultations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultation consultation = await db.Consultations.FindAsync(id);
            if (consultation == null)
            {
                return HttpNotFound();
            }
            return View(consultation);
        }

        // GET: Consultations/Create
        public ActionResult Create()
        {
            ViewBag.AppointmentID = new SelectList(db.Appointments, "AppointmentID", "PatientName");
            return View();
        }

        // POST: Consultations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ConsultationID,PatientName,AppointmentDate,Symptoms,Diagnosis,Notes,AppointmentID")] Consultation consultation)
        {
            if (ModelState.IsValid)
            {
                db.Consultations.Add(consultation);

                await GetInfo(consultation);

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AppointmentID = new SelectList(db.Appointments, "AppointmentID", "PatientName", consultation.AppointmentID);
            return View(consultation);
        }

        // GET: Consultations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultation consultation = await db.Consultations.FindAsync(id);
            if (consultation == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppointmentID = new SelectList(db.Appointments, "AppointmentID", "PatientName", consultation.AppointmentID);
            return View(consultation);
        }

        // POST: Consultations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ConsultationID,PatientName,AppointmentDate,Symptoms,Diagnosis,Notes,AppointmentID")] Consultation consultation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consultation).State = EntityState.Modified;

                await GetInfo(consultation);

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AppointmentID = new SelectList(db.Appointments, "AppointmentID", "PatientName", consultation.AppointmentID);
            return View(consultation);
        }

        // GET: Consultations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultation consultation = await db.Consultations.FindAsync(id);
            if (consultation == null)
            {
                return HttpNotFound();
            }
            return View(consultation);
        }

        // POST: Consultations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Consultation consultation = await db.Consultations.FindAsync(id);
            db.Consultations.Remove(consultation);
            await db.SaveChangesAsync();
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
        public async Task GetInfo(Consultation consultation)
        {
            Appointment appoint = (from p in db.Appointments
                                   where p.AppointmentID == consultation.AppointmentID
                                   select p).SingleOrDefault();

            consultation.AppointmentDate = appoint.AppointmentTime;
            consultation.PatientName = appoint.PatientName;

            await db.SaveChangesAsync();
        }
    }
}
