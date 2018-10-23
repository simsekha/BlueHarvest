using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Capgemini.Hayri.Model
{
    public class Account
    {
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
