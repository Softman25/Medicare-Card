using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ThisisaTest.Database
{
    public class Invoice
    {
        [Key]
        public ulong UserId { get; set; }
        public int Amount { get; set; }
        public int Profile { get; set; }
    }

}
