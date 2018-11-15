using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCATM.Models
{
    public enum TransactionProcessStatus
    {
        Success,
        Error
    };
    public class TransactionStatus
    {
        public int ID { get; set; }
        public TransactionProcessStatus processStatus { get; set; } 
        public String StatusMessage{ get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TransactionTime { get; set; }
        [Required]
        public virtual Transaction transaction { get; set; }
        [Required]
        public int TransactionId { get; set; }
       
    }
}