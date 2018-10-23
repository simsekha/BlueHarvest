using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Capgemini.Hayri.Model.Request
{
    public class AddTransactionRequest
    {
        public string CustomerID { get; set; }
        public Transaction Transaction { get; set; }
    }
}
