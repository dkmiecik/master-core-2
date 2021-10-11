using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

using test.Models;
using test.Helpers;

namespace test.Services
{
    public interface IUsersService
    {
        List<User> Get(int page, int pageSize);
    }

    public class UsersService : IUsersService
    {
        private readonly IMongoCollection<User> _users;
        private readonly AppSettings _appSettings;

        public UsersService(IUsersStoreDatabaseSettings settings, IOptions<AppSettings> appSettings) 
        {
            _appSettings = appSettings.Value;

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public List<User> Get(int page, int pageSize) =>
            _users.Find(user => true).Skip(page * pageSize).Limit(pageSize).ToList();
    }
}
