using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace test.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("name")]
        public UserName Name { get; set; }

        [BsonElement("location")]
        public UserLocation Location { get; set; }

        [BsonElement("login")]
        public UserLoginDetails Login { get; set; }

        [BsonElement("phone")]
        public dynamic Phone { get; set; }

        [BsonElement("nat")]
        public string Nationality { get; set; }

        [BsonElement("picture")]
        public UserPicture Picture { get; set; }
    }

    public class UserName
    {
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("first")]
        public string First { get; set; }
        [BsonElement("last")]
        public string Last { get; set; }
    }

    public class UserLocation
    {
        [BsonElement("city")]
        public string City { get; set; }
        [BsonElement("state")]
        public string State { get; set; }
        [BsonElement("country")]
        public string Country { get; set; }
        [BsonElement("postcode")]
        public dynamic Postcode { get; set; }
        [BsonElement("street")]
        public UserLocationStreet Street { get; set; }
    }

    public class UserLocationStreet
    {
        [BsonElement("number")]
        public dynamic Number { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
    }

    public class UserLocationCoordinates
    {
        [BsonElement("latitude")]
        public string Latitude { get; set; }
        [BsonElement("longitude")]
        public string Longitude { get; set; }
    }

    public class UserLocationTimezone
    {
        [BsonElement("offset")]
        public string Offset { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
    }

    public class UserLoginDetails
    {
        [BsonElement("uuid")]
        public string Uuid { get; set; }
        [BsonElement("username")]
        public string Username { get; set; }
        [BsonElement("hash")]
        public string Hash { get; set; }
        public string Password { get; set; }
    }

    public class UserPicture
    {
        [BsonElement("large")]
        public string Large { get; set; }
        [BsonElement("medium")]
        public string Medium { get; set; }
        [BsonElement("thumbnail")]
        public string Thumbnail { get; set; }
    }

    public class UserResponse
    {
        public UserResponse(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Gender = user.Gender;
            Name = user.Name;
            Location = user.Location;
            UserName = user.Login.Username;
            Phone = user.Phone;
            Nationality = user.Nationality;
        }

        public string Id { get; set; }

        public string Email { get; set; }

        public string Gender { get; set; }

        public UserName Name { get; set; }

        public UserLocation Location { get; set; }

        public string UserName { get; set; }

        public string Phone { get; set; }

        public string Nationality { get; set; }
    }

    public class CreateUserResponse
    {
        public CreateUserResponse(string userId)
        {
            Id = userId;
        }

        public string Id { get; set; }
    }
}