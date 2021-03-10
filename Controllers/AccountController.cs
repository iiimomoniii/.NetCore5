using System.Net;
using System.Threading.Tasks;
using Hero_Project.Entities;
using Hero_Project.NetCore5.DTOs.Account;
using Hero_Project.NetCore5.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Hero_Project.NetCore5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Register(RegisterRequest registerRequest)
        {
            var account = registerRequest.Adapt<Account>();
            await accountService.Register(account);
            return StatusCode((int) HttpStatusCode.Created);
        }
        

        [HttpPost("[action]")]
        public async Task<ActionResult> Login(LoginRequest loginRequest)
        {
            var account = await accountService.Login(loginRequest.Username, loginRequest.Password);
            if (account == null) {
                return Unauthorized();
            }
            return Ok(new { token = "asfhdfdfajkh"});
        }

    }
}