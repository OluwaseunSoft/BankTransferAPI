using FCMBBankTransaction.Interface;
using FCMBBankTransaction.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FCMBBankTransaction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransfer _transfer;
        public TransferController(ITransfer transfer)
        {
            _transfer = transfer;
        }
        [HttpPost]
        public async Task<ActionResult> DoTransfer(TransferRequest doTransfer)
        {
            try
            {
                var result = await _transfer.DoTransfer(doTransfer);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
