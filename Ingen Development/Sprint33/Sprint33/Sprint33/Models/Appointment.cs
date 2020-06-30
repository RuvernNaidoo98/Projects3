using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint33.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }
        public Patient Patient { get; set; }
        public int PatientID { get; set; }
        public Doctor Doctor { get; set; }
        public int DoctorID { get; set; }
        [Display(Name ="Patient`s Name")]
        public string PatientName { get; set; }
        [Display(Name ="Doctor`s Name")]
        public string DoctorName { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name ="Day and Time of Appointment")]
        public DateTime AppointmentTime { get; set; }
        public bool Confirmed { get; set; }
        [Display(Name ="Checked In")]
        public bool CheckedIn { get; set; }
        [Display(Name ="Patient`s Symtoms")]
        public string symtoms { get; set; }
        [Display(Name ="Doctor`s Diagnosis")]
        public string diagnosis { get; set; }
        [Display(Name ="Exspration Date")]
        public DateTime Expire { get; set; }
    }
}