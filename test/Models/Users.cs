using System;
namespace test.Models
{
    public class Users
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public Users(string id, string userName)
        {
            Id = id;
            UserName = userName;
        }
    }

    public class UserPagination
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
