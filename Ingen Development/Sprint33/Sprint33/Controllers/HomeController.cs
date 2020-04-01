using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sprint33.Models;
using System.Net;
using System.Data.Entity;

namespace Sprint33.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //Project Code From Here
        public ActionResult Login()
        {
            Session["UserName"] = "";
            Session["id"] = "";
            ViewBag.Message = "";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "UserID,FirstName,Surname,ContactNumber,Email,Password")] Doctor doctors, [Bind(Include = "UserID,FirstName,Surname,Age,ContactNumber,Email,Password")] Patient patients, [Bind(Include = "UserID,FirstName,Surname,Email,Password")] Admin admins)
        {


            ViewBag.Message = "Incorrect Username or Passowrd";
            foreach (var item in db.Doctors.ToList())
            {
                if (doctors.Email.Equals(item.Email) && doctors.Password.Equals(item.Password))
                {
                    Session["id"] = item.UserID;
                    Session["UserName"] = item.FirstName + " " + item.Surname;
                    return RedirectToAction("Homepage", "Doctors1");
                }
            }
            foreach (var item in db.Patients)
            {
                if (patients.Email.Equals(item.Email) && patients.Password.Equals(item.Password))
                {
                    Session["id"] = item.UserID;
                    Session["UserName"] = item.FirstName + " " + item.Surname;
                    return RedirectToAction("Homepage", "Patients1");
                }
            }
            foreach (var item in db.Admins)
            {
                if (admins.Email.Equals(item.Email) && admins.Password.Equals(item.Password))
                {
                    Session["id"] = item.UserID;
                    Session["UserName"] = item.FirstName + " " + item.Surname;
                    return RedirectToAction("Homepage", "Admins1");
                }
            }

            return View(doctors);

        }
        public ActionResult ForgotPassword()
        {
            ViewBag.ID = "";
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword([Bind(Include = "FirstName , Surname , ContactNumber , Email")]Patient patient)
        {
            foreach (var item in db.Patients)
            {
                if (patient.FirstName == item.FirstName && patient.Surname == item.Surname && patient.ContactNumber == item.ContactNumber && patient.Email == item.Email)
                {

                    return RedirectToAction("ResetPassword", new { id = item.UserID });
                }
            }
            ViewBag.Message = "Recovery Details Incorrect Please try again";
            return View(patient);
        }
        public ActionResult ResetPassword(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword([Bind(Include = "UserID,FirstName,Surname,Age,ContactNumber,Email,Password")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(patient);
        }
      
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Patient patient)
        {
           
            if (ModelState.IsValid)
            {
                patient.isActive = true;
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(patient);
        }
    }
}