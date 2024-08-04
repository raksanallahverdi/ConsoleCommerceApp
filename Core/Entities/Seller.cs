﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Seller : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public decimal MyBankAccount { get; set; } = 10000;
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Pin { get; set; }
        public string SerialNumber { get; set; }
    }
}
