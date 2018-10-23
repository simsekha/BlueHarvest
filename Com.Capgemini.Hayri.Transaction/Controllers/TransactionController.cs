using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Capgemini.Hayri.API.Transaction;
using Com.Capgemini.Hayri.Model;
using Com.Capgemini.Hayri.Model.Request;
using Com.Capgemini.Hayri.Model.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Com.Capgemini.Hayri.Transaction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private IMemoryCache _cache;
        //Since I am using In-Memory Cache in this example, I should pass cache object in to the constructor
        //Normally, I should use Redis or memcached for in memory DB requirements.
        public TransactionController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            if(_cache.Get(Constants.TRANSACTIONS_CACHE_KEY)==null)
                _cache.Set(Constants.TRANSACTIONS_CACHE_KEY,JsonConvert.SerializeObject(new Dictionary<string,List<Model.Transaction>>()));
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return BadRequest(Constants.SYSTEM_DOES_NOT_HAVE_GET_ALL_TRANSACTIONS_API);
        }
        [HttpGet("{customerID}")]
        public ActionResult<User> Get(string customerID)
        {
            Dictionary<string,List<Model.Transaction>> transactionsInTheSystem = JsonConvert.DeserializeObject<Dictionary<string,List<Model.Transaction>>>(_cache.Get(Constants.TRANSACTIONS_CACHE_KEY).ToString());
            var doWeHaveAnyTransactionForThisUserBefore = transactionsInTheSystem.TryGetValue(customerID, out List<Model.Transaction> transactionsForThisUser);

            if (!doWeHaveAnyTransactionForThisUserBefore)
            {
                transactionsForThisUser = new List<Model.Transaction>();
            }
            return Ok(new GetTransactionsResponse
            {
                CustomerID = customerID,
                ReturnDescription = Constants.OK,
                Transactions = transactionsForThisUser
            });
        }

        [HttpPost]
        public ActionResult<OpenAccountResponse> Post([FromBody] AddTransactionRequest addTransactionRequest)
        {
            if (addTransactionRequest == null || string.IsNullOrEmpty(addTransactionRequest.CustomerID))
            {
                return BadRequest(new AddTransactionResponse
                {
                    Request = addTransactionRequest,
                    ReturnDescription = Constants.NOT_A_VALID_INPUT
                });
            }

            Dictionary<string, List<Model.Transaction>> transactionsInTheSystem = JsonConvert.DeserializeObject<Dictionary<string, List<Model.Transaction>>>(_cache.Get(Constants.TRANSACTIONS_CACHE_KEY).ToString());
            var doWeHaveAnyTransactionForThisUserBefore = transactionsInTheSystem.TryGetValue(addTransactionRequest.CustomerID, out List<Model.Transaction> transactionsForThisUser);

            if (!doWeHaveAnyTransactionForThisUserBefore)
            { 
                //We do not have any transaction belong to this user before
                transactionsForThisUser = new List<Model.Transaction>
                {
                    addTransactionRequest.Transaction
                };
                transactionsInTheSystem.Add(addTransactionRequest.CustomerID, transactionsForThisUser);
            }
            else
            {
                //We have already some transactions for this user
                //So add a new one
                transactionsForThisUser.Add(addTransactionRequest.Transaction);
                transactionsInTheSystem[addTransactionRequest.CustomerID] = transactionsForThisUser;
            }

            _cache.Set(Constants.TRANSACTIONS_CACHE_KEY,JsonConvert.SerializeObject(transactionsInTheSystem));
            return Ok(new AddTransactionResponse
            {
                ReturnDescription = Constants.OK,
                Request = addTransactionRequest,
            });
        }

    }
}
