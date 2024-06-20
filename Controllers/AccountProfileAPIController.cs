using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.AppServices.AccountProfile;

namespace MyBudgetManagement.Controllers
{
    [ApiController]
    [Route("/api/accountprofile")]
    public class AccountProfileAPIController : ControllerBase
    {

        private readonly IAccountProfileAppService _accountProfileAppService;
        
        public AccountProfileAPIController(IAccountProfileAppService accountProfileAppService)
        {
            _accountProfileAppService = accountProfileAppService;
        }
        
        // GET: AccountProfileController
        [HttpGet("/accountprofile/getaccountprofile")]
        public async Task<IActionResult> GetAccountProfile()
        {
            var accountprofile = _accountProfileAppService.GetAccountProfileDtoAsync().Result;
            return Ok(accountprofile);  
        }

    }
}
