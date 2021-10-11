using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class UserLogin
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class UserLoginResponse
    {
        public UserLoginResponse(string token)
        {
            Token = token;
        }

        public string Token { get; set; }
    }
}