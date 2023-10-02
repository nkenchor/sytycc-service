using System.Data;
using Sytycc_Service.Domain;
using Serilog;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Sytycc_Service.Api;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserValidationService _userValidationService;
    private readonly IPasswordProvider _passwordProvider;

    public UserService(IUserRepository userRepository, IUserValidationService userValidationService, IPasswordProvider passwordProvider)
    {
        _userRepository = userRepository;
        _userValidationService = userValidationService;
        _passwordProvider = passwordProvider;
    }

    public async Task<string> CreateUser(CreateUserDto userDto)
    {
        try
        {
            var validationException = _userValidationService.ValidateCreateUser(userDto);
            if (validationException != null) throw validationException;

            var availableUser = await _userRepository.GetUserByUserName($"{userDto.UserName}");
            if (availableUser != null)
            {
                Log.Warning($"There is already a user found with the given username: {userDto.UserName}.");
                throw new ConflictException($"there is already a user found with the given username: {userDto.UserName}.");
            }
            
            var user = new User(userDto);
            var (passwordHash, passwordSalt) = _passwordProvider.CreatePasswordHash(userDto.Password);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return await _userRepository.CreateUser(user);
        }
        catch (BadRequestException e)
        {
            Log.Error($"Bad Request Error: {e.Message}");
            throw;
        }
        catch (ConflictException e)
        {
            Log.Error($"Conflict Error: {e.Message}");
            throw;
        }
        catch (AppException e)
        {
            Log.Error($"Database Error: {e.Message}");
            throw;
        }

        // General error (this should be last in the catch sequence)
        catch (Exception e)
        {
            Log.Error($"Error Creating User: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<string> UpdateUser(string reference, UpdateUserDto userDto)
    {
        try
        {
        var validationException = _userValidationService.ValidateUpdateUser(userDto);
        if (validationException != null) throw validationException;

        await GetUserByReference(reference);

        var user = new User(userDto)
        {
            Reference = reference
        };

        var availableUser = await _userRepository.GetUserByUserName($"{userDto.UserName}");
        if (availableUser != null)
        {
            Log.Warning($"There is already a user found with the given username: {userDto.UserName}.");
            throw new ConflictException($"there is already a user found with the given username: {userDto.UserName}.");
        }
            
      
        var (passwordHash, passwordSalt) = _passwordProvider.CreatePasswordHash(userDto.Password);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;


        return await _userRepository.UpdateUser(reference, user);
        }
        catch (BadRequestException e)
        {
            Log.Error($"Bad Request Error: {e.Message}");
            throw;
        }
        catch (AppException e)
        {
            Log.Error($"Database Error: {e.Message}");
            throw;
        }

        // General error (this should be last in the catch sequence)
        catch (Exception e)
        {
            Log.Error($"Error Updating User: {e.Message}");
            throw new InternalServerException(e.Message);
        }

    }

    public async Task<string> DeleteUser(string reference)
    {
        try
        {
        return await _userRepository.DeleteUser(reference);
        }
        catch (BadRequestException e)
        {
            Log.Error($"Bad Request Error: {e.Message}");
            throw;
        }
        catch (AppException e)
        {
            Log.Error($"Database Error: {e.Message}");
            throw;
        }

        // General error (this should be last in the catch sequence)
        catch (Exception e)
        {
            Log.Error($"Error Deleting User: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<UserDto> GetUserByReference(string reference)
    {
        try
        {
            var user = await _userRepository.GetUserByReference(reference) ?? throw new NotFoundException("User not found by the given reference.");
            return new UserDto
            {
                Reference = user.Reference,
                FullName = user.FullName,
                Bio = user.Bio,
                Role = user.Role,
                Email = user.Email,
                Phone = user.Phone,
                UserName = user.UserName,
   
            };
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching user by reference: {e.Message}");
        }
    }
  public async Task<UserDto> GetUserByUserName(string username)
    {
        try
        {
            var user = await _userRepository.GetUserByUserName(username) ?? throw new NotFoundException("User not found by the given username.");
            return new UserDto
            {
                Reference = user.Reference,
                FullName = user.FullName,
                Bio = user.Bio,
                Role = user.Role,
                Email = user.Email,
                Phone = user.Phone,
                UserName = user.UserName,
         
            };
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching user by fullname: {e.Message}");
        }
    }

    public async Task<List<UserDto>> GetUserList(int page)
    {
        try
        {
            var users = await _userRepository.GetUserList(page);
            
            if (users == null || !users.Any())
                throw new NotFoundException("No users found for the given page.");

            return users.Select(user => new UserDto
            {
                Reference = user.Reference,
                FullName = user.FullName,
                Bio = user.Bio,
                Role = user.Role,
                Email = user.Email,
                Phone = user.Phone,
                UserName = user.UserName,
            }).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching user list: {e.Message}");
        }
    }


    public async Task<List<UserDto>> SearchUserList(int page, string fullname)
    {
        try
        {
            var users = await _userRepository.SearchUserList(page, fullname);
            
            if (users == null || !users.Any())
                throw new NotFoundException("No users found with the given name.");

            return users.Select(user => new UserDto
            {
                Reference = user.Reference,
                FullName = user.FullName,
                Bio = user.Bio,
                Role = user.Role,
                Email = user.Email,
                Phone = user.Phone,
                UserName = user.UserName,

                
            }).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error searching user list: {e.Message}");
        }
    }
    public async Task<string?> Login(LoginUserDto userDto)
    {
        var user = await _userRepository.GetUserByUserName(userDto.UserName);
        if (user == null || !_passwordProvider.VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
            return null;

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Service.TokenKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email),
            
            }),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt16(Service.TokenExpiry)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}

