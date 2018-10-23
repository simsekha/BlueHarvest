using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Capgemini.Hayri.Model.Request
{
    public class OpenAccountRequest
    {
        public string CustomerID { get; set; }
        public decimal InitialCredit { get; set; }
    }
}
