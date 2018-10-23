using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Capgemini.Hayri.Model.Response
{
    public class GetTransactionsResponse
    {
        public string CustomerID { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string ReturnDescription { get; set; }
    }
}
