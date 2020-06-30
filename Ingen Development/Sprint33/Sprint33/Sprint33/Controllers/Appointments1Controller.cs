using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sprint33.Models;
using System.Net.Mail;

namespace Sprint33.Controllers
{
    public class Appointments1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Appointments1
        public ActionResult Index()
        {
            return View(db.Appointments.ToList());
        }

        // GET: Appointments1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Appointments1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentID,PatientID,DoctorID,PatientName,DoctorName,AppointmentTime,Confirmed,CheckedIn,symtoms,diagnosis,Expire")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appointment);
        }

        // GET: Appointments1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppointmentID,PatientID,DoctorID,PatientName,DoctorName,AppointmentTime,Confirmed,CheckedIn,symtoms,diagnosis,Expire")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        // GET: Appointments1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
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

        //Patient Side
        public ActionResult PatientIndex()
        {
            return View(db.Appointments.ToList());
        }

        public ActionResult BookSlot(int? id)
        {
            ViewBag.Message = "";
            
            foreach (var item in db.Appointments)
            {

                if (item.PatientName == Session["UserName"].ToString() )
                {
                    if (item.CheckedIn == false)
                    {
                        ViewBag.Message = "You already have a Booking";
                        RedirectToAction("PatientIndex");
                    }
                    
                }

           }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }

            return View(appointments);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookSlot([Bind(Include = "AppointmentID,PatientID,DoctorID,PatientName,DoctorName,AppointmentTime,Confirmed,CheckedIn,symtoms,diagnosis,Expire")]Appointment appointments)
        {
            int Patient_ID = Convert.ToInt32(Session["id"]);
            if (ModelState.IsValid)
            {
                appointments.CheckedIn = false;
                appointments.PatientID = Patient_ID;
                appointments.PatientName = Session["UserName"].ToString();
                db.Entry(appointments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PatientIndex");
            }
            return View(appointments);
        }

        public ActionResult ViewAppointment()
        {
            return View(db.Appointments.ToList());
        }
        public ActionResult PatientCancel(int id)
        {
            Appointment x = db.Appointments.Find(id);
            x.symtoms = null;
            x.PatientName = null;
            x.Confirmed = false;
            db.Entry(x).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("PatientIndex");

        }
        public ActionResult PatientHistory()
        {
            return View(db.Appointments.ToList());
        }
        //End of Patient Side//

        //Doctor Side
        public ActionResult DoctorIndex()
        {
            return View(db.Appointments.ToList());
        }

        public ActionResult DoctorCancel(int id)
        {
            Appointment x = db.Appointments.Find(id);
            x.symtoms = null;
            x.PatientName = null;
            x.Confirmed = false;
            db.Entry(x).State = EntityState.Modified;
            db.SaveChanges();
            int pid = 0;
            string email = "";
            try
            {

                foreach (var item in db.Patients)
                {
                    if (item.UserID == x.PatientID)
                    {
                        pid = item.UserID;
                        email = item.Email;
                    }
                }

                MailMessage mail = new MailMessage();

                string MailTo = email;  //lecturerEmail;
                mail.From = new MailAddress("doctornaidoosoffice@gmail.com");
                mail.To.Add(MailTo);
                mail.Subject = "Dr Naidoo Office";  // Mail Subject  
                mail.Body = "Good day " + x.PatientName + " we regret to inform you that your appointment for : " + x.AppointmentTime + " has been cancelled we apologize for the inconvience, please reschedule a new appointment.";




                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "doctornaidoosoffice@gmail.com",
                        Password = "Doctor05@"
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(mail);

                }

            }

            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString());
            }
            return RedirectToAction("DoctorIndex");

        }
        public ActionResult DoctorEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }
            return View(appointments);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoctorEdit([Bind(Include = "AppointmentID,PatientID,DoctorID,PatientName,DoctorName,AppointmentTime,Confirmed,CheckedIn,symtoms,diagnosis,Expire")]Appointment appointments)
        {
            if (ModelState.IsValid)
            {

                db.Entry(appointments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DoctorIndex");
            }
            return View(appointments);
        }
        public ActionResult DoctorDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }
        public ActionResult CreateSlot()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSlot(Appointment appointments)
        {
            appointments.Expire = appointments.AppointmentTime;
            appointments.DoctorID = 1;
            appointments.PatientID = 1;
            appointments.CheckedIn = false;
            appointments.Confirmed = false;
            appointments.DoctorName = Session["Username"].ToString();
            db.Appointments.Add(appointments);
            db.SaveChanges();
            return RedirectToAction("DoctorIndex");
        }
        public ActionResult CheckedInList()
        {
            return View(db.Appointments.ToList());
        }
        public ActionResult Perscriptions(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }
            return View(appointments);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Perscriptions([Bind(Include = "AppointmentID,PatientID,DoctorID,PatientName,DoctorName,AppointmentTime,Confirmed,CheckedIn,symtoms,diagnosis,Expire")]Appointment appointments)
        {
            if (ModelState.IsValid)
            {

                appointments.CheckedIn = true;
                db.Entry(appointments).State = EntityState.Modified;
                db.SaveChanges();
                int pid = 0;
                string email = "";
                Appointment x = db.Appointments.Find(appointments.AppointmentID);
                try
                {

                    foreach (var item in db.Patients)
                    {
                        if (item.UserID == x.PatientID)
                        {
                            pid = item.UserID;
                            email = item.Email;
                        }
                    }

                    MailMessage mail = new MailMessage();

                    string MailTo = email;  //lecturerEmail;
                    mail.From = new MailAddress("doctornaidoosoffice@gmail.com");
                    mail.To.Add(MailTo);
                    mail.Subject = "Dr Naidoo Office";  // Mail Subject  
                    mail.Body = "Good day " + x.PatientName+" The Perscription for your check up : "+x.AppointmentTime+" is "+x.diagnosis+"." ;




                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "doctornaidoosoffice@gmail.com",
                            Password = "Doctor05@"
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Send(mail);

                    }

                }

                catch (Exception ex)
                {

                    //MessageBox.Show(ex.ToString());
                }
                return RedirectToAction("CheckedInList");
            }
            return View(appointments);
        }
        public ActionResult DoctorPatientHistory()
        {
            return View(db.Appointments.ToList());
        }
        //End of Doctor side//

        //Admin side
        public ActionResult AdminIndex()
        {
            return View(db.Appointments.ToList());
        }
        public ActionResult AdminCancel(int id)
        {
            Appointment x = db.Appointments.Find(id);
            x.symtoms = null;
            x.PatientName = null;
            x.Confirmed = false;
            db.Entry(x).State = EntityState.Modified;
            db.SaveChanges();
            int pid = 0;
            string email = "";
            try
            {

                foreach (var item in db.Patients)
                {
                    if (item.UserID == x.PatientID)
                    {
                        pid = item.UserID;
                        email = item.Email;
                    }
                }

                MailMessage mail = new MailMessage();

                string MailTo = email;  //lecturerEmail;
                mail.From = new MailAddress("doctornaidoosoffice@gmail.com");
                mail.To.Add(MailTo);
                mail.Subject = "Dr Naidoo Office";  // Mail Subject  
                mail.Body = "Good day " + x.PatientName + " we regret to inform you that your appointment for : " + x.AppointmentTime + "  has been cancelled we apologize for the inconvience, please reschedule a new appointment.";




                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "doctornaidoosoffice@gmail.com",
                        Password = "Doctor05@"
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(mail);

                }

            }

            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString());
            }
            return RedirectToAction("AdminIndex");

        }
        public ActionResult AdminEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }
            return View(appointments);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminEdit([Bind(Include = "AppointmentID,PatientID,DoctorID,PatientName,DoctorName,AppointmentTime,Confirmed,CheckedIn,symtoms,diagnosis,Expire")]Appointment appointments)
        {
            if (ModelState.IsValid)
            {

                db.Entry(appointments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }
            return View(appointments);
        }
        public ActionResult AdminDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }
        public ActionResult ConfirmBooking()
        {

            return View(db.Appointments.ToList());
        }
        public ActionResult Confirm(int id)
        {
            Appointment x = db.Appointments.Find(id);
            x.Confirmed = true;
            db.Entry(x).State = EntityState.Modified;
            db.SaveChanges();
            int pid = 0;
            string email = "";

            try
            {

                foreach (var item in db.Patients)
                {
                    if (item.UserID == x.PatientID)
                    {
                        pid = item.UserID;
                        email = item.Email;
                    }
                }

                MailMessage mail = new MailMessage();

                string MailTo = email;  //lecturerEmail;
                mail.From = new MailAddress("doctornaidoosoffice@gmail.com");
                mail.To.Add(MailTo);
                mail.Subject = "Dr Naidoo Office";  // Mail Subject  
                mail.Body = "Good day " + x.PatientName + " we would like to inform you that your appointment for : " + x.AppointmentTime + " has been confirmed.";




                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "doctornaidoosoffice@gmail.com",
                        Password = "Doctor05@"
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(mail);

                }

            }

            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString());
            }

            return RedirectToAction("ConfirmBooking");
        }
        
    }
    //End of Admin Side//

    


}
//Email Tests
//mail.From = new MailAddress("Lureshen100@gmail.com");
//mail.To.Add(email);
//mail.Subject = "Test Mail";
//mail.Body = "This is for testing SMTP mail from GMAIL";

