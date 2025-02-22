using FCMBBankTransaction.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FCMBBankTransaction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionHistoryController : ControllerBase
    {
        private readonly ITransaction _transactionData;
        public TransactionHistoryController(ITransaction transactionData)
        {
            _transactionData = transactionData;
        }

        [HttpGet]
        public async Task<ActionResult> GetEvents(string accountNumber)
        {
            try
            {
                var transactionHistory = await _transactionData.GetTransactionData(accountNumber);
                return Ok(transactionHistory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
