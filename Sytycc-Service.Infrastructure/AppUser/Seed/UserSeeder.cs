using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sytycc_Service.Domain;
using Sytycc_Service.Infrastructure;

namespace Sytycc_Service.Api;

public class UserSeeder
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordProvider _passwordProvider;

    public UserSeeder(IUserRepository userRepository, IPasswordProvider passwordProvider)
    {
        _userRepository = userRepository;
        _passwordProvider = passwordProvider;
    }

    public async Task SeedUserAsync()
    {
        var userDtos = new List<CreateUserDto>
        {
            new CreateUserDto
            {
                LastName = "Nonso",
                FirstName = "Sapphire",
                UserName  = "sapphire",
                Password = "nimrod",
                Role = "Administrator",
                Bio = "Administrator",
                Email = "admin@sytycc.com",
                Phone = "+447876042889",
            }
        };
       

        foreach (var userDto in userDtos)
        {
            var (passwordHash, passwordSalt) = _passwordProvider.CreatePasswordHash(userDto.Password);
          
            var user = new User(userDto)
            {
                Reference = Guid.NewGuid().ToString(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };
            await _userRepository.CreateUser(user);
        }
    }
}
