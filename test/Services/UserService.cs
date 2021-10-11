using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
 
using System;
using System.Security.Claims;
using System.Text;
using MongoDB.Driver;
using System.Linq;

using test.Models;
using test.Helpers;
using System.IdentityModel.Tokens.Jwt;
using MongoDB.Bson;

namespace test.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(UserLogin model);
        User GetById(string id);
        User GetByIdAndDelete(string id);
        string CreateUser(User user);
        void UpdateUserById(string id, User user);
        void ModifyUser(string id, UserEditBody user);
        string DecodeJwtToken(string token);
    }

    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly AppSettings _appSettings;

        public UserService(IUsersStoreDatabaseSettings settings, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public AuthenticateResponse Authenticate(UserLogin model)
        {
            try
            {
                var user = GetByEmail(model.Login);

                // return null if user not found
                if (user == null) return null;

                bool verified = BCrypt.Net.BCrypt.Verify(model.Password, user.Login.Hash);
                if (verified == true)
                {
                    // authentication successful so generate jwt token
                    var token = generateJwtToken(user);
                    return new AuthenticateResponse(user, token);
                }

                return null;
            } catch (Exception)
            {
                return null;
            }

        }

        public User GetByEmail(string email) =>
            _users.Find(user => user.Email == email).FirstOrDefault();

        public User GetById(string id) =>
             _users.Find(user => user.Login.Uuid == id).FirstOrDefault();

        public User GetByIdAndDelete(string id) =>
            _users.FindOneAndDelete(user => user.Login.Uuid == id);

        public User GetByUserName(string username) =>
            _users.Find(user => user.Login.Username == username).FirstOrDefault();

        public void UpdateUserById(string id, User user)
        {
            var update = GetById(id);
            user.Id = update.Id;

            if (user.GetType().GetProperty("Password") != null)
            {
                var hash = BCrypt.Net.BCrypt.HashPassword(user.Login.Password, 10);
                user.Login.Hash = hash;
                user.Login.Password = null;
            }
            _users.ReplaceOne(u => u.Login.Uuid == id, user);
        }


        public string CreateUser(User user)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(user.Login.Password, 10);
            user.Login.Hash = hash;
            user.Login.Password = null;
            _users.InsertOne(user);
            return user.Login.Uuid;
        }
            

        public void ModifyUser(string id, UserEditBody user)
        {
            var filter = Builders<User>.Filter.Eq("Login.Uuid", id);
            var update = Builders<User>.Update.Set("Login.Username", user.Username);
            _users.UpdateOne(filter, update);
        }

        public string DecodeJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            var id = tokenS.Claims.First(claim => claim.Type == "id").Value;

            return id;
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Login.Uuid.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
