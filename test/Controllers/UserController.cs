using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using test.Models;
using test.Services;
using System.Linq;
using System;

namespace test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET User by Id
        // Return StatusCode 200/404

        [HttpGet("{id}")]
        public ActionResult<UserResponse> Get([FromRoute] string id)
        {
            try
            {
                var user = _userService.GetById(id);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                return Ok(new UserResponse(
                        user
                    ));
            } catch (Exception)
            {
                return NotFound();
            }

        }

        // POST Login User with login and password
        // Return StatusCode 200/401 and Authorization Header

        [HttpPost("Login")]
        [AllowAnonymous]
        public ActionResult<User> Login([FromBody] UserLogin user)
        {
            var userData = _userService.Authenticate(user);

            if (userData == null)
            {
                return Unauthorized();
            }

            Response.Headers.Add("Authorization", "Bearer " + userData.Token);
            return Ok(new UserLoginResponse(userData.Token));
        }

        // POST Create User
        // Return StatusCode 200/400

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<CreateUserResponse> CreateUser([FromBody] User body)
        {
            try
            {
                var userId = _userService.CreateUser(body);
                if (userId == null)
                {
                    return BadRequest("User not created");
                }
                return Ok(new CreateUserResponse(userId));
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE User by Id
        // Return StatusCode 200/404

        [HttpDelete("{id}")]
        public ActionResult DeleteUser([FromRoute] string id)
        {
            try
            {
                var user = _userService.GetByIdAndDelete(id);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                return Ok();
            } catch (Exception)
            {
                return NotFound();
            }

        }

        // PUT User by Id, edit User data
        // Return StatusCode 200/400/404 and User

        [HttpPut("{id}")]
        public ActionResult<User> UpdateUser([FromRoute] string id, [FromBody] User body)
        {
            try
            {
                var user = _userService.GetById(id);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                _userService.UpdateUserById(id, body);

                return Ok(user);
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        // PATCH User by Authorization Header
        // Return StatusCode 200/401/404 and User

        [HttpPatch("Me")]
        public ActionResult<string> PatchUser([FromBody] UserEditBody body)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                {
                    return Unauthorized("Invalid token");
                }

                var userId = _userService.DecodeJwtToken(token);
                _userService.ModifyUser(userId, body);

                return Ok();
            } catch (Exception)
            {
                return NotFound();
            }

        }
    }
}

// API:
// GET /isalive
// GET /users
// GET /user/:id
// POST /user/login
// POST /user
// PUT /user/:id
// PATCH /user
// DELETE /user/:id
