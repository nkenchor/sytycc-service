
using System.Security.Cryptography;
using System.Security.Policy;
using MongoDB.Bson.Serialization.Attributes;

namespace Sytycc_Service.Domain;

public class User
{
    [BsonId]
    public string Reference { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; private set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string Role { get; set; }
    public string Bio { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public User(CreateUserDto createUserDto)
    {
        Reference = Guid.NewGuid().ToString();
        FirstName = createUserDto.FirstName;
        LastName = createUserDto.LastName;
        FullName = $"{LastName}, {FirstName}";
        UserName = createUserDto.UserName;
        Role = createUserDto.Role;
        Bio = createUserDto.Bio;
        Email = createUserDto.Email;
        Phone = createUserDto.Phone;

    }
    public User(UpdateUserDto updateUserDto)
    {
        LastName = updateUserDto.LastName;
        FirstName = updateUserDto.FirstName;
        FullName = $"{LastName}, {FirstName}";
        UserName = updateUserDto.UserName;
        Role = updateUserDto.Role;
        Bio = updateUserDto.Bio;
        Email = updateUserDto.Email;
        Phone = updateUserDto.Phone;

    }
    public User()
    {
        
    }
   
}

   
