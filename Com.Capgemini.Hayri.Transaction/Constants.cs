using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Capgemini.Hayri.Model;
using Microsoft.AspNetCore.Mvc;

namespace Com.Capgemini.Hayri.API.Transaction
{
    public static class Constants
    {
        public static readonly string TRANSACTION_URL = "";
        public static readonly string NOT_A_VALID_INPUT = "Not a valid input";
        public static readonly string NO_SUCH_USER_IN_THE_SYSTEM = "No Such User In The System";
        public static readonly string TRANSACTION_ADDED = "Transaction Added";
        public static readonly string OK = "OK";
        public static readonly string TRANSACTIONS_CACHE_KEY = "Transactions";
        public static readonly string INTERNAL_SERVER_ERROR = "Internal Server Error";
        public static readonly string SYSTEM_DOES_NOT_HAVE_GET_ALL_TRANSACTIONS_API = "System does not have get all transactions api, so please provide a customer id";
    }
}
