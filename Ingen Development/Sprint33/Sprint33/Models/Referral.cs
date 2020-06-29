using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Sprint33.Models
{
    public class Referral
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Referral ID")]
        public int refferal_ID { get; set; }


        [Display(Name = "Doctor Full Name")]
        public string referral_Doctors_Name { get; set; }


        [Display(Name = "Doctor Practice Address")]
        public string referral_doctor_Add { get; set; }


        [Display(Name = "Doctor Contact Number")]
        public string referral_doctor_num { get; set; }


        [Display(Name = "Doctor Email Address")]
        public string referral_doctor_Email { get; set; }


        [Required(ErrorMessage = "Patient Name is mandatory")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please use letters only")]
        [Display(Name = "Patient Name")]
        public string referral_patient_Name { get; set; }


        [Required(ErrorMessage = "Patient Surname is mandatory")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please use letters only")]
        [Display(Name = "Patient Surname")]
        public string referral_patient_Surname { get; set; }


        [Display(Name = "Patient Date of birth")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime referral_patient_DOB { get; set; }


        [Display(Name = "Patient Gender")]
        public string referral_patient_Gender { get; set; }



        [Display(Name = "Validate Until")]
        [DataType(DataType.Date)]
        public DateTime referral_ValidDate { get; set; }


        [Display(Name = "Hospital")]
        public string refferal_Location { get; set; }


        [Display(Name = "Reasoning")]
        public string referral_Reasoning { get; set; }


        public string dName()
        {
            string dFullName = "Dr. James Govender ";
            return (dFullName);
        }

        public string dAdd()
        {
            string dAddress = "37 Magaliesberg Street, Shallcross, Durban, 4079 ";
            return (dAddress);
        }

        public string dCon()
        {
            string dContactNum = "0314099088";
            return (dContactNum);
        }

        public string dEmail()
        {
            string dEmail = "drjgovender@medis.co.za";
            return (dEmail);
        }

    }
}