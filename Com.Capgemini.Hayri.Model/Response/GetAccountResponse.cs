using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Capgemini.Hayri.Model.Response
{
    public class GetAccountResponse
    {
        public string CustomerID { get; set; }
        public User User { get; set; }
        public string ReturnDescription { get; set; }
    }
}
