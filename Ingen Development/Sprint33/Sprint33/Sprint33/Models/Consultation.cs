using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sprint33.Models
{
    public class Consultation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConsultationID { get; set; }
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }
        [Required]
        [Display(Name = "Symptoms")]
        public string Symptoms { get; set; }
        [Required]
        [Display(Name = "Diagnosis")]
        public string Diagnosis { get; set; }
        [Required]
        [Display(Name = "Self-Care Notes")]
        public string Notes { get; set; }
        public int AppointmentID { get; set; }
        public virtual Appointment appointment { get; set; }
    }
}