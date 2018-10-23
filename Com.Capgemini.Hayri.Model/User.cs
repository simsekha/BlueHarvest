using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Capgemini.Hayri.Model
{
    public class User
    {
        public string CustomerID { get; set; }
        public string Name { get;set;}
        public string Surname { get; set; } 
        public Account Account { get; set; }
    }
}
