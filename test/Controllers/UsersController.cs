using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using test.Models;
using test.Services;

namespace test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController: ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // GET Users by page and page size
        // Return Users List

        [HttpGet]
        public ActionResult<List<Users>> Get([FromQuery] UserPagination pagination)
        {
            var page = 0;
            var pageSize = 10;

            if (pagination.GetType().GetProperty("Page") != null)
            {
                page = pagination.Page;
            }

            if (pagination.GetType().GetProperty("PageSize") != null && pagination.PageSize != 0)
            {
                pageSize = pagination.PageSize;
            }

            return _usersService.Get(page, pageSize).ConvertAll(user => new Users(user.Id, user.Login.Username));
        }

    }
}
