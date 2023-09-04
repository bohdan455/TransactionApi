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
        private readonly IExcelService _excelService;
        private readonly UserManager<User> _userManager;

        public TransactionController(IExcelService excelService,UserManager<User> userManager)
        {
            _excelService = excelService;
            _userManager = userManager;
        }

        [HttpPost("LoadExcelFile")]
        public async Task<IActionResult> LoadFile(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) == ".xlsx")
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                await _excelService.LoadFile(file.OpenReadStream(), user.Id);
                return Ok();
            }

            return BadRequest("Accept only xlsx files");
        }
    }
}
