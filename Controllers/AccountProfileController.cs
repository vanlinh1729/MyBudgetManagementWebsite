using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.AppServices.AccountProfile;

namespace MyBudgetManagement.Controllers
{
    [ApiController]
    [Route("/api/accountprofile")]
    public class AccountProfileController : ControllerBase
    {

        private readonly IAccountProfileAppService _accountProfileAppService;
        
        public AccountProfileController(IAccountProfileAppService accountProfileAppService)
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
