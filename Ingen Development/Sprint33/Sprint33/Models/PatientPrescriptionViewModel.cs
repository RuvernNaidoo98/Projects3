using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sprint33.Models
{
    public class PatientPrescriptionViewModel
    {
        public string PatientName { get; set; }
        public int Age { get; set; }
        public string DoctorName { get; set; }
        [Required]
        [Display(Name = "Prescribed Medication:")]
        [DataType(DataType.MultilineText)]
        public string PrescriptionDetails { get; set; }
        public DateTime DateIssued { get; set; }
        [Required]
        [Display(Name = "Prescription Valid Until")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        [CurrentDate(ErrorMessage = "Date must be after or equal to current date")]
        public DateTime PrescriptionValid { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int AppointmentId { get; set; }

    }

    public class CurrentDateAttribute : ValidationAttribute
    {
        public CurrentDateAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            var dt = (DateTime)value;
            if (dt >= DateTime.Now)
            {
                return true;
            }
            return false;
        }
    }
}