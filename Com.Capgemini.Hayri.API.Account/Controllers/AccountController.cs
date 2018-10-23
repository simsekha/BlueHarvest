using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Com.Capgemini.Hayri.Integrate;
using Com.Capgemini.Hayri.Model;
using Com.Capgemini.Hayri.Model.Request;
using Com.Capgemini.Hayri.Model.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Com.Capgemini.Hayri.API.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IMemoryCache _cache;
        //Since I am using In-Memory Cache in this example, I should pass cache object in to the constructor
        //Normally, I should use Redis or memcached for in memory DB requirements.
        public AccountController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            //We are adding some predefined customer here, if we do not have already
            AddPredefinedUsers();
        }

        private void AddPredefinedUsers()
        {
            var usersInTheSystem = _cache.Get(Constants.USERS_CACHE_KEY);
            if (null == usersInTheSystem)
            {
                var usersInTheSystemList = new List<User>
                {
                    new User { CustomerID = "1", Name = "Hayri", Surname = "Şimşek" },
                    new User { CustomerID = "2", Name = "A", Surname = "AA" },
                    new User { CustomerID = "3", Name = "B", Surname = "BB" },
                    new User { CustomerID = "4", Name = "C", Surname = "CC" }
                };

                _cache.Set(Constants.USERS_CACHE_KEY, JsonConvert.SerializeObject(usersInTheSystemList));
            }
        }
        [HttpGet]
        public ActionResult<string> Get()
        {
            return BadRequest(Constants.SYSTEM_DOES_NOT_HAVE_GET_ALL_ACCOUNTS_API);
        }
        [HttpGet("{customerID}")]
        public ActionResult<User> Get(string customerID)
        {
            List<User> usersInTheSystem = JsonConvert.DeserializeObject<List<User>>(_cache.Get(Constants.USERS_CACHE_KEY).ToString());
            var userIndex = usersInTheSystem.FindIndex(u => u.CustomerID.Equals(customerID));
            if (userIndex < 0)
            {
                return Ok(new GetAccountResponse
                {
                    CustomerID = customerID,
                    User = null,
                    ReturnDescription = Constants.NO_SUCH_USER_IN_THE_SYSTEM
                });
            }

            //I need to get transaction details from transaction API.
            var transactions = new Integrator().GetTransactionsFromRemoteAPI(new GetTransactionsRequest
            {
                CustomerID = customerID
            });
            if(null!=usersInTheSystem[userIndex].Account)
                usersInTheSystem[userIndex].Account.Transactions = transactions.Transactions;
            return Ok(new GetAccountResponse
            {
                CustomerID = customerID,
                User = usersInTheSystem[userIndex],
                ReturnDescription = Constants.OK
            });
        }

        [HttpPost]
        public ActionResult<OpenAccountResponse> Post([FromBody] OpenAccountRequest openAccountRequest)
        {
            if (openAccountRequest == null || string.IsNullOrEmpty(openAccountRequest.CustomerID))
            {
                return BadRequest(new OpenAccountResponse
                {
                    Request = openAccountRequest,
                    ReturnDescription = Constants.NOT_A_VALID_INPUT
                });
            }

            List<User> usersInTheSystem = JsonConvert.DeserializeObject<List<User>>(_cache.Get(Constants.USERS_CACHE_KEY).ToString());
            var userIndex = usersInTheSystem.FindIndex(u => u.CustomerID.Equals(openAccountRequest.CustomerID));

            if (userIndex < 0)
            {
                return Ok(new OpenAccountResponse
                {
                    Request = openAccountRequest,
                    ReturnDescription = Constants.NO_SUCH_USER_IN_THE_SYSTEM
                });
            }

            var user = usersInTheSystem[userIndex];
            if (null == user.Account)
            {
                user.Account = new Model.Account();
            }

            if (openAccountRequest.InitialCredit != 0) //Since Initial credit > 0, we must trigger transaction service and add a new transaction
            {
                var transaction = new Transaction
                {
                    TransactionDate = DateTime.Now,
                    TransactionDetail = String.Format(Constants.TRANSACTION_ADDED, openAccountRequest.InitialCredit)
                };

                user.Account.Transactions.Add(transaction);
                user.Account.Balance += openAccountRequest.InitialCredit;
                try
                {
                    new Integrator().AddTransactionQuee(new AddTransactionRequest
                    {
                        CustomerID = openAccountRequest.CustomerID,
                        Transaction = transaction
                    });
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, (new OpenAccountResponse
                    {
                        Request = openAccountRequest,
                        ReturnDescription = Constants.INTERNAL_SERVER_ERROR
                    }));
                }
            }

            //Flow completed, So I need to update In-Memory DB (Cache) as below.

            usersInTheSystem[userIndex] = user;
            _cache.Set(Constants.USERS_CACHE_KEY, JsonConvert.SerializeObject(usersInTheSystem));

            return Ok(new OpenAccountResponse
            {
                Request = openAccountRequest,
                ReturnDescription = Constants.OK
            });
        }

    }
}
