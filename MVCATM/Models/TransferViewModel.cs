using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVCATM.Models
{
    public class TransferViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal Amount { get; set; }

        public virtual CheckingAccount FromCheckingAccount { get; set; }
        [Required]
        public int FromCheckingAccountId { get; set; }

        public virtual CheckingAccount ToCheckingAccount { get; set; }
      
        public int ToCheckingAccountId { get; set; }
        [Required]
        public string ToAccountNumber { get; set; }

        public virtual TransactionStatus TransactionStatus { get; set; }

        public int TransactionStatusId { get; set; }
    }
}