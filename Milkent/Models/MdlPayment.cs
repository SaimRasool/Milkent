using System;
using System.ComponentModel.DataAnnotations;

namespace Milkent.Models
{
    public class MdlPayment
    {
        [Key]
        public int ID { get; set; }
        [Key]
        public int Bill_ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public double Credit_Debit { get; set; }

        public double Total { get; set; }

    }
}