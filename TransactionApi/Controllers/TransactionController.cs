using BLL.Services.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TransactionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly UserManager<User> _userManager;
        private readonly ICSVService _cSVService;

        public TransactionController(
            ITransactionService transactionService,
            UserManager<User> userManager,
            ICSVService cSVService)
        {
            _transactionService = transactionService;
            _userManager = userManager;
            _cSVService = cSVService;
        }

        [HttpPost("LoadExcelFile")]
        public async Task<IActionResult> LoadFile(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) == ".xlsx")
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                await _transactionService.LoadDataFromExcel(file.OpenReadStream(), user.Id);
                return Ok();
            }

            return BadRequest("Accept only xlsx files");
        }

        [HttpPost("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                await _transactionService.ChangeStatus(id, user.Id, status);
                return Ok();
            }
            catch ( ArgumentException)
            {
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetCsvFile")]
        public async Task<IActionResult> GetCsvFile()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var file = await _cSVService.GetCsvFileStream(user.Id);
            return File(file, "text/csv", "transactions.csv");
        }
    }
}
