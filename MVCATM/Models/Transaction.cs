using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCATM.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Range(1.0, Double.MaxValue)]
        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal Amount { get; set; }

        public virtual CheckingAccount checkingAccount { get; set; }
        [Required]
        public int CheckingAccountId { get; set; }

        

     
    }
}