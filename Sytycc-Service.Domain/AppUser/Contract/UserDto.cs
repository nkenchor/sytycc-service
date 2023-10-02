
using MongoDB.Bson.Serialization.Attributes;

namespace Sytycc_Service.Domain;

public class UserDto
{
    public string Reference { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    
    public string Role { get; set; }
    public string Bio { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
public class CreateUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string Bio { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

}
public class UpdateUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string Bio { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

}

public class LoginUserDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}


