using Com.Capgemini.Hayri.Model.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Capgemini.Hayri.Model.Response
{
    public class OpenAccountResponse
    {
        public OpenAccountRequest Request { get; set; }
        public string ReturnDescription { get; set; }
    }
}
