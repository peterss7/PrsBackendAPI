using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace PrsBackendAPI6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IRepositoryWrapper _repository;

        public AuthenticationController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Authenticate(Uri uri)
        {
            /*
             *  if username and password match, login is successful. i want a proper authentication process, working on that in another project
             * 
             */
        }


    }
}