//SmtpServer.Port = 465;
//SmtpServer.Credentials = new System.Net.NetworkCredential("Lureshen100@gmail.com", "digimonworld");
//SmtpServer.EnableSsl = true;

//SmtpServer.Send(mail);
//MailMessage mail = new MailMessage("Lureshen100@gmail.com", email);
//SmtpClient client = new SmtpClient();
//client.Port = 25;
//client.DeliveryMethod = SmtpDeliveryMethod.Network;
//client.UseDefaultCredentials = false;
//client.Host = "smtp.gmail.com";
//mail.Subject = "this is a test email.";
//mail.Body = "this is my test email body";
//client.Send(mail);
////MessageBox.Show("mail Send");
//WebMail.SmtpServer = "smtp.gmail.com";
//WebMail.SmtpPort = 587;
//WebMail.SmtpUseDefaultCredentials = true;
//WebMail.EnableSsl = true;
//WebMail.UserName = "Lureshen100@gmail.com";
//WebMail.Password = "digimonworld";
//WebMail.From = "Lureshen100@gmail.com";
//WebMail.Send(to: email, subject: "Dearest applicant : " + "suckabala", body: "Your vote through the E-Lect online voting application has been successful. Please go ahead and share our apllication. Help a brother out. We appreciate your use of E-Lect. Voting is made easy through E-Lect.Viva E-Lect Viva! \n" + "Yours Gratefully \n" + "The E-Lect Team", isBodyHtml: true);

// Sending MailTo  
//List<string> li = new List<string>();
//mail.CC.Add(string.Join<string>(",", li)); // Sending CC  
//mail.Bcc.Add(string.Join<string>(",", li)); // Sending Bcc
