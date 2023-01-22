using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Milkent.Models
{
    public class MdlSales
    {
        [Key]
        public int ID { get; set; }
        [Key]
        public int CustomerID { get; set; }
        [Key]
        public int UserID { get; set; }
        [Key]
        public int Bill_ID { get; set; }
        [Required]
        public double MilkCredit { get; set; }

        [Required]
        public double Fat { get; set; }

        [Required]
        public string PartOfDay { get; set; }

        [Required]
        public double LR { get; set; }


        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public double CashDebit { get; set; }


        [Required]
        public double SalePrice { get; set; }

        //unrequired Field
        public double TS { get; set; }
        public double Total { get; set; }
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }
        public List<MdlCustomer> CustomerList { get; set; }
    }
}