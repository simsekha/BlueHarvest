using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Capgemini.Hayri.Model;
using Microsoft.AspNetCore.Mvc;

namespace Com.Capgemini.Hayri.API.Account
{
    public static class Constants
    {
        public static readonly string NOT_A_VALID_INPUT = "Not a valid input";
        public static readonly string NO_SUCH_USER_IN_THE_SYSTEM = "No Such User In The System";
        public static readonly string TRANSACTION_ADDED = "Transaction Added for {0}";
        public static readonly string OK = "OK";
        public static readonly string USERS_CACHE_KEY = "Users";
        public static readonly string INTERNAL_SERVER_ERROR = "Internal Server Error";
        public static readonly string SYSTEM_DOES_NOT_HAVE_GET_ALL_ACCOUNTS_API = "System does not have get all accounts api, so please provide an customer id";
    }
}
