using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint33.Models
{
    public class Inventory
    {
        [Key]
        public int ItemID { get; set; }
        [Required]
        [Display(Name = "Name Of Item")]
        public string itemName { get; set; }
        [Required]
        [Display(Name = "Stock On Hand")]
        public int itemQuantity { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Price Per Item")]
        public double itemPrice { get; set; }
        public byte[] Item_Image { get; set; }

        public string Item_ImagePath { get; set; }
    }
}