using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Milkent.Models
{
    public class MdlPurchase
    {
        [Key]
        public int ID { get; set; }
        [Key]
        public int SupplierID { get; set; }
        [Key]
        public int UserID { get; set; }
        [Key]
        public int Bill_ID { get; set; }
        [Required]
        public double Milk { get; set; }

        [Required]
        public double Fat { get; set; }

        [Required]
        public string PartOfDay { get; set; }

        [Required]
        public double LR { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public double Credit { get; set; }

        [Required]
        public double PurchasePrice { get; set; }

        //unrequired Field
        public double TS { get; set; }
        public double Total { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FromDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ToDate { get; set; }
        public List<MdlSupplier> SupplierList { get; set; }
    }
}