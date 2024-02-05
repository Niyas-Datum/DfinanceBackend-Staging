using Dfinance.api.Authorization;

using Dfinance.AuthApplication.Services.Interface;

using Dfinance.Shared.Routes.v1;

using Microsoft.AspNetCore.Mvc;



namespace Dfinance.api.Controllers.v1.DMain.Miscellaneous

{

    [ApiController]

    [Authorize]

    public class PasswordController : Controller

    {

        private readonly IPasswordService _passwordService;



        public PasswordController(IPasswordService passwordService)

        {

            _passwordService = passwordService;

        }



        [HttpGet(ApiRoutes.Password.GetPassword)]

        public IActionResult Password(string password)

        {

            try

            {
                var pass = _passwordService.IsPasswordOk(password);

                return Ok(pass);

            }

            catch (Exception ex)

            {



                return BadRequest(ex.Message);

            }

        }





    }

}