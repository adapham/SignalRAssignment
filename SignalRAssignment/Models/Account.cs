using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignalRAssignment.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string UserName { get; set; } 
        public string Password { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }
        public short Type { get; set; } = 0;
    }
}
