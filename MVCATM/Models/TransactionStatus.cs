using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCATM.Models
{
    public enum ProcessStatus
    {
        Success,
        Error
    };
    public class TransactionStatus
    {
        public int ID { get; set; }
        public ProcessStatus processStatus { get; set; } 
        public String StatusMessage{ get; set; }
        [Required]
        public DateTime TransactionTime { get; set; }
        [Required]
        public virtual Transaction transaction { get; set; }
        [Required]
        public int TransactionId { get; set; }
    }
}